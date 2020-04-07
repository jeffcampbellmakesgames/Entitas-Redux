/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeEvents
	{
		#region Private Fixture
		private class RemoveEventTest : IAnyStandardEventListener,
		                                IFlagEntityEventListener
		{
			public TestEntity listener => _listener;
			public string value => _value;

			readonly bool _removeComponentWhenEmpty;
			readonly TestEntity _listener;

			string _value;

			public RemoveEventTest(Contexts contexts, bool removeComponentWhenEmpty)
			{
				_removeComponentWhenEmpty = removeComponentWhenEmpty;
				_listener = contexts.Test.CreateEntity();
				_listener.AddAnyStandardEventListener(this);
				_listener.AddFlagEntityEventListener(this);
			}

			public void OnAnyStandardEvent(TestEntity entity, string value)
			{
				_listener.RemoveAnyStandardEventListener(this, _removeComponentWhenEmpty);
				_value = value;
			}

			public void OnFlagEntityEvent(TestEntity entity)
			{
				_listener.RemoveFlagEntityEventListener(this, _removeComponentWhenEmpty);
				_value = "true";
			}
		}

		#endregion

		private Contexts _contexts;
		private AnyStandardEventEventSystem _standardEventSystem;
		private FlagEntityEventEventSystem _flagEntityEventSystem;

		[SetUp]
		public void Setup()
		{
			_contexts = new Contexts();
			_standardEventSystem = new AnyStandardEventEventSystem(_contexts);
			_flagEntityEventSystem = new FlagEntityEventEventSystem(_contexts);
		}

		#region AnyStandardEventEventSystem

		[NUnit.Framework.Test]
		public void StandardEventListenerCanBeRemovedInCallback()
		{
			var eventTest = new RemoveEventTest(_contexts, false);

			_contexts.Test.CreateEntity().AddStandardEvent("Test");

			_standardEventSystem.Execute();

			Assert.AreEqual("Test", eventTest.value);
		}

		[NUnit.Framework.Test]
		public void StandardEventListenerCanBeRemovedInMiddleOfCallback()
		{
			var eventTest1 = new RemoveEventTest(_contexts, false);
			var eventTest2 = new RemoveEventTest(_contexts, false);
			var eventTest3 = new RemoveEventTest(_contexts, false);

			_contexts.Test.CreateEntity().AddStandardEvent("Test");
			_standardEventSystem.Execute();

			Assert.AreEqual("Test", eventTest1.value);
			Assert.AreEqual("Test", eventTest2.value);
			Assert.AreEqual("Test", eventTest3.value);
		}

		[NUnit.Framework.Test]
		public void StandardEventListenerCanBeRemovedInCallbackAndRemoveComponent()
		{
			var eventTest = new RemoveEventTest(_contexts, true);

			_contexts.Test.CreateEntity().AddStandardEvent("Test");
			_standardEventSystem.Execute();

			Assert.AreEqual("Test", eventTest.value);
		}

		#endregion

		#region FlagEntityEventEventSystem

		[NUnit.Framework.Test]
		public void FlagEntityEventEventSystemListenerCanBeRemovedInCallback()
		{
			var eventTest = new RemoveEventTest(_contexts, false);

			eventTest.listener.IsFlagEntityEvent = true;
			_flagEntityEventSystem.Execute();

			Assert.AreEqual("true", eventTest.value);
		}

		[NUnit.Framework.Test]
		public void FlagEntityEventEventSystemListenerCanBeRemovedInCallbackAndRemoveComponent()
		{
			var eventTest = new RemoveEventTest(_contexts, true);

			eventTest.listener.IsFlagEntityEvent = true;
			_flagEntityEventSystem.Execute();

			Assert.AreEqual("true", eventTest.value);
		}

		#endregion
	}
}
