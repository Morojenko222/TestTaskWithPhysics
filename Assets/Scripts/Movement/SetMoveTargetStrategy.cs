using UnityEngine;

namespace Movement
{
	public abstract class SetMoveTargetStrategy
	{
		protected abstract void SetTargetPoint(Vector2 screenCoordinates);
	}
}