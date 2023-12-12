using UnityEngine;
using Views;

namespace Config
{
	[CreateAssetMenu(fileName = "ViewPrefabs", menuName = "Configs/ViewPrefabs")]
	public class Prefabs : ScriptableObject
	{
		public CharView CharView => _charView;
		public TargetPoint TargetPoint => _targetPoint;
		public InventoryElementView InventoryElementView => _inventoryElementView;
		public ProjectileView Projectile => _projectile;
		public GameObject ProjectileCollisionFx => _projectileCollisionFx;

		[SerializeField] private CharView _charView;
		[SerializeField] private TargetPoint _targetPoint;
		[SerializeField] private InventoryElementView _inventoryElementView;
		[SerializeField] private ProjectileView _projectile;
		[SerializeField] private GameObject _projectileCollisionFx;
	}
}