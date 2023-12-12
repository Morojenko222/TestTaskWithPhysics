using System;
using System.Collections.Generic;
using Events;

public class EventBus : IDisposable {
	private Dictionary<Type, Action<IGameEvent>> _subscribers;

	public EventBus()
	{
		_subscribers = new Dictionary<Type, Action<IGameEvent>>();
	}

	public void Subscribe<T>(Action<T> subscriber) where T : IGameEvent 
	{
		Type eventType = typeof(T);
		
		if (!_subscribers.ContainsKey(eventType)) 
		{
			_subscribers[eventType] = (e) => subscriber.Invoke((T)e);
		} 
		else 
		{
			_subscribers[eventType] += (e) => subscriber((T)e);
		}
	}

	public void Unsubscribe<T>(Action<T> subscriber) where T : IGameEvent, new() 
	{
		Type eventType = typeof(T);
	
		if (_subscribers.ContainsKey(eventType)) 
		{
			_subscribers[eventType] -= (e) => subscriber((T)e);
		}
	}

	public void Publish<T>(T eventInstance) where T : IGameEvent 
	{
		Type eventType = typeof(T);
    
		if (_subscribers.TryGetValue(eventType, out var subscriber)) 
		{
			subscriber.Invoke(eventInstance);
		}
	}

	public void Dispose()
	{
		_subscribers.Clear();
	}
}