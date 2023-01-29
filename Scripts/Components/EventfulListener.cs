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
		/// The event this component is listening for
		/// </summary>
		[Tooltip("The event this component is listening for")]
		[SerializeField] private string @event;

		/// <summary>
		/// Called when the event is sent
		/// </summary>
		[Tooltip("Called when the event is sent")]
		public UnityEvent OnEvent;

		/// <inheritdoc cref="@event"/>
		public string Event => @event;
		
		private void OnEnable()
		{
			Eventful.AddListener(@event, OnEvent.Invoke);
		}

		private void OnDisable()
		{
			Eventful.RemoveListener(@event, OnEvent.Invoke);
		}
	}
}