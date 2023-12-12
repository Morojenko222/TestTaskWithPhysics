using Spine.Unity;
using UnityEngine;

namespace Views
{
	public class CharView : MonoBehaviour
	{
		public Transform CharRoot => _charRoot;
		public SkeletonAnimation CharAnim => _charAnim;

		[SerializeField] private Transform _charRoot;
		[SerializeField] private SkeletonAnimation _charAnim;
	}
}