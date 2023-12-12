using UnityEngine;

namespace Views
{
	public class InventoryElementView : MonoBehaviour
	{
		public RectTransform Root => _root;
		public Transform NotificationMarker => _notificationMarker;

		[SerializeField] private RectTransform _root;
		[SerializeField] private Transform _notificationMarker;
	}
}