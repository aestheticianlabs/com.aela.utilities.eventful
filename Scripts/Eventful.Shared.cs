using System;
using System.Collections.Generic;
using UnityEngine;
using ListenerDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<System.Delegate>>;

namespace AeLa.Utilities.Eventful
{
	public static partial class Eventful
	{
		private static readonly Queue<Action> queuedActions = new();
		private static int invocationDepth = 0;
		
		private static void AddListenerInternal(string e, Delegate listener, ListenerDict dict)
		{
			// queue add action to be executed after event invocation
			if (invocationDepth > 0)
			{
				queuedActions.Enqueue(() => AddListenerInternal(e, listener, dict));
				return;
			}

			if (!dict.ContainsKey(e))
			{
				dict.Add(e, new List<Delegate>());
			}

			dict[e].Add(listener);
		}

		private static void RemoveListenerInternal(
			string e, Delegate listener, Dictionary<string, List<Delegate>> target
		)
		{
			// queue add action to be executed after event invocation
			if (invocationDepth > 0)
			{
				queuedActions.Enqueue(() => RemoveListenerInternal(e, listener, target));
				return;
			}
			
			if (!target.TryGetValue(e, out var list) || !list.Remove(listener))
			{
				Logging.Warn($"Could not remove listener for event {e} because it is not registered.");
				return;
			}

			if (list.Count == 0)
			{
				target.Remove(e);
			}
		}

		/// <summary>
		/// Removes all listeners subscribed to all events
		/// </summary>
		public static void RemoveAllListeners()
		{
			// queue add action to be executed after event invocation
			if (invocationDepth > 0)
			{
				queuedActions.Enqueue(RemoveAllListeners);
				return;
			}
			
			listeners.Clear();
			listenersWithParams.Clear();
			targetedListeners.Clear();
			targetedListenersWithParams.Clear();
		}

		/// <summary>
		/// Executes all actions queued during event invocation.
		/// </summary>
		private static void ExecuteActionQueue()
		{
			while (queuedActions.Count > 0)
			{
				queuedActions.Dequeue().Invoke();
			}
		}
	}
}