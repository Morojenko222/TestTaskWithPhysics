using System;
using UnityEngine;

namespace Events
{
	public class ObjectCollisionEvent : IGameEvent, IDisposable
	{
		public GameObject Source { get; private set; }
		public Collision2D Other { get; private set; }

		public ObjectCollisionEvent SetValue(GameObject source, Collision2D other)
		{
			Source = source;
			Other = other;
			return this;
		}

		public void Dispose()
		{
			Source = default;
			Other = default;
		}
	}
}