using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

namespace Movement
{
	public class SetMoveTargetOnEventTriggerPressStrategy : SetMoveTargetStrategy
	{
		private EventBus _eventBus;
		private SceneContext _context;
		private NewTargetSetEvent _targetSetEvent;
		private EventTrigger.Entry _onPressEntry;
		private bool _inputBlocked;
		
		public SetMoveTargetOnEventTriggerPressStrategy(EventBus eventBus, SceneContext context)
		{
			_eventBus = eventBus;
			_context = context;
			
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
			_onPressEntry = new EventTrigger.Entry(); 
			_onPressEntry.eventID = EventTriggerType.PointerDown;
			_onPressEntry.callback.AddListener(OnAreaPress);
			_context.PressArea.triggers.Add(_onPressEntry);

			_targetSetEvent = new NewTargetSetEvent();
			
			_eventBus.Subscribe<MovementStartedEvent> (OnMovementStartedEvent);
			_eventBus.Subscribe<MovementStoppedEvent> (OnMovementStoppedEvent);
			_eventBus.Subscribe<GameDisposeEvent> (Dispose);
		}

		private void OnAreaPress(BaseEventData data)
		{
			if (_inputBlocked) return;
			
			Vector2 worldPoint = data.currentInputModule.input.mousePosition;
			SetTargetPoint(worldPoint);
		}

		private void OnMovementStartedEvent(MovementStartedEvent _)
		{
			_inputBlocked = true;
		}

		private void OnMovementStoppedEvent(MovementStoppedEvent _)
		{
			_inputBlocked = false;
		}
		
		private void Dispose(GameDisposeEvent _)
		{
			_onPressEntry.callback = null;
		}
	}
}