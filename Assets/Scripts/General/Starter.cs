using Config;
using UnityEngine;
using Views;

namespace General
{
	public class Starter : MonoBehaviour
	{
		[SerializeField] private SceneContext _context;
		[SerializeField] private MainConfig _mainConfig;

		private GameManager _gameManager;

		private void Start()
		{
			_gameManager = new GameManager(_context, _mainConfig);
		}

		private void Update()
		{
			_gameManager.Update();
		}

		private void OnDestroy()
		{
			_gameManager.Dispose();
		}
	}
}