using System;
using UnityEngine;
using UnityEngine.Events;

namespace AeLa.Utilities.Eventful.Components
{
	/// <summary>
	/// Invokes a UnityEvent when an Eventful event is sent
	/// </summary>
	public class EventfulListener : MonoBehaviour
	{
		/// <summary>
		/// The target object for the event (if any)
		/// </summary>
		[Tooltip("The target object for the event (if any)")]
		[SerializeField] private GameObject target;
	
		/// <summary>
		/// The event this component is listening for
		/// </summary>
		[Tooltip("The event this component is listening for")]
		[SerializeField] private string @event;

		/// <summary>
		/// Called when the event is sent
		/// </summary>
		[Tooltip("Called when the event is sent")]
		public UnityEvent OnEvent = new();

		private bool listenerRegistered;

		/// <inheritdoc cref="target"/>
		public GameObject Target => target;

		/// <inheritdoc cref="@event"/>
		public string Event => @event;

		/// <summary>
		/// Sets or changes the <see cref="@event"/> and <see cref="target"/> this component listens for
		/// </summary>
		public void ListenFor(GameObject target, string @event)
		{
			this.target = target;
			this.@event = @event;
			if (listenerRegistered) RemoveListener();
			AddListener();
		}


		/// <inheritdoc cref="ListenFor(UnityEngine.GameObject,string)"/>
		public void ListenFor(string @event) => ListenFor(null, @event);
		
		private void OnEnable()
		{
			AddListener();
		}

		private void OnDisable()
		{
			RemoveListener();
		}

		private void AddListener()
		{
			if (string.IsNullOrEmpty(@event)) return;
			Eventful.AddListener(target, @event, OnEvent.Invoke);
			listenerRegistered = true;
		}

		private void RemoveListener()
		{
			if (!listenerRegistered) return;
			Eventful.RemoveListener(target, @event, OnEvent.Invoke);
			listenerRegistered = false;
		}
	}
}