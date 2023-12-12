using System;
using UnityEngine;

namespace Events
{
	public class MousePressedEvent : IGameEvent, IDisposable
	{
		public Vector2 PressPosition { get; private set; }

		public MousePressedEvent SetValue(Vector2 newValue)
		{
			PressPosition = newValue;
			return this;
		}

		public void Dispose()
		{
			PressPosition = default;
		}
	}
}