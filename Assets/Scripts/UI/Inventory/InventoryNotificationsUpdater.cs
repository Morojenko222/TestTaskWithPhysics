using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace UI.Inventory
{
	public class InventoryNotificationsUpdater
	{
		private InventoryWindowViews _windowViews;
		private List<InventoryElementView> _elements;
		private HashSet<InventoryElementView> _requiredToRemoveNotificationsSet;
		private HashSet<InventoryElementView> _notificationsRemovedSet;

		public InventoryNotificationsUpdater(InventoryWindowViews windowViews, List<InventoryElementView> elements)
		{
			_windowViews = windowViews;
			_elements = elements;
			_windowViews.ScrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);

			Initialize();
		}

		public void Dispose()
		{
			_windowViews.ScrollRect.onValueChanged.RemoveAllListeners();
		}
		
		public void RefreshNotifications()
		{
			_requiredToRemoveNotificationsSet.Clear();
			_notificationsRemovedSet.Clear();

			foreach (InventoryElementView view in _elements)
			{
				view.NotificationMarker.gameObject.SetActive(true);
			}
		}

		public void OnWindowOpen()
		{
			ApplyRequiredToRemoveRequests();
			LayoutRebuilder.ForceRebuildLayoutImmediate(_windowViews.ElementContainer);
			UpdateElementsViewedStatus();
		}

		private void Initialize()
		{
			_requiredToRemoveNotificationsSet = new HashSet<InventoryElementView>();
			_notificationsRemovedSet = new HashSet<InventoryElementView>();
		}

		private void OnScrollRectValueChanged(Vector2 pos)
		{
			UpdateElementsViewedStatus();
		}

		private void UpdateElementsViewedStatus()
		{
			float viewportYSize = _windowViews.Viewport.rect.size.y;
			float contentYPos = _windowViews.ScrollRect.content.anchoredPosition.y;
			float space = _windowViews.VerticalLayout.spacing;
			
			foreach (var view in _elements)
			{
				if (_notificationsRemovedSet.Contains(view))
					continue;

				float elementYSize = view.Root.sizeDelta.y;
				float elementYPos = view.Root.localPosition.y;
				float step = elementYSize + space;
					
				float upperEdge = elementYPos * (-1) + step;
				float lowerEdge = (viewportYSize + elementYPos) * (-1);
				
				if (contentYPos > lowerEdge && contentYPos < upperEdge)
				{
					_requiredToRemoveNotificationsSet.Add(view);
				}
				else
				{
					if (_requiredToRemoveNotificationsSet.Contains(view))
					{
						view.NotificationMarker.gameObject.SetActive(false);
						_requiredToRemoveNotificationsSet.Remove(view);
						_notificationsRemovedSet.Add(view);
					}
				}
			}
		}
		
		private void ApplyRequiredToRemoveRequests()
		{
			foreach (InventoryElementView view in _requiredToRemoveNotificationsSet)
			{
				view.NotificationMarker.gameObject.SetActive(false);
				_notificationsRemovedSet.Add(view);
			}
			
			_requiredToRemoveNotificationsSet.Clear();
		}
	}
}