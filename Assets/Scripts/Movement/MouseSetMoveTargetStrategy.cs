using Events;
using UnityEngine;

namespace Movement
{
	public class MouseSetMoveTargetStrategy : SetMoveTargetStrategy
	{
		private EventBus _eventBus;
		private NewTargetSetEvent _targetSetEvent;
		
		public MouseSetMoveTargetStrategy(EventBus eventBus)
		{
			_eventBus = eventBus;
			
			Initialize();
		}

		protected override void SetTargetPoint(Vector2 screenCoordinates)
		{
			using (_targetSetEvent.SetValue(screenCoordinates))
			{
				_eventBus.Publish(_targetSetEvent);
			}
		}

		private void Initialize()
		{
			_eventBus.Subscribe<MousePressedEvent>(OnMousePressEvent);
			_targetSetEvent = new NewTargetSetEvent();
		}

		private void OnMousePressEvent(MousePressedEvent evt)
		{
			SetTargetPoint(evt.PressPosition);
		}
	}
}