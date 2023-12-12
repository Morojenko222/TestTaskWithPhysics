using System.Collections.Generic;
using System.Threading;
using Config;
using Events;
using General;
using UnityEngine;
using Views;
using Task = System.Threading.Tasks.Task;

namespace Projectiles
{
	public class ProjectilesSpawner
	{
		private EventBus _eventBus;
		private GameParameters _gameParameters;
		private Prefabs _prefabs;
		
		private Pool<ProjectileView> _projectilesPool;
		private Queue<Task> _spawnedObjects;
		private ObjectCollisionEvent _collisionEvent;
		private ObjectTriggerEvent _triggerEvent;
		private CancellationTokenSource _gameEndTokenSource;
		
		private bool _inputBlocked;

		public ProjectilesSpawner(EventBus eventBus, GameParameters gameParameters, Prefabs prefabs)
		{
			_eventBus = eventBus;
			_gameParameters = gameParameters;
			_prefabs = prefabs;
			
			Initialize();
		}

		private void Initialize()
		{
			_projectilesPool = new Pool<ProjectileView>(SpawnProjectile, OnReleaseProjectileToPool, OnGetProjectileFromPool);
			_collisionEvent = new ObjectCollisionEvent();
			_triggerEvent = new ObjectTriggerEvent();
			_spawnedObjects = new Queue<Task>();
			_gameEndTokenSource = new CancellationTokenSource();
			
			_eventBus.Subscribe<MousePressedEvent>(OnMousePressedEvent);
			_eventBus.Subscribe<MovementStartedEvent>(OnMovementStartedEvent);
			_eventBus.Subscribe<MovementStoppedEvent>(OnMovementStoppedEvent);
			_eventBus.Subscribe<GameDisposeEvent>(Dispose);

			_inputBlocked = true;
		}

		private void OnMousePressedEvent(MousePressedEvent evt)
		{
			if (!_inputBlocked)
			{
				Vector2 inputCoordinates = evt.PressPosition;
				Vector2 spawnPos = Camera.main.ScreenToWorldPoint(inputCoordinates);

				Shot(spawnPos);
			}
		}

		private void Shot(Vector2 spawnPos)
		{
			ProjectileView view = _projectilesPool.Get();
			view.transform.position = spawnPos;
			view.Rb.AddForce(Vector3.up * _gameParameters.ProjectileShotImpulse, ForceMode2D.Impulse);
			_spawnedObjects.Enqueue(RunReleaseProcess(view));
		}

		// Can be moved to a factory.
		private ProjectileView SpawnProjectile()
		{
			ProjectileView projectilePrefab = _prefabs.Projectile;
			ProjectileView view = GameObject.Instantiate(projectilePrefab);
			
			return view;
		}
		private void OnMovementStartedEvent(MovementStartedEvent _)
		{
			_inputBlocked = false;
		}

		private void OnMovementStoppedEvent(MovementStoppedEvent _)
		{
			_inputBlocked = true;
		}

		private void OnGetProjectileFromPool(ProjectileView view)
		{
			view.Rb.velocity = default;
			view.gameObject.SetActive(true);
			view.CollisionsDetector.OnCollisionEnterEvent += OnProjectileCollision;
			view.CollisionsDetector.OnTriggerEnterEvent += OnProjectileTrigger;
		}
		
		private void OnReleaseProjectileToPool(ProjectileView view)
		{
			view.gameObject.SetActive(false);
			view.CollisionsDetector.OnCollisionEnterEvent -= OnProjectileCollision;
			view.CollisionsDetector.OnTriggerExitEvent -= OnProjectileTrigger;
		}

		private void OnProjectileCollision(GameObject source, Collision2D other)
		{
			using (_collisionEvent.SetValue(source, other))
			{
				_eventBus.Publish(_collisionEvent);
			}
		}

		private void OnProjectileTrigger(GameObject source, Collider2D other)
		{
			using (_triggerEvent.SetValue(source, other))
			{
				_eventBus.Publish(_triggerEvent);
			}
		}

		private async Task RunReleaseProcess(ProjectileView view)
		{
			await Task.Delay(_gameParameters.ProjectileLifetime * 1000, _gameEndTokenSource.Token);
			_spawnedObjects.Dequeue();
			_projectilesPool.Release(view);
		}
		
		private void Dispose(GameDisposeEvent _)
		{
			DisposeAsync();
		}
		
		private async void DisposeAsync()
		{
			_gameEndTokenSource.Cancel();

			while (_spawnedObjects.Count > 0)
			{
				Task task = _spawnedObjects.Dequeue();
				while (!task.IsCanceled)
				{
					await Task.Yield();
				}
			}
			_gameEndTokenSource.Dispose();
		}
	}
}