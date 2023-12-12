using System.Collections.Generic;
using Animation;
using Collisions;
using Config;
using Events;
using Input;
using Movement;
using Projectiles;
using UI.Inventory;
using UI.TargetPoints;
using Views;

namespace General
{
	public class GameManager
	{
		private SceneContext _context;
		private MainConfig _mainConfig;

		private EventBus _eventBus;

		private MouseInputController _mouseInput;
		private KeyboardInputController _keyboardInputController;
		private SetMoveTargetStrategy _moveTargetStrategy;
		private LerpCharacterMoving _lerpCharacterMoving;
		private UiTargetPointsCreator _uiTargetPointsCreator;
		private CharMovementAnimation _charMovementAnimation;
		private MovementProcessController _movementProcessController;
		private InventoryFlowController _inventoryFlowController;
		private ProjectilesSpawner _projectilesSpawner;
		private FxOnProjectileCollisionsSpawner _fxOnProjectileCollisionsSpawner;
		private PlayerMovementOnCollisionStopper playerMovementOnCollisionStopper;

		private List<IUpdatable> _updatables;

		public GameManager(SceneContext context, MainConfig mainConfig)
		{
			_context = context;
			_mainConfig = mainConfig;

			Initialize();
		}

		private void Initialize()
		{
			_eventBus = new EventBus();
			_mouseInput = new MouseInputController(_eventBus);
			_keyboardInputController = new KeyboardInputController(_eventBus);
			_moveTargetStrategy = new SetMoveTargetOnEventTriggerPressStrategy(_eventBus, _context);
			
			_lerpCharacterMoving = new LerpCharacterMoving(
				_eventBus,
				_mainConfig.GameParameters,
				_context.CharView);
			
			_uiTargetPointsCreator = new UiTargetPointsCreator(
					_eventBus,
					_mainConfig.Prefabs,
					_context.TargetPointsContainer);
			
			_charMovementAnimation = new CharMovementAnimation(
				_eventBus,
				_mainConfig.Animations,
				_context);
			
			_movementProcessController = new MovementProcessController(
					_eventBus,
					_mainConfig.GameParameters,
					_context.ControlViews);

			_inventoryFlowController = new InventoryFlowController(
				_eventBus,
				_mainConfig.GameParameters,
				_mainConfig.Prefabs,
				_context.ControlViews.InventoryWindowViews);

			_projectilesSpawner = new ProjectilesSpawner(
				_eventBus,
				_mainConfig.GameParameters,
				_mainConfig.Prefabs);

			_fxOnProjectileCollisionsSpawner = new FxOnProjectileCollisionsSpawner(
				_eventBus,
				_mainConfig.GameParameters,
				_mainConfig.Prefabs);
			
			playerMovementOnCollisionStopper = new PlayerMovementOnCollisionStopper(_eventBus);
			
			_updatables = new List<IUpdatable> { _mouseInput, _keyboardInputController};
		}

		public void Update()
		{
			foreach (IUpdatable updatable in _updatables)
			{
				updatable.Update();
			}
		}

		public void Dispose()
		{
			_eventBus.Publish(new GameDisposeEvent());
			_eventBus.Dispose();
		}
	}
}