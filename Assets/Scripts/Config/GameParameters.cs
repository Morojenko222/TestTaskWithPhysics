using UnityEngine;

namespace Config
{
	[CreateAssetMenu(fileName = "GameParameters", menuName = "Configs/GameParameters")]
	public class GameParameters : ScriptableObject
	{
		public float CharMovementSpeed => _charMovementSpeed;
		public int MinimalTargetPointsCounter => _minimalTargetPointsCounter;
		public int InventoryElementsAmount => _inventoryElementsAmount;
		public int ProjectileShotImpulse => _projectileShotImpulse;
		public int ProjectileLifetime => _projectileLifetime;
		public int CollisionFxLifetime => _collisionFxLifetime;

		[SerializeField] private float _charMovementSpeed = 10f;
		[SerializeField] private int _minimalTargetPointsCounter = 4;
		[SerializeField] private int _inventoryElementsAmount = 30;
		[SerializeField] private int _projectileShotImpulse = 10;
		[SerializeField] private int _projectileLifetime = 3;
		[SerializeField] private int _collisionFxLifetime = 3;
	}
}