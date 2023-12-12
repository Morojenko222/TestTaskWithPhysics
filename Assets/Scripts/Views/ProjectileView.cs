using Collisions;
using UnityEngine;

namespace Views
{
	public class ProjectileView : MonoBehaviour
	{
		public Rigidbody2D Rb => _rb;
		public CollisionsDetector CollisionsDetector => _collisionsDetector;

		[SerializeField] private Rigidbody2D _rb;
		[SerializeField] private CollisionsDetector _collisionsDetector;
	}
}