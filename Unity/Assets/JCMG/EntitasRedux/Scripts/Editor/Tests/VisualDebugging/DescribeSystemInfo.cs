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
			_info = new SystemInfo(new TestExecuteSystem());
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForInitializeSystem()
		{
			var system = new TestInitializeSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual(system, info.System);
			Assert.AreEqual("TestInitialize", info.SystemName);

			Assert.IsTrue(info.IsInitializeSystems);
			Assert.IsFalse(info.IsExecuteSystems);

			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsFalse(info.IsReactiveSystems);

			Assert.AreEqual(0, info.AccumulatedExecutionDuration);
			Assert.AreEqual(0, info.MinExecutionDuration);
			Assert.AreEqual(0, info.MaxExecutionDuration);
			Assert.AreEqual(0, info.AverageExecutionDuration);

			Assert.IsTrue(info.isActive);
		}

		[NUnit.Framework.Test]
		public void CreatesSystemInfoForExecuteSystem()
		{
			var system = new TestExecuteSystem();
			var info = new SystemInfo(system);

			Assert.AreEqual("TestExecute", info.SystemName);
			Assert.IsFalse(info.IsInitializeSystems);
			Assert.IsTrue(info.IsExecuteSystems);
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
			Assert.IsFalse(info.IsExecuteSystems);
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
			Assert.IsFalse(info.IsExecuteSystems);
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
			Assert.IsFalse(info.IsExecuteSystems);
			Assert.IsFalse(info.IsCleanupSystems);
			Assert.IsFalse(info.IsTearDownSystems);
			Assert.IsTrue(info.IsReactiveSystems);
		}

		[NUnit.Framework.Test]
		public void AddsExecutionDuration()
		{
			_info.AddExecutionDuration(42);

			Assert.AreEqual(42, _info.AccumulatedExecutionDuration);
			Assert.AreEqual(42, _info.MinExecutionDuration);
			Assert.AreEqual(42, _info.MaxExecutionDuration);
			Assert.AreEqual(42, _info.AverageExecutionDuration);
		}

		[NUnit.Framework.Test]
		public void AddsAnotherExecutionDuration()
		{
			_info.AddExecutionDuration(20);
			_info.AddExecutionDuration(10);

			Assert.AreEqual(30, _info.AccumulatedExecutionDuration);
			Assert.AreEqual(10, _info.MinExecutionDuration);
			Assert.AreEqual(20, _info.MaxExecutionDuration);
			Assert.AreEqual(15, _info.AverageExecutionDuration);
		}

		[NUnit.Framework.Test]
		public void ResetsDurations()
		{
			_info.AddExecutionDuration(20);
			_info.AddExecutionDuration(10);

			_info.ResetDurations();

			Assert.AreEqual(0, _info.AccumulatedExecutionDuration);
			Assert.AreEqual(10, _info.MinExecutionDuration);
			Assert.AreEqual(20, _info.MaxExecutionDuration);
			Assert.AreEqual(0, _info.AverageExecutionDuration);
		}

		[NUnit.Framework.Test]
		public void KeepsMinDurationAfterReset()
		{
			_info.AddExecutionDuration(20);
			_info.AddExecutionDuration(10);

			_info.ResetDurations();

			_info.AddExecutionDuration(15);

			Assert.AreEqual(15, _info.AccumulatedExecutionDuration);
			Assert.AreEqual(10, _info.MinExecutionDuration);
			Assert.AreEqual(20, _info.MaxExecutionDuration);
			Assert.AreEqual(15, _info.AverageExecutionDuration);
		}
	}
}
