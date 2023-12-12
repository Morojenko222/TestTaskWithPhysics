using System;
using UnityEngine;

namespace Events
{
	public class KeyStateChangedEvent : IGameEvent, IDisposable
	{
		public KeyCode Key { get; private set; }
		public bool IsPressed { get; private set; }

		public KeyStateChangedEvent SetValues(KeyCode key, bool isPressed)
		{
			Key = key;
			IsPressed = isPressed;
			return this;
		}

		public void Dispose()
		{
			Key = default;
			IsPressed = default;
		}
	}
}