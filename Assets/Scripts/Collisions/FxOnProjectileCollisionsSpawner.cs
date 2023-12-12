using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Config;
using Events;
using General;
using UnityEngine;

namespace Collisions
{
	public class FxOnProjectileCollisionsSpawner
	{
		private EventBus _eventBus;
		private GameParameters _gameParameters;
		private Prefabs _prefabs;
		
		private Pool<GameObject> _collisionEffectsPool;
		private Queue<Task> _spawnedObjects;
		private CancellationTokenSource _gameEndTokenSource;
		
		public FxOnProjectileCollisionsSpawner(EventBus eventBus, GameParameters gameParameters, Prefabs prefabs)
		{
			_eventBus = eventBus;
			_gameParameters = gameParameters;
			_prefabs = prefabs;
			
			Initialize();
		}

		private void Initialize()
		{
			_collisionEffectsPool = new Pool<GameObject>(SpawnFx, OnReleaseFxToPool, OnGetFxFromPool);
			_spawnedObjects = new Queue<Task>();
			_gameEndTokenSource = new CancellationTokenSource();
			
			_eventBus.Subscribe<ObjectCollisionEvent> (OnObjectsCollision);
			_eventBus.Subscribe<GameDisposeEvent> (Dispose);
		}

		private void OnObjectsCollision(ObjectCollisionEvent evt)
		{
			GameObject fx = _collisionEffectsPool.Get();
			fx.transform.position = evt.Other.contacts[0].point;

			_spawnedObjects.Enqueue(RunReleaseProcess(fx));
		}
		
		// Can be moved to a factory.
		private GameObject SpawnFx()
		{
			GameObject fxPrefab = _prefabs.ProjectileCollisionFx;
			GameObject fxObject = GameObject.Instantiate(fxPrefab);
			
			return fxObject;
		}
		
		private async Task RunReleaseProcess(GameObject view)
		{
			await Task.Delay(_gameParameters.CollisionFxLifetime * 1000, _gameEndTokenSource.Token);
			_spawnedObjects.Dequeue();
			_collisionEffectsPool.Release(view);
		}
		
		private void OnGetFxFromPool(GameObject view)
		{
			view.gameObject.SetActive(true);
		}
		
		private void OnReleaseFxToPool(GameObject view)
		{
			view.gameObject.SetActive(false);
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