using UnityEngine;
using UnityEngine.UI;

namespace Views
{
	public class InventoryWindowViews : MonoBehaviour
	{
		public Transform Root => _root;
		public ScrollRect ScrollRect => _scrollRect;
		public RectTransform Viewport => _viewport;
		public RectTransform ElementContainer => _elementContainer;
		public VerticalLayoutGroup VerticalLayout => _verticalLayout;
		public Button OpenWindowBtn => _openWindowBtn;
		public Button CloseWindowBtn => _closeWindowBtn;
		public Button RefreshNotifications => _refreshNotifications;

		[SerializeField] private Transform _root;
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private RectTransform _viewport;
		[SerializeField] private RectTransform _elementContainer;
		[SerializeField] private VerticalLayoutGroup _verticalLayout;
		[SerializeField] private Button _openWindowBtn;
		[SerializeField] private Button _closeWindowBtn;
		[SerializeField] private Button _refreshNotifications;
	}
}