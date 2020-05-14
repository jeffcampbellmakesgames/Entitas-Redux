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

using JCMG.EntitasRedux.VisualDebugging;
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeSystemInfo
	{
		private SystemInfo _info;

		[SetUp]
		public void Setup()
		{
			_info = new SystemInfo(new TestUpdateSystem());
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForInitializeSystem()
		{
			var system = new TestInitializeSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual(system, info.System);
			Assert.AreEqual("TestInitialize", info.SystemName);

			Assert.IsTrue(info.IsInitializeSystems);
			Assert.IsFalse(info.IsUpdateSystems);

			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsFalse(info.IsReactiveSystems);

			Assert.AreEqual(0, info.AccumulatedUpdateDuration);
			Assert.AreEqual(0, info.MinUpdateDuration);
			Assert.AreEqual(0, info.MaxUpdateDuration);
			Assert.AreEqual(0, info.AverageUpdateDuration);

			Assert.IsTrue(info.isActive);
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForExecuteSystem()
		{
			var system = new TestUpdateSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual("TestUpdate", info.SystemName);
			Assert.IsFalse(info.IsInitializeSystems);
			Assert.IsTrue(info.IsUpdateSystems);
			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsFalse(info.IsReactiveSystems);
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForCleanupSystem()
		{
			var system = new TestCleanupSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual("TestCleanup", info.SystemName);
			Assert.IsFalse(info.IsInitializeSystems);
			Assert.IsFalse(info.IsUpdateSystems);
			Assert.IsTrue(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsFalse(info.IsReactiveSystems);
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForTearDownSystem()
		{
			var system = new TestTearDownSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual("TestTearDown", info.SystemName);

			Assert.IsFalse(info.IsInitializeSystems);
			Assert.IsFalse(info.IsUpdateSystems);
			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsTrue(info.IsTearDownSystems);
			Assert.IsFalse(info.IsReactiveSystems);
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForReactiveSystem()
		{
			var system = new TestReactiveSystem(new MyTestContext());
			var info = new SystemInfo(system);

			Assert.AreEqual("TestReactive", info.SystemName);

			Assert.IsFalse(info.IsInitializeSystems);
			Assert.IsFalse(info.IsUpdateSystems);
			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsTrue(info.IsReactiveSystems);
		}

		[NUnit.Framework.Test]
		public void AddsExecutionDuration()
		{
			_info.AddUpdateDuration(42);

			Assert.AreEqual(42, _info.AccumulatedUpdateDuration);
			Assert.AreEqual(42, _info.MinUpdateDuration);
			Assert.AreEqual(42, _info.MaxUpdateDuration);
			Assert.AreEqual(42, _info.AverageUpdateDuration);
		}

		[NUnit.Framework.Test]
		public void AddsAnotherExecutionDuration()
		{
			_info.AddUpdateDuration(20);
			_info.AddUpdateDuration(10);

			Assert.AreEqual(30, _info.AccumulatedUpdateDuration);
			Assert.AreEqual(10, _info.MinUpdateDuration);
			Assert.AreEqual(20, _info.MaxUpdateDuration);
			Assert.AreEqual(15, _info.AverageUpdateDuration);
		}

		[NUnit.Framework.Test]
		public void ResetsDurations()
		{
			_info.AddUpdateDuration(20);
			_info.AddUpdateDuration(10);

			_info.ResetFrameDurations();

			Assert.AreEqual(0, _info.AccumulatedUpdateDuration);
			Assert.AreEqual(10, _info.MinUpdateDuration);
			Assert.AreEqual(20, _info.MaxUpdateDuration);
			Assert.AreEqual(0, _info.AverageUpdateDuration);
		}

		[NUnit.Framework.Test]
		public void KeepsMinDurationAfterReset()
		{
			_info.AddUpdateDuration(20);
			_info.AddUpdateDuration(10);

			_info.ResetFrameDurations();

			_info.AddUpdateDuration(15);

			Assert.AreEqual(15, _info.AccumulatedUpdateDuration);
			Assert.AreEqual(10, _info.MinUpdateDuration);
			Assert.AreEqual(20, _info.MaxUpdateDuration);
			Assert.AreEqual(15, _info.AverageUpdateDuration);
		}
	}
}
