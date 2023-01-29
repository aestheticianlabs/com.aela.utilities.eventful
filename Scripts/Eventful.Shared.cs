using System;
using System.Collections.Generic;
using UnityEngine;
using ListenerDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<System.Delegate>>;

namespace AeLa.Utilities.Eventful
{
	public static partial class Eventful
	{
		private static void AddListenerInternal(string e, Delegate listener, ListenerDict dict)
		{
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
			listeners.Clear();
			listenersWithParams.Clear();
			targetedListeners.Clear();
			targetedListenersWithParams.Clear();
		}
	}
}