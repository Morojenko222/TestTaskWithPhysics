using System.Collections.Generic;
using Config;
using Events;
using UnityEngine;
using Views;

namespace UI.TargetPoints
{
	public class UiTargetPointsCreator
	{
		private EventBus _eventBus;
		private Prefabs _prefabs;
		private Transform _targetPointsContainer;
		
		private Queue<TargetPoint> _targetsPos;
		private bool _movingInProcess;

		public UiTargetPointsCreator(EventBus eventBus, Prefabs prefabs, Transform targetPointsContainer)
		{
			_eventBus = eventBus;
			_prefabs = prefabs;
			_targetPointsContainer = targetPointsContainer;
			
			Initialize();
		}

		private void Initialize()
		{
			_targetsPos = new Queue<TargetPoint>();
			
			_eventBus.Subscribe<NewTargetSetEvent> (CreateUiTargetPoint);
			_eventBus.Subscribe<TargetReachedEvent> (OnTargetReachedEvent);
			_eventBus.Subscribe<MovementStoppedEvent> (OnMovementStopEvent);
		}

		// Can be polled, currently objects initialize as new objects for simplicity
		private void CreateUiTargetPoint(NewTargetSetEvent evt)
		{
			TargetPoint targetPointPrefab = _prefabs.TargetPoint;
			Vector2 inputCoordinates = evt.ScreenCoordinates;

			TargetPoint newTarget = Object.Instantiate(
				targetPointPrefab,
				inputCoordinates,
				Quaternion.identity,
				_targetPointsContainer);
			
			newTarget.Text.text = (_targetsPos.Count + 1).ToString();
			
			_targetsPos.Enqueue(newTarget);
		}

		private void OnTargetReachedEvent(TargetReachedEvent _)
		{
			RemoveCurrentTargetPoint();
		}

		private void RemoveCurrentTargetPoint()
		{
			if (_targetsPos.TryDequeue(out TargetPoint point))
			{
				Object.Destroy(point.gameObject);
			}
		}

		private void OnMovementStopEvent(MovementStoppedEvent _)
		{
			while (_targetsPos.Count > 0)
			{
				TargetPoint point = _targetsPos.Dequeue();
				Object.Destroy(point.gameObject);
			}
		}
	}
}