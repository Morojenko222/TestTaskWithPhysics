using UnityEngine;

namespace Config
{
	[CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
	public class MainConfig : ScriptableObject
	{
		public Prefabs Prefabs => _prefabs;
		public GameParameters GameParameters => _gameParameters;
		public AnimationConfig Animations => _animations;

		[SerializeField] private Prefabs _prefabs;
		[SerializeField] private GameParameters _gameParameters;
		[SerializeField] private AnimationConfig _animations;
	}
}