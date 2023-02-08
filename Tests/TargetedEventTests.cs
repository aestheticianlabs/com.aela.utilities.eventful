using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UnityEngine;

namespace AeLa.Utilities.Eventful.Tests
{
	public class TargetedEventTests
	{
		[Test]
		public void ListenSimple()
		{
			const string eventName = "event";

			var target = new GameObject();
			var badTarget = new GameObject();
			var called = false;
			Eventful.AddListener(target, eventName, () => called = true);

			Eventful.Send(target, eventName);

			Assert.True(called, "Listener not called");
			called = false;
			
			Eventful.Send(eventName);
			Assert.False(called, "Targeted listener called on global event");
			called = false;
			
			Eventful.Send(badTarget, eventName);
			Assert.False(called, "Targeted listener called from wrong target");

			Eventful.RemoveAllListeners();
			Object.Destroy(target);
			Object.Destroy(badTarget);
		}

		/// <summary>
		/// We expect listeners for the same event, with or without parameters, to be called when the event is sent
		/// </summary>
		[Test]
		public void ListenSimpleSendParameter()
		{
			const string eventName = "event";
			const int value = 5;
			var target = new GameObject();
			var badTarget = new GameObject();

			var delegateCalled = false;
			var d2Val = -1;

			Eventful.AddListener(target, eventName, () => delegateCalled = true);
			Eventful.AddListener<int>(target, eventName, v => d2Val = v);
			// todo: more than 1 parameter test

			Eventful.Send(target, eventName, value);

			Assert.True(delegateCalled, "Listener not called");
			Assert.AreEqual(d2Val, value, "Incorrect parameter value passed to listener");
			delegateCalled = false;
			
			Eventful.Send(eventName, value);
			
			Assert.False(delegateCalled, "Targeted listener called on global event");
			delegateCalled = false;
			
			Eventful.Send(badTarget, eventName, value);
			Assert.False(delegateCalled, "Targeted listener called from wrong target");

			Eventful.RemoveAllListeners();
			Object.Destroy(target);
			Object.Destroy(badTarget);
		}

