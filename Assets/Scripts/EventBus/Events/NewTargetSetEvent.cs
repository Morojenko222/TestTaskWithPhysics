using System;
using UnityEngine;

namespace Events
{
	public class NewTargetSetEvent : IGameEvent, IDisposable
	{
		public Vector2 ScreenCoordinates { get; private set; }

		public NewTargetSetEvent SetValue(Vector2 newValue)
		{
			ScreenCoordinates = newValue;
			return this;
		}

		public void Dispose()
		{
			ScreenCoordinates = default;
		}
	}
}