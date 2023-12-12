using System;
using UnityEngine;

namespace Collisions
{
	public class CollisionsDetector : MonoBehaviour
	{
		public event Action<GameObject, Collision2D> OnCollisionEnterEvent;
		public event Action<GameObject, Collision2D> OnCollisionExitEvent;
		public event Action<GameObject, Collider2D> OnTriggerEnterEvent;
		public event Action<GameObject, Collider2D> OnTriggerExitEvent;

		private void OnCollisionEnter2D(Collision2D other)
		{
			OnCollisionEnterEvent?.Invoke(gameObject, other);
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			OnCollisionExitEvent?.Invoke(gameObject, other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			OnTriggerEnterEvent?.Invoke(gameObject, other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			OnTriggerExitEvent?.Invoke(gameObject, other);
		}

		private void OnDestroy()
		{
			OnCollisionEnterEvent = null;
			OnCollisionExitEvent = null;
			OnTriggerEnterEvent = null;
			OnTriggerExitEvent = null;
		}
	}
}