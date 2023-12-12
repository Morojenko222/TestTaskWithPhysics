using UnityEngine;
using UnityEngine.EventSystems;

namespace Views
{
	public class SceneContext : MonoBehaviour
	{
		public CharView CharView => _charView;
		public Transform TargetPointsContainer => _targetPointsContainer;
		public EventTrigger PressArea => _pressArea;
		public UiControlViews ControlViews => _controlViews;

		[SerializeField] private CharView _charView;
		[SerializeField] private Transform _targetPointsContainer;
		[SerializeField] private EventTrigger _pressArea;
		[SerializeField] private UiControlViews _controlViews;
	}
}