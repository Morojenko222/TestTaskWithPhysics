using Config;
using Events;
using Views;

namespace Movement
{
	public class MovementProcessController
	{
		private EventBus _eventBus;
		private GameParameters _gameParameters;
		private UiControlViews _views;

		private int _targetPointsCounter;
		private RequestToStartMovementEvent _startMovementEvent;
		
		public MovementProcessController(EventBus eventBus, GameParameters gameParameters, UiControlViews views)
		{
			_eventBus = eventBus;
			_gameParameters = gameParameters;
			_views = views;
			
			Initialize();
		}

		private void Initialize()
		{
			_startMovementEvent = new RequestToStartMovementEvent();
			
			_views.StartMovementBtn.onClick.AddListener(OnRunButtonPress);
			_views.StartMovementBtn.gameObject.SetActive(false);
			_views.MovingLabel.gameObject.SetActive(false);
			
			_eventBus.Subscribe<NewTargetSetEvent> (OnTargetSetEvent);
			_eventBus.Subscribe<MovementStoppedEvent> (OnMovementStoppedEvent);
			_eventBus.Subscribe<GameDisposeEvent> (Dispose);
		}

		private void OnTargetSetEvent(NewTargetSetEvent evt)
		{
			_targetPointsCounter += 1;
			UpdateRunMovementButtonState();
		}

		private void OnMovementStoppedEvent(MovementStoppedEvent _)
		{
			_views.MovingLabel.gameObject.SetActive(false);
		}

		private void UpdateRunMovementButtonState()
		{
			bool targetsEnoughToRun = _targetPointsCounter >= _gameParameters.MinimalTargetPointsCounter;
			_views.StartMovementBtn.gameObject.SetActive(targetsEnoughToRun);
		}

		private void OnRunButtonPress()
		{
			_targetPointsCounter = 0;
			UpdateRunMovementButtonState();
			_eventBus.Publish(_startMovementEvent);
			_views.MovingLabel.gameObject.SetActive(true);
		}
		
		private void Dispose(GameDisposeEvent _)
		{
			_views.StartMovementBtn.onClick.RemoveAllListeners();
		}
	}
}