using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
	public class Pool<T> : IDisposable
	{
		private Stack<T> _stack;

		private Func<T> _ctor;
		private Action<T> _onRelease;
		private Action<T> _onGet;

		public Pool(Func<T> ctor, Action<T> onRelease = null, Action<T> onGet = null)
		{
			_stack = new Stack<T>();
		
			_ctor = ctor;
			_onRelease = onRelease;
			_onGet = onGet;
		}

		public T Get()
		{
			T resource = _stack.Count > 0 
				? _stack.Pop() 
				: _ctor.Invoke();
		
			_onGet?.Invoke(resource);

			return resource;
		}

		public void Release(T releasedObject)
		{
			_stack.Push(releasedObject);
			_onRelease?.Invoke(releasedObject);
		}

		public void Dispose()
		{
			_stack.Clear();
			_ctor = null;
			_onRelease = null;
			_onGet = null;
		
			Debug.Log("Dispose 2");
		}
	}
}