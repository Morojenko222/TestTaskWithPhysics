using Config;
using Events;
using Spine.Unity;
using UnityEngine;
using Views;

namespace Animation
{
	public class CharMovementAnimation
	{
		private EventBus _eventBus;
		private AnimationConfig _animConfig;
		private SceneContext _context;
		
		public CharMovementAnimation(EventBus eventBus, AnimationConfig animConfig, SceneContext context)
		{
			_eventBus = eventBus;
			_animConfig = animConfig;
			_context = context;
			
			Initialize();
		}

		private void Initialize()
		{
			_eventBus.Subscribe<MovementStartedEvent>(OnMovementStartEvent);
			_eventBus.Subscribe<MovementStoppedEvent>(OnMovementStopEvent);

			SetIdleAnimation();
		}

		private void OnMovementStartEvent(MovementStartedEvent evt)
		{
			Vector2 direction = evt.TargetPosition - evt.InitPosition;
			AnimationReferenceAsset anim = GetAnimation(direction);
			SetDirection(anim, direction);

			_context.CharView.CharAnim.AnimationState.SetAnimation(0, anim, true);
		}

		private AnimationReferenceAsset GetAnimation(Vector2 direction)
		{
			return direction.y > 0 ? _animConfig.WalkBack : _animConfig.WalkFront;
		}

		private void SetDirection(AnimationReferenceAsset anim, Vector2 direction)
		{
			Transform charRoot = _context.CharView.CharRoot;
			if (direction.x > 0)
			{
				if (anim == _animConfig.WalkFront)
				{
					charRoot.localScale = new Vector3(-1f, 1f, 1f);
				}
				else if (anim == _animConfig.WalkBack)
				{
					charRoot.localScale = new Vector3(1f, 1f, 1f);
				}
			}
			else
			{
				if (anim == _animConfig.WalkFront)
				{
					charRoot.localScale = new Vector3(1f, 1f, 1f);
				}
				else if (anim == _animConfig.WalkBack)
				{
					charRoot.localScale = new Vector3(-1f, 1f, 1f);
				}
			}
		}

		private void OnMovementStopEvent(MovementStoppedEvent _)
		{
			SetIdleAnimation();
		}

		private void SetIdleAnimation()
		{
			AnimationReferenceAsset idleAnim = _animConfig.Idle;
			_context.CharView.CharAnim.AnimationState.SetAnimation(0, idleAnim, true);
		}
	}
}