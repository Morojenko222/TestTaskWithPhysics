using System.Collections.Generic;
using Config;
using Events;
using UnityEngine;
using Views;

namespace UI.Inventory
{
	public class InventoryFlowController
	{
		private EventBus _eventBus;
		private GameParameters _gameParameters;
		private Prefabs _prefabs;
		private InventoryWindowViews _windowViews;

		private InventoryNotificationsUpdater _notificationsUpdater;
		private List<InventoryElementView> _elements;
		
		public InventoryFlowController(EventBus eventBus, GameParameters gameParameters, Prefabs prefabs, InventoryWindowViews windowViews)
		{
			_eventBus = eventBus;
			_gameParameters = gameParameters;
			_prefabs = prefabs;
			_windowViews = windowViews;
			
			Initialize();
		}

		private void Initialize()
		{
			_elements = new List<InventoryElementView>();
			_notificationsUpdater = new InventoryNotificationsUpdater(_windowViews, _elements);
			
			_windowViews.OpenWindowBtn.onClick.AddListener(OpenInventory);
			_windowViews.CloseWindowBtn.onClick.AddListener(CloseInventory);
			_windowViews.RefreshNotifications.onClick.AddListener(RefreshNotifications);
			_eventBus.Subscribe<GameDisposeEvent> (Dispose);
			
			InstantiateTestElements();
		}

		// Can be polled, currently objects initialize as new objects for simplicity
		private void InstantiateTestElements()
		{
			for (int i = 0; i < _gameParameters.InventoryElementsAmount; i++)
			{
				InventoryElementView view = Object.Instantiate(_prefabs.InventoryElementView, _windowViews.ElementContainer);
				_elements.Add(view);
			}
		}

		private void OpenInventory()
		{
			_windowViews.Root.gameObject.SetActive(true);
			_notificationsUpdater.OnWindowOpen();
		}

		private void CloseInventory()
		{
			_windowViews.Root.gameObject.SetActive(false);
		}

		private void RefreshNotifications()
		{
			_notificationsUpdater.RefreshNotifications();
		}
		
		private void Dispose(GameDisposeEvent _)
		{
			_windowViews.OpenWindowBtn.onClick.RemoveAllListeners();
			_windowViews.CloseWindowBtn.onClick.RemoveAllListeners();
			_windowViews.RefreshNotifications.onClick.RemoveAllListeners();
			_notificationsUpdater.Dispose();
		}
	}
}