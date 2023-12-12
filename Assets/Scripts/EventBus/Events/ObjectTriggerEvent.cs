using System;
using UnityEngine;

namespace Events
{
	public class ObjectTriggerEvent : IGameEvent, IDisposable
	{
		public GameObject Source { get; private set; }
		public Collider2D Other { get; private set; }

		public ObjectTriggerEvent SetValue(GameObject source, Collider2D other)
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