		[Test]
		public void ListenWithParameters()
		{
			const string e = "event";
			var target = new GameObject();

			const int v1 = 1;
			const bool v2 = true;
			const float v3 = .1f;
			const string v4 = "test";
			const char v5 = 'a';

			var called = false;
			Eventful.AddListener<int>(
				target, e, p1 =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
				}
			);
			Eventful.Send(target, e, v1);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool>(
				target, e, (p1, p2) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
				}
			);
			Eventful.Send(target, e, v1, v2);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float>(
				target, e, (p1, p2, p3) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
				}
			);
			Eventful.Send(target, e, v1, v2, v3);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float, string>(
				target, e, (p1, p2, p3, p4) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
					Assert.AreEqual(v4, p4);
				}
			);
			Eventful.Send(target, e, v1, v2, v3, v4);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float, string, char>(
				target, e, (p1, p2, p3, p4, p5) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
					Assert.AreEqual(v4, p4);
					Assert.AreEqual(v5, p5);
				}
			);
			Eventful.Send(target, e, v1, v2, v3, v4, v5);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			
			Object.Destroy(target);
		}
		
		[Test]
		public void SendEventWithoutListeners()
		{
			const string e = "event";
			var target = new GameObject();
			
			// these should all fail silently
			Eventful.Send(target, e);
			Eventful.Send(target, e, 0);
			Eventful.Send(target, e, 0, 0);
			Eventful.Send(target, e, 0, 0, 0);
			Eventful.Send(target, e, 0, 0, 0, 0);
			Eventful.Send(target, e, 0, 0, 0, 0, 0);
			
			Eventful.AddListener(target, "notOurEvent", () => {});
			
			// these should all fail silently
			Eventful.Send(target, e);
			Eventful.Send(target, e, 0);
			Eventful.Send(target, e, 0, 0);
			Eventful.Send(target, e, 0, 0, 0);
			Eventful.Send(target, e, 0, 0, 0, 0);
			Eventful.Send(target, e, 0, 0, 0, 0, 0);
			
			Eventful.RemoveAllListeners();
			Object.Destroy(target);
		}

		[Test]
		public void RemoveListener()
		{
			const string eventName = "event";
			var target = new GameObject();

			var callCount = 0;

			void Listener() => callCount++;
			Eventful.AddListener(target, eventName, Listener);
			Eventful.Send(target, eventName);
			Eventful.RemoveListener(target, eventName, Listener);
			Eventful.Send(target, eventName);

			Assert.Less(callCount, 2);

			Eventful.RemoveAllListeners();
			Object.Destroy(target);
		}

		[Test]
		public void RemoveAllListenersForEvent()
		{
			const string eventName = "event";
			var target = new GameObject();

			var callCount = 0;

			void Listener() => callCount++;
			void Listener2() => callCount++;
			void Listener3(int p) => callCount++;
			void Listener4(int p) => callCount++;
			
			Eventful.AddListener(target, eventName, Listener);
			Eventful.AddListener(target, eventName, Listener2);
			Eventful.AddListener<int>(target, eventName, Listener3);
			Eventful.AddListener<int>(target, eventName, Listener4);
			Eventful.Send(target, eventName, 0);
			Eventful.RemoveAllListeners(target, eventName);
			Eventful.Send(target, eventName, 0);
			Eventful.RemoveAllListeners();

			switch (callCount)
			{
				case < 4:
					Assert.Inconclusive("Not all listeners called");
					break;
				case > 4:
					Assert.Fail("Listeners called more than once (possibly not removed)");
					break;
			}
			
			Object.Destroy(target);
		}
		
		[Test]
		public void RemoveAllListenersForTarget()
		{
			const string eventName = "event";
			var target = new GameObject();

			var callCount = 0;

			void Listener() => callCount++;
			void Listener2() => callCount++;
			void Listener3(int p) => callCount++;
			void Listener4(int p) => callCount++;
			
			Eventful.AddListener(target, eventName, Listener);
			Eventful.AddListener(target, eventName, Listener2);
			Eventful.AddListener<int>(target, eventName, Listener3);
			Eventful.AddListener<int>(target, eventName, Listener4);
			Eventful.Send(target, eventName, 0);
			Eventful.RemoveAllListeners(target);
			Eventful.Send(target, eventName, 0);
			Eventful.RemoveAllListeners();

			switch (callCount)
			{
				case < 4:
					Assert.Inconclusive("Not all listeners called");
					break;
				case > 4:
					Assert.Fail("Listeners called more than once (possibly not removed)");
					break;
			}
			
			Object.Destroy(target);
		}

		[Test]
		[SuppressMessage("ReSharper", "AccessToModifiedClosure")]
		public void RemoveListenerParameter()
		{
			const string eventName = "event";
			var target = new GameObject();

			var callCount = 0;

			void Listener(int o) => callCount++;

			Eventful.AddListener<int>(target, eventName, Listener);
			Eventful.Send(target, eventName, 0);
			Eventful.RemoveListener<int>(target, eventName, Listener);
			Eventful.Send(target, eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;

			void Listener2(int p1, int p2) => callCount++;
			Eventful.AddListener<int, int>(target, eventName, Listener2);
			Eventful.Send(target, eventName, 0, 0);
			Eventful.RemoveListener<int, int>(target, eventName, Listener2);
			Eventful.Send(target, eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener3(int p1, int p2, int p3) => callCount++;
			Eventful.AddListener<int, int, int>(target, eventName, Listener3);
			Eventful.Send(target, eventName, 0, 0, 0);
			Eventful.RemoveListener<int, int, int >(target, eventName, Listener3);
			Eventful.Send(target, eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener4(int p1, int p2, int p3, int p4) => callCount++;
			Eventful.AddListener<int, int, int, int>(target, eventName, Listener4);
			Eventful.Send(target, eventName, 0, 0, 0, 0);
			Eventful.RemoveListener<int, int, int, int>(target, eventName, Listener4);
			Eventful.Send(target, eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener5(int p1, int p2, int p3, int p4, int p5) => callCount++;
			Eventful.AddListener<int, int, int, int, int>(target, eventName, Listener5);
			Eventful.Send(target, eventName, 0, 0, 0, 0, 0);
			Eventful.RemoveListener<int, int, int, int, int>(target, eventName, Listener5);
			Eventful.Send(target, eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;

			Object.Destroy(target);
		}

		[Test]
		public void ModifyCallbacksDuringInvokeException()
		{
			// add a listener to the event that removes itself during invocation
			// removal should be queued and executed after event invocation
			const string eventName = "event";
			var target = new GameObject();
			var calls = 0;
			var queuedListenerCalls = 0;

			void QueuedListener()
			{
				queuedListenerCalls++;
				Eventful.RemoveAllListeners(target);
			}
			
			void Listener()
			{
				calls++;
				Eventful.RemoveListener(target, eventName, Listener);
				Eventful.AddListener(target, eventName, QueuedListener);
			}
			
			Eventful.AddListener(target, eventName, Listener);
			Eventful.Send(target, eventName);
			Eventful.Send(target, eventName);
			Eventful.Send(target, eventName);
			
			Assert.AreEqual(calls, 1);
			Assert.AreEqual(queuedListenerCalls, 1);
			
			Eventful.RemoveAllListeners();
		}
	}
}