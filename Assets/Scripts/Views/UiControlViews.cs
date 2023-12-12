using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
	public class UiControlViews : MonoBehaviour
	{
		public Button StartMovementBtn => _startMovementBtn;
		public TextMeshProUGUI MovingLabel => _movingLabel;
		public InventoryWindowViews InventoryWindowViews => _inventoryWindowViews;

		[SerializeField] private Button _startMovementBtn;
		[SerializeField] private TextMeshProUGUI _movingLabel;
		[SerializeField] private InventoryWindowViews _inventoryWindowViews;
	}
}