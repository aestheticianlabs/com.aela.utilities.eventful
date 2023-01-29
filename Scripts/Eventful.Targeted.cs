using System;
using System.Collections.Generic;
using UnityEngine;
using ListenerDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<System.Delegate>>;

namespace AeLa.Utilities.Eventful
{
	public static partial class Eventful
	{
		private static readonly Dictionary<GameObject, ListenerDict> targetedListeners = new();
		private static readonly Dictionary<GameObject, ListenerDict> targetedListenersWithParams = new();

		/// <summary>
		/// Sends an event without any parameters
		/// </summary>
		/// <param name="target">The target for this event</param>
		/// <param name="e">The name of the event to send</param>
		public static void Send(GameObject target, string e)
		{
			if (!targetedListeners.TryGetValue(target, out var listeners)) return;

			foreach (var del in listeners[e])
			{
				del.DynamicInvoke();
			}
		}

		/// <summary>
		/// Sends an event with parameters
		/// </summary>
		/// <param name="target">The target for this event</param>
		/// <param name="e">The name of the event to send</param>
		/// <param name="args">The parameters to send with the event</param>
		public static void Send(GameObject target, string e, params object[] args)
		{
			if (!targetedListenersWithParams.TryGetValue(target, out var listeners)) return;

			foreach (var del in listeners[e])
			{
				del.DynamicInvoke(args);
			}

			Send(target, e);
		}

		private static void AddListenerInternal(
			GameObject target, string e, Delegate listener, Dictionary<GameObject, ListenerDict> targetsDict
		)
		{
			if (!targetsDict.ContainsKey(target))
			{
				targetsDict.Add(target, new ListenerDict());
			}
			
			AddListenerInternal(e, listener, targetsDict[target]);
		}

		/// <summary>
		/// Adds a listener for event <paramref name="e"/>
		/// </summary>
		/// <param name="target">The target for this event</param>
		/// <param name="e">The event to listen for</param>
		/// <param name="listener">The function to be called when <paramref name="e"/> is sent</param>
		public static void AddListener(GameObject target, string e, Action listener) =>
			AddListenerInternal(target, e, listener, targetedListeners);

		/// <inheritdoc cref="AddListener(GameObject, string, Action)"/>
		public static void AddListener<T>(GameObject target, string e, Action<T> listener) =>
			AddListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="AddListener(GameObject, string, Action)"/>
		public static void AddListener<T1, T2>(GameObject target, string e, Action<T1, T2> listener) =>
			AddListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="AddListener(GameObject, string, Action)"/>
		public static void AddListener<T1, T2, T3>(GameObject target, string e, Action<T1, T2, T3> listener) =>
			AddListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="AddListener(GameObject, string, Action)"/>
		public static void AddListener<T1, T2, T3, T4>(GameObject target, string e, Action<T1, T2, T3, T4> listener) =>
			AddListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="AddListener(GameObject, string, Action)"/>
		public static void AddListener<T1, T2, T3, T4, T5>(GameObject target, string e, Action<T1, T2, T3, T4, T5> listener) =>
			AddListenerInternal(target, e, listener, targetedListenersWithParams);

		private static void RemoveListenerInternal(
			GameObject target, string e, Delegate listener, Dictionary<GameObject, ListenerDict> targetedListeners
		)
		{
			if (!targetedListeners.TryGetValue(target, out var listeners)) return;
			
			RemoveListenerInternal(e, listener, listeners);
			
			// release target reference
			if (listeners.Count == 0)
			{
				targetedListeners.Remove(target);
			}
		}

		/// <summary>
		/// Removes the <paramref name="listener"/> for event <paramref name="e"/>
		/// </summary>
		/// <param name="target">The target for this event</param>
		/// <param name="e">The event to stop listening for</param>
		/// <param name="listener">The listener to remove</param>
		public static void RemoveListener(GameObject target, string e, Action listener) =>
			RemoveListenerInternal(target, e, listener, targetedListeners);

		/// <inheritdoc cref="RemoveListener(GameObject, string, Action)"/>
		public static void RemoveListener<T>(GameObject target, string e, Action<T> listener) =>
			RemoveListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="RemoveListener(GameObject, string, Action)"/>
		public static void RemoveListener<T1, T2>(GameObject target, string e, Action<T1, T2> listener) =>
			RemoveListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="RemoveListener(GameObject, string, Action)"/>
		public static void RemoveListener<T1, T2, T3>(GameObject target, string e, Action<T1, T2, T3> listener) =>
			RemoveListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="RemoveListener(GameObject, string, Action)"/>
		public static void RemoveListener<T1, T2, T3, T4>(GameObject target, string e, Action<T1, T2, T3, T4> listener) =>
			RemoveListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <inheritdoc cref="RemoveListener(GameObject, string, Action)"/>
		public static void RemoveListener<T1, T2, T3, T4, T5>(GameObject target, string e, Action<T1, T2, T3, T4, T5> listener) =>
			RemoveListenerInternal(target, e, listener, targetedListenersWithParams);

		/// <summary>
		/// Removes all listeners for the provided target
		/// </summary>
		/// <param name="target">The target to remove listeners for</param>
		public static void RemoveAllListeners(GameObject target)
		{
			targetedListeners.Remove(target);
			targetedListenersWithParams.Remove(target);
		}

		/// <summary>
		/// Removes all listeners for the provided event on the target
		/// </summary>
		/// <param name="target">The target object</param>
		/// <param name="e">The event name</param>
		public static void RemoveAllListeners(GameObject target, string e)
		{
			if (targetedListeners.TryGetValue(target, out var listeners))
			{
				listeners.Remove(e);
				if (listeners.Count == 0) targetedListeners.Remove(target);
			}

			if (targetedListenersWithParams.TryGetValue(target, out listeners))
			{
				listeners.Remove(e);
				if (listeners.Count == 0) targetedListenersWithParams.Remove(target);
			}
		}
	}
}