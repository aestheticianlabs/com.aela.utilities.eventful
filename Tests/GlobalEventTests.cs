using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace AeLa.Utilities.Eventful.Tests
{
	public class GlobalEventTests
	{
		[Test]
		public void ListenSimple()
		{
			const string eventName = "event";

			var called = false;
			Eventful.AddListener(eventName, () => called = true);

			Eventful.Send(eventName);

			Assert.True(called, "Listener not called");

			Eventful.RemoveAllListeners();
		}

		/// <summary>
		/// We expect listeners for the same event, with or without parameters, to be called when the event is sent
		/// </summary>
		[Test]
		public void ListenSimpleSendParameter()
		{
			const string eventName = "event";
			const int value = 5;

			var delegateCalled = false;
			var d2Val = -1;

			Eventful.AddListener(eventName, () => delegateCalled = true);
			Eventful.AddListener<int>(eventName, v => d2Val = v);
			// todo: more than 1 parameter test

			Eventful.Send(eventName, value);

			Assert.True(delegateCalled);
			Assert.AreEqual(d2Val, value);

			Eventful.RemoveAllListeners();
		}

		[Test]
		public void ListenWithParameters()
		{
			const string e = "event";

			const int v1 = 1;
			const bool v2 = true;
			const float v3 = .1f;
			const string v4 = "test";
			const char v5 = 'a';

			var called = false;
			Eventful.AddListener<int>(
				e, p1 =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
				}
			);
			Eventful.Send(e, v1);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool>(
				e, (p1, p2) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
				}
			);
			Eventful.Send(e, v1, v2);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float>(
				e, (p1, p2, p3) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
				}
			);
			Eventful.Send(e, v1, v2, v3);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float, string>(
				e, (p1, p2, p3, p4) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
					Assert.AreEqual(v4, p4);
				}
			);
			Eventful.Send(e, v1, v2, v3, v4);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;

			Eventful.AddListener<int, bool, float, string, char>(
				e, (p1, p2, p3, p4, p5) =>
				{
					called = true;
					Assert.AreEqual(v1, p1);
					Assert.AreEqual(v2, p2);
					Assert.AreEqual(v3, p3);
					Assert.AreEqual(v4, p4);
					Assert.AreEqual(v5, p5);
				}
			);
			Eventful.Send(e, v1, v2, v3, v4, v5);
			Eventful.RemoveAllListeners();
			Assert.True(called, "Listener not called");
			called = false;
		}
		
		[Test]
		public void SendEventWithoutListeners()
		{
			const string e = "event";
			
			// these should all fail silently
			Eventful.Send(e);
			Eventful.Send(e, 0);
			Eventful.Send(e, 0, 0);
			Eventful.Send(e, 0, 0, 0);
			Eventful.Send(e, 0, 0, 0, 0);
			Eventful.Send(e, 0, 0, 0, 0, 0);
		}

		[Test]
		public void RemoveListener()
		{
			const string eventName = "event";

			var callCount = 0;

			void Listener() => callCount++;
			Eventful.AddListener(eventName, Listener);
			Eventful.Send(eventName);
			Eventful.RemoveListener(eventName, Listener);
			Eventful.Send(eventName);

			Assert.Less(callCount, 2);

			Eventful.RemoveAllListeners();
		}

		[Test]
		public void RemoveAllListenersForEvent()
		{
			const string eventName = "event";

			var callCount = 0;

			void Listener() => callCount++;
			void Listener2() => callCount++;
			void Listener3(int p) => callCount++;
			void Listener4(int p) => callCount++;
			
			Eventful.AddListener(eventName, Listener);
			Eventful.AddListener(eventName, Listener2);
			Eventful.AddListener<int>(eventName, Listener3);
			Eventful.AddListener<int>(eventName, Listener4);
			Eventful.Send(eventName, 0);
			Eventful.RemoveAllListeners(eventName);
			Eventful.Send(eventName, 0);
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
			
		}

		[Test]
		[SuppressMessage("ReSharper", "AccessToModifiedClosure")]
		public void RemoveListenerParameter()
		{
			const string eventName = "event";

			var callCount = 0;

			void Listener(int o) => callCount++;

			Eventful.AddListener<int>(eventName, Listener);
			Eventful.Send(eventName, 0);
			Eventful.RemoveListener<int>(eventName, Listener);
			Eventful.Send(eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;

			void Listener2(int p1, int p2) => callCount++;
			Eventful.AddListener<int, int>(eventName, Listener2);
			Eventful.Send(eventName, 0, 0);
			Eventful.RemoveListener<int, int>(eventName, Listener2);
			Eventful.Send(eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener3(int p1, int p2, int p3) => callCount++;
			Eventful.AddListener<int, int, int>(eventName, Listener3);
			Eventful.Send(eventName, 0, 0, 0);
			Eventful.RemoveListener<int, int, int >(eventName, Listener3);
			Eventful.Send(eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener4(int p1, int p2, int p3, int p4) => callCount++;
			Eventful.AddListener<int, int, int, int>(eventName, Listener4);
			Eventful.Send(eventName, 0, 0, 0, 0);
			Eventful.RemoveListener<int, int, int, int>(eventName, Listener4);
			Eventful.Send(eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
			
			void Listener5(int p1, int p2, int p3, int p4, int p5) => callCount++;
			Eventful.AddListener<int, int, int, int, int>(eventName, Listener5);
			Eventful.Send(eventName, 0, 0, 0, 0, 0);
			Eventful.RemoveListener<int, int, int, int, int>(eventName, Listener5);
			Eventful.Send(eventName);
			Eventful.RemoveAllListeners();

			Assert.AreEqual(1, callCount);
			callCount = 0;
		}

		[Test]
		public void ModifyCallbacksDuringInvokeException()
		{
			// add a listener to the event that removes itself during invocation
			// removal should be queued and executed after event invocation
			const string eventName = "event";
			var calls = 0;
			var queuedListenerCalls = 0;

			void QueuedListener()
			{
				queuedListenerCalls++;
				Eventful.RemoveAllListeners(eventName);
			}
			
			void Listener()
			{
				calls++;
				Eventful.RemoveListener(eventName, Listener);
				Eventful.AddListener(eventName, QueuedListener);
			}
			
			Eventful.AddListener(eventName, Listener);
			Eventful.Send(eventName);
			Eventful.Send(eventName);
			Eventful.Send(eventName);
			
			Assert.AreEqual(calls, 1);
			Assert.AreEqual(queuedListenerCalls, 1);
			
			Eventful.RemoveAllListeners();
		}
	}
}