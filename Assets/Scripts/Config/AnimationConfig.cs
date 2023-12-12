using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
	[CreateAssetMenu(fileName = "AnimationConfig", menuName = "Configs/AnimationConfig")]
	public class AnimationConfig : ScriptableObject
	{
		public AnimationReferenceAsset WalkFront => _walkFront;
		public AnimationReferenceAsset WalkBack => _walkBack;
		public AnimationReferenceAsset Idle => idle;

		[SerializeField] private AnimationReferenceAsset _walkFront;
		[SerializeField] private AnimationReferenceAsset _walkBack;
		[SerializeField] private AnimationReferenceAsset idle;
	}
}