using AeLa.Utilities.Eventful.Components;
using NUnit.Framework;
using UnityEngine;

namespace AeLa.Utilities.Eventful.Tests
{
	public class ComponentTests
	{
		[Test]
		public void ListenWithoutTarget()
		{
			const string e = "test";
			const string badE = "testBad";
			var called = false;

			var listener = new GameObject().AddComponent<EventfulListener>();
			listener.ListenFor(e);

			listener.OnEvent.AddListener(() => called = true);
			
			Eventful.Send(e);
			Assert.True(called, "Listener not called");

			called = false;
			Eventful.Send(badE);
			Assert.False(called, "Listener called on wrong event");

			called = false;
			listener.enabled = false;
			Eventful.Send(e);
			Assert.False(called, "Listener called while component disabled");
			
			called = false;
			Object.DestroyImmediate(listener);
			Eventful.Send(e);
			Assert.False(called, "Listener called after component was destroyed");
			
			Eventful.RemoveAllListeners();
		}
		
		[Test]
		public void ListenWithTarget()
		{
			const string e = "test";
			const string badE = "testBad";
			var target = new GameObject();
			var badTarget = new GameObject();
			var called = false;

			var listener = new GameObject().AddComponent<EventfulListener>();
			listener.ListenFor(target, e);

			listener.OnEvent.AddListener(() => called = true);
			
			Eventful.Send(target, e);
			Assert.True(called, "Listener not called");
			
			called = false;
			Eventful.Send(badTarget, e);
			Assert.False(called, "Listener called on wrong target");

			called = false;
			Eventful.Send(target, badE);
			Assert.False(called, "Listener called on wrong event");

			called = false;
			listener.enabled = false;
			Eventful.Send(target, e);
			Assert.False(called, "Listener called while component was disabled");
			listener.enabled = true;
			
			called = false;
			Object.DestroyImmediate(listener);
			Eventful.Send(target, e);
			Assert.False(called, "Listener called after component was destroyed");
			
			Object.Destroy(target);
			Object.Destroy(badTarget);
			Eventful.RemoveAllListeners();
		}
	}
}