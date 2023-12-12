using TMPro;
using UnityEngine;

namespace Views
{
	public class TargetPoint : MonoBehaviour
	{
		public Transform Root => _root;
		public TextMeshProUGUI Text => _text;

		[SerializeField] private Transform _root;
		[SerializeField] private TextMeshProUGUI _text;
	}
}