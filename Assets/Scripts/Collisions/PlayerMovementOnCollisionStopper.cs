using DefaultNamespace;
using Events;

namespace Collisions
{
	public class PlayerMovementOnCollisionStopper
	{
		private EventBus _eventBus;
		private InterruptMovingEvent _interruptMovingEvent;

		public PlayerMovementOnCollisionStopper (EventBus eventBus)
		{
			_eventBus = eventBus;
			
			Initialize();
		}

		private void Initialize()
		{
			_interruptMovingEvent = new InterruptMovingEvent();
			
			_eventBus.Subscribe<ObjectTriggerEvent> (OnObjectTriggerEvent);
		}

		private void OnObjectTriggerEvent(ObjectTriggerEvent evt)
		{
			if (evt.Other.CompareTag(TagsNames.PLAYER))
			{
				_eventBus.Publish(_interruptMovingEvent);
			}
		}
	}
}