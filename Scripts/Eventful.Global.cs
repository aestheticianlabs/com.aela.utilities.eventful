using System;
using System.Collections.Generic;

namespace AeLa.Utilities.Eventful
{
	public static partial class Eventful
	{
		private static readonly Dictionary<string, List<Delegate>> listeners = new();
		private static readonly Dictionary<string, List<Delegate>> listenersWithParams = new();
		
		/// <summary>
		/// Sends an event without any parameters
		/// </summary>
		/// <param name="e">The name of the event to send</param>
		public static void Send(string e)
		{
			if (!listeners.ContainsKey(e)) return;

			foreach (var del in listeners[e])
			{
				del.DynamicInvoke();
			}
		}

		/// <summary>
		/// Sends an event with parameters
		/// </summary>
		/// <param name="e">The name of the event to send</param>
		/// <param name="args">The parameters to send with the event</param>
		public static void Send(string e, params object[] args)
		{
			if (!listenersWithParams.ContainsKey(e)) return;

			foreach (var del in listenersWithParams[e])
			{
				del.DynamicInvoke(args);
			}

			Send(e);
		}
		
		/// <summary>
		/// Adds a listener for event <paramref name="e"/>
		/// </summary>
		/// <param name="e">The event to listen for</param>
		/// <param name="listener">The function to be called when <paramref name="e"/> is sent</param>
		public static void AddListener(string e, Action listener) =>
			AddListenerInternal(e, listener, listeners);

		/// <inheritdoc cref="AddListener(string, Action)"/>
		public static void AddListener<T>(string e, Action<T> listener) =>
			AddListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="AddListener(string, Action)"/>
		public static void AddListener<T1, T2>(string e, Action<T1, T2> listener) =>
			AddListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="AddListener(string, Action)"/>
		public static void AddListener<T1, T2, T3>(string e, Action<T1, T2, T3> listener) =>
			AddListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="AddListener(string, Action)"/>
		public static void AddListener<T1, T2, T3, T4>(string e, Action<T1, T2, T3, T4> listener) =>
			AddListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="AddListener(string, Action)"/>
		public static void AddListener<T1, T2, T3, T4, T5>(string e, Action<T1, T2, T3, T4, T5> listener) =>
			AddListenerInternal(e, listener, listenersWithParams);

		/// <summary>
		/// Removes the <paramref name="listener"/> for event <paramref name="e"/>
		/// </summary>
		/// <param name="e">The event to stop listening for</param>
		/// <param name="listener">The listener to remove</param>
		public static void RemoveListener(string e, Action listener) =>
			RemoveListenerInternal(e, listener, listeners);

		/// <inheritdoc cref="RemoveListener(string, Action)"/>
		public static void RemoveListener<T>(string e, Action<T> listener) =>
			RemoveListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="RemoveListener(string, Action)"/>
		public static void RemoveListener<T1, T2>(string e, Action<T1, T2> listener) =>
			RemoveListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="RemoveListener(string, Action)"/>
		public static void RemoveListener<T1, T2, T3>(string e, Action<T1, T2, T3> listener) =>
			RemoveListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="RemoveListener(string, Action)"/>
		public static void RemoveListener<T1, T2, T3, T4>(string e, Action<T1, T2, T3, T4> listener) =>
			RemoveListenerInternal(e, listener, listenersWithParams);

		/// <inheritdoc cref="RemoveListener(string, Action)"/>
		public static void RemoveListener<T1, T2, T3, T4, T5>(string e, Action<T1, T2, T3, T4, T5> listener) =>
			RemoveListenerInternal(e, listener, listenersWithParams);


		/// <summary>
		/// Removes all listeners subscribed to event <paramref name="e"/>
		/// </summary>
		/// <param name="e">The target event</param>
		public static void RemoveAllListeners(string e)
		{
			listeners.Remove(e);
			listenersWithParams.Remove(e);
		}
	}
}