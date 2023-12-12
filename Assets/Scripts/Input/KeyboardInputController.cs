using Events;
using General;
using UnityEngine;

namespace Input
{
	public class KeyboardInputController : IUpdatable
	{
		private EventBus _eventBus;
		private KeyStateChangedEvent _keyStateChangedEvent;

		public KeyboardInputController(EventBus eventBus)
		{
			_eventBus = eventBus;
			
			Initialize();
		}

		private void Initialize()
		{
			_keyStateChangedEvent = new KeyStateChangedEvent();
		}

		public void Update()
		{
			CheckSpaceButton();
		}

		// Can be refactored if it requires more buttons to control
		private void CheckSpaceButton()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				using (_keyStateChangedEvent.SetValues(KeyCode.Space, true))
				{
					_eventBus.Publish(_keyStateChangedEvent);
				}
			}
		}

	}
}