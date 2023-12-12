using System;
using UnityEngine;

namespace Events
{
	public class MovementStartedEvent : IGameEvent, IDisposable
	{
		public Vector2 InitPosition { get; private set; }
		public Vector2 TargetPosition { get; private set; }

		public MovementStartedEvent SetValue(Vector2 initPosition, Vector2 targetPosition)
		{
			InitPosition = initPosition;
			TargetPosition = targetPosition;
			return this;
		}

		public void Dispose()
		{
			SetValue(default, default);
		}
	}
}