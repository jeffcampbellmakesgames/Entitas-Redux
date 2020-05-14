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

using JCMG.EntitasRedux;
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeSystems
	{
		private MyTestContext _ctx = null;
		private Systems _systems = null;

		[SetUp]
		public void Setup()
		{
			_ctx = new MyTestContext();
			_systems = new Systems();
		}

		#region Fixtures

		[NUnit.Framework.Test]
		public void InitializesInitializeSystemSpy()
		{
			var system = new InitializeSystemSpy();

			Assert.AreEqual(0, system.DidInitialize);

			system.Initialize();

			Assert.AreEqual(1, system.DidInitialize);
		}

		[NUnit.Framework.Test]
		public void ExecutesExecuteSystemSpy()
		{
			var system = new UpdateSystemSpy();

			Assert.AreEqual(0, system.DidExecute);

			system.Update();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void ExecutesFixedUpdateSystemSpy()
		{
			var system = new FixedUpdateSystemSpy();

			Assert.AreEqual(0, system.DidExecute);

			system.FixedUpdate();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void ExecutesLateUpdateSystemSpy()
		{
			var system = new LateUpdateSystemSpy();

			Assert.AreEqual(0, system.DidExecute);

			system.LateUpdate();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void CleansUpCleanupSystemSpy()
		{
			var system = new CleanupSystemSpy();

			Assert.AreEqual(0, system.DidCleanup);

			system.Cleanup();

			Assert.AreEqual(1, system.DidCleanup);
		}

		[NUnit.Framework.Test]
		public void TearsDownTearDownSystemSpy()
		{
			var system = new TearDownSystemSpy();

			Assert.AreEqual(0, system.DidTearDown);

			system.TearDown();

			Assert.AreEqual(1, system.DidTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_ctx.CreateEntity().AddComponentA();

			Assert.AreEqual(0, system.DidInitialize);
			system.Initialize();
			Assert.AreEqual(1, system.DidInitialize);

			Assert.AreEqual(0, system.DidExecute);
			system.Execute();
			Assert.AreEqual(1, system.DidExecute);

			Assert.AreEqual(0, system.DidCleanup);
			system.Cleanup();
			Assert.AreEqual(1, system.DidCleanup);

			Assert.AreEqual(0, system.DidTearDown);
			system.TearDown();
			Assert.AreEqual(1, system.DidTearDown);
		}

		[NUnit.Framework.Test]
		public void ExecutesReactiveSystemSpy()
		{
			var system = CreateReactiveSystem(_ctx);

			system.Execute();

			Assert.AreEqual(1, system.Entities.Length);
		}

		#endregion

		#region Systems

		[NUnit.Framework.Test]
		public void ReturnsSystemsWhenAddingSystem()
		{
			Assert.AreEqual(_systems, _systems.Add(new InitializeSystemSpy()));
		}

		[NUnit.Framework.Test]
		public void SystemsInitializesIInitializeSystem()
		{
			var system = new InitializeSystemSpy();
			_systems.Add(system);
			_systems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);
		}

		[NUnit.Framework.Test]
		public void SystemsExecutesIExecuteSystemSystem()
		{
			var system = new UpdateSystemSpy();
			_systems.Add(system);
			_systems.Update();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsExecutesIFixedUpdateSystemSystem()
		{
			var system = new FixedUpdateSystemSpy();
			_systems.Add(system);
			_systems.FixedUpdate();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsExecutesILateUpdateSystemSystem()
		{
			var system = new LateUpdateSystemSpy();
			_systems.Add(system);
			_systems.LateUpdate();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void WrapsReactiveSystemInIReactiveSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_systems.Add(system);
			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void AddsReactiveSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_systems.Add(system);
			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void CleansUpICleanupSystem()
		{
			var system = new CleanupSystemSpy();
			_systems.Add(system);
			_systems.Cleanup();

			Assert.AreEqual(1, system.DidCleanup);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownInterfacesOfReactiveSystemSpy()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_ctx.CreateEntity().AddComponentA();

			_systems.Add(system);

			Assert.AreEqual(0, system.DidInitialize);
			_systems.Initialize();
			Assert.AreEqual(1, system.DidInitialize);

			Assert.AreEqual(0, system.DidExecute);
			_systems.Execute();
			Assert.AreEqual(1, system.DidExecute);

			Assert.AreEqual(0, system.DidCleanup);
			_systems.Cleanup();
			Assert.AreEqual(1, system.DidCleanup);

			Assert.AreEqual(0, system.DidTearDown);
			_systems.TearDown();
			Assert.AreEqual(1, system.DidTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownReactiveSystem()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			Assert.AreEqual(0, system.DidInitialize);
			_systems.Initialize();
			Assert.AreEqual(1, system.DidInitialize);

			Assert.AreEqual(0, system.DidExecute);
			_systems.Execute();
			_systems.Execute();
			Assert.AreEqual(1, system.DidExecute);

			Assert.AreEqual(0, system.DidCleanup);
			_systems.Cleanup();
			Assert.AreEqual(1, system.DidCleanup);

			Assert.AreEqual(0, system.DidTearDown);
			_systems.TearDown();
			Assert.AreEqual(1, system.DidTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			var parentSystems = new Systems();
			parentSystems.Add(_systems);

			Assert.AreEqual(0, system.DidInitialize);
			parentSystems.Initialize();
			Assert.AreEqual(1, system.DidInitialize);

			Assert.AreEqual(0, system.DidExecute);
			parentSystems.Execute();
			parentSystems.Execute();
			Assert.AreEqual(1, system.DidExecute);

			Assert.AreEqual(0, system.DidCleanup);
			parentSystems.Cleanup();
			Assert.AreEqual(1, system.DidCleanup);

			Assert.AreEqual(0, system.DidTearDown);
			parentSystems.TearDown();
			Assert.AreEqual(1, system.DidTearDown);

		}

		[NUnit.Framework.Test]
		public void SystemsClearsReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			_systems.Clear();
			_systems.Update();

			Assert.AreEqual(0, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsClearsReactiveSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			parentSystems.Clear();
			parentSystems.Update();

			Assert.AreEqual(0, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsDeactivatesReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			_systems.Deactivate();
			_systems.Update();

			Assert.AreEqual(0, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsDeactivatesSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			parentSystems.Deactivate();
			parentSystems.Update();

			Assert.AreEqual(0, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsActivatesReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			_systems.Deactivate();
			_systems.Activate();
			_systems.Execute();

			Assert.AreEqual(0, system.DidExecute);

			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsReactivatesSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.DidInitialize);

			parentSystems.Deactivate();
			parentSystems.Activate();
			parentSystems.Update();

			Assert.AreEqual(0, system.DidExecute);

			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.DidExecute);
		}

		#endregion

		#region Helpers

		private static ReactiveSystemSpy CreateReactiveSystem(MyTestContext context)
		{
			var system = new ReactiveSystemSpy(context.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			context.CreateEntity().AddComponentA();

			return system;
		}

		#endregion
	}
}
