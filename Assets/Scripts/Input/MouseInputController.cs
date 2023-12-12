using Events;
using General;
using UnityEngine;

namespace Input
{
	public class MouseInputController : IUpdatable
	{
		private EventBus _eventBus;
		private MousePressedEvent _pressedEvent;

		public MouseInputController(EventBus eventBus)
		{
			_eventBus = eventBus;
			Initialize();
		}

		private void Initialize()
		{
			_pressedEvent = new MousePressedEvent();
		}

		public void Update()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				Vector3 mousePosition = UnityEngine.Input.mousePosition;

				using (_pressedEvent.SetValue(mousePosition))
				{
					_eventBus.Publish(_pressedEvent);
				}
			}
		}
	}
}