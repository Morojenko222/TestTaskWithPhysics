using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Config;
using Events;
using UnityEngine;
using Views;

namespace Movement
{
	public class LerpCharacterMoving
	{
		private EventBus _eventBus;
		private GameParameters _gameParameters;
		private CharView _charView;

		private Queue<Vector2> _targetsPos;
		private bool _movingInProcess;
		
		private TargetReachedEvent _targetReachedEvent;
		private MovementStartedEvent _movementStartedEvent;
		private MovementStoppedEvent _movementStoppedEvent;
		private CancellationTokenSource _tokenSource;
		
		public LerpCharacterMoving(EventBus eventBus, GameParameters gameParameters, CharView charView)
		{
			_eventBus = eventBus;
			_gameParameters = gameParameters;
			_charView = charView;
			
			Initialize();
		}

		private void Initialize()
		{
			_targetsPos = new Queue<Vector2>();
			_targetReachedEvent = new TargetReachedEvent();
			_movementStartedEvent = new MovementStartedEvent();
			_movementStoppedEvent = new MovementStoppedEvent();
			_tokenSource = new CancellationTokenSource();
			
			_eventBus.Subscribe<NewTargetSetEvent> (OnNewTargetSet);
			_eventBus.Subscribe<RequestToStartMovementEvent> (OnMovementStartRequest);
			_eventBus.Subscribe<KeyStateChangedEvent> (OnKeyPressedEvent);
			_eventBus.Subscribe<InterruptMovingEvent> (OnInterruptMovingEvent);
			_eventBus.Subscribe<GameDisposeEvent> (Dispose);
			
		}

		private void OnNewTargetSet(NewTargetSetEvent evt)
		{
			Vector2 inputCoordinates = evt.ScreenCoordinates;
			Vector2 newTargetPos = Camera.main.ScreenToWorldPoint(inputCoordinates);
			_targetsPos.Enqueue(newTargetPos);
		}

		private void OnMovementStartRequest(RequestToStartMovementEvent _)
		{
			if (!_movingInProcess)
			{
				StartMoving();
			}
		}

		private void OnKeyPressedEvent(KeyStateChangedEvent evt)
		{
			if (_movingInProcess
				&& evt.Key == KeyCode.Space
				&& evt.IsPressed)
			{
				InterruptMoving();
			}
		}

		private void OnInterruptMovingEvent(InterruptMovingEvent _)
		{
			if (_movingInProcess)
			{
				InterruptMoving();
			}
		}

		private async Task StartMoving()
		{
			_tokenSource = new CancellationTokenSource();

			while (_targetsPos.Count > 0)
			{
				_movingInProcess = true;
				
				Vector2 charInitPos  = _charView.CharRoot.position;
				Vector2 targetPos = _targetsPos.Dequeue();
				
				using (_movementStartedEvent.SetValue(charInitPos, targetPos))
				{
					_eventBus.Publish(_movementStartedEvent);
				}

				await LerpMove(targetPos);
				
				_eventBus.Publish(_targetReachedEvent);
				_movingInProcess = false;

				if (_tokenSource.Token.IsCancellationRequested)
				{
					_targetsPos.Clear();
					break;
				}
			}
			
			_eventBus.Publish(_movementStoppedEvent);
		}

		private void InterruptMoving()
		{
			if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
			{
				_tokenSource.Cancel();
			}
		}

		private async Task LerpMove(Vector2 targetPos)
		{
			Vector2 charInitPos  = _charView.CharRoot.position;
			float speed = _gameParameters.CharMovementSpeed;
			float delay = 0.01f;
			TimeSpan delaySpan = TimeSpan.FromSeconds(delay);
			
			float distance = (targetPos - charInitPos ).magnitude;
			float timeSec = (distance / speed) ;
			float timePassed = 0f;
			
			while (true)
			{
				float timeDiff = timeSec - timePassed;
				
				if (timeDiff < delay)
				{
					break;
				}

				timePassed += delay;
				
				float lerpCoeff = 1f - timeDiff / timeSec;
				Vector2 newPos = Vector2.Lerp(charInitPos, targetPos, lerpCoeff);
				_charView.CharRoot.position = newPos;
				
				await Task.Delay(delaySpan);
				
				if (_tokenSource.Token.IsCancellationRequested)
					break;
			}
		}

		private void Dispose(GameDisposeEvent _)
		{
			if (_tokenSource != null)
			{
				if (!_tokenSource.IsCancellationRequested)
				{
					_tokenSource.Cancel();
				}

				_tokenSource.Dispose();
			}
		}
	}
}