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
		private JCMG.EntitasRedux.Systems _systems = null;

		[SetUp]
		public void Setup()
		{
			_ctx = new MyTestContext();
			_systems = new JCMG.EntitasRedux.Systems();
		}

		#region Fixtures

		[NUnit.Framework.Test]
		public void InitializesInitializeSystemSpy()
		{
			var system = new InitializeSystemSpy();

			Assert.AreEqual(0, system.didInitialize);

			system.Initialize();

			Assert.AreEqual(1, system.didInitialize);
		}

		[NUnit.Framework.Test]
		public void ExecutesExecuteSystemSpy()
		{
			var system = new ExecuteSystemSpy();

			Assert.AreEqual(0, system.didExecute);

			system.Execute();

			Assert.AreEqual(1, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void CleansUpCleanupSystemSpy()
		{
			var system = new CleanupSystemSpy();

			Assert.AreEqual(0, system.didCleanup);

			system.Cleanup();

			Assert.AreEqual(1, system.didCleanup);
		}

		[NUnit.Framework.Test]
		public void TearsDownTearDownSystemSpy()
		{
			var system = new TearDownSystemSpy();

			Assert.AreEqual(0, system.didTearDown);

			system.TearDown();

			Assert.AreEqual(1, system.didTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_ctx.CreateEntity().AddComponentA();

			Assert.AreEqual(0, system.didInitialize);
			system.Initialize();
			Assert.AreEqual(1, system.didInitialize);

			Assert.AreEqual(0, system.didExecute);
			system.Execute();
			Assert.AreEqual(1, system.didExecute);

			Assert.AreEqual(0, system.didCleanup);
			system.Cleanup();
			Assert.AreEqual(1, system.didCleanup);

			Assert.AreEqual(0, system.didTearDown);
			system.TearDown();
			Assert.AreEqual(1, system.didTearDown);
		}

		[NUnit.Framework.Test]
		public void ExecutesReactiveSystemSpy()
		{
			var system = CreateReactiveSystem(_ctx);

			system.Execute();

			Assert.AreEqual(1, system.entities.Length);
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

			Assert.AreEqual(1, system.didInitialize);
		}

		[NUnit.Framework.Test]
		public void SystemsExecutesIExecuteSystemSystem()
		{
			var system = new ExecuteSystemSpy();
			_systems.Add(system);
			_systems.Execute();

			Assert.AreEqual(1, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void WrapsReactiveSystemInIReactiveSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_systems.Add(system);
			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void AddsReactiveSystem()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_systems.Add(system);
			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void CleansUpICleanupSystem()
		{
			var system = new CleanupSystemSpy();
			_systems.Add(system);
			_systems.Cleanup();

			Assert.AreEqual(1, system.didCleanup);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownInterfacesOfReactiveSystemSpy()
		{
			var system = new ReactiveSystemSpy(_ctx.CreateCollector(Matcher<MyTestEntity>.AllOf(CID.ComponentA)));
			_ctx.CreateEntity().AddComponentA();

			_systems.Add(system);

			Assert.AreEqual(0, system.didInitialize);
			_systems.Initialize();
			Assert.AreEqual(1, system.didInitialize);

			Assert.AreEqual(0, system.didExecute);
			_systems.Execute();
			Assert.AreEqual(1, system.didExecute);

			Assert.AreEqual(0, system.didCleanup);
			_systems.Cleanup();
			Assert.AreEqual(1, system.didCleanup);

			Assert.AreEqual(0, system.didTearDown);
			_systems.TearDown();
			Assert.AreEqual(1, system.didTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownReactiveSystem()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			Assert.AreEqual(0, system.didInitialize);
			_systems.Initialize();
			Assert.AreEqual(1, system.didInitialize);

			Assert.AreEqual(0, system.didExecute);
			_systems.Execute();
			_systems.Execute();
			Assert.AreEqual(1, system.didExecute);

			Assert.AreEqual(0, system.didCleanup);
			_systems.Cleanup();
			Assert.AreEqual(1, system.didCleanup);

			Assert.AreEqual(0, system.didTearDown);
			_systems.TearDown();
			Assert.AreEqual(1, system.didTearDown);
		}

		[NUnit.Framework.Test]
		public void InitializesExecutesCleansUpAndTearsDownSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			var parentSystems = new JCMG.EntitasRedux.Systems();
			parentSystems.Add(_systems);

			Assert.AreEqual(0, system.didInitialize);
			parentSystems.Initialize();
			Assert.AreEqual(1, system.didInitialize);

			Assert.AreEqual(0, system.didExecute);
			parentSystems.Execute();
			parentSystems.Execute();
			Assert.AreEqual(1, system.didExecute);

			Assert.AreEqual(0, system.didCleanup);
			parentSystems.Cleanup();
			Assert.AreEqual(1, system.didCleanup);

			Assert.AreEqual(0, system.didTearDown);
			parentSystems.TearDown();
			Assert.AreEqual(1, system.didTearDown);

		}

		[NUnit.Framework.Test]
		public void SystemsClearsReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			_systems.ClearReactiveSystems();
			_systems.Execute();

			Assert.AreEqual(0, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsClearsReactiveSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new JCMG.EntitasRedux.Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			parentSystems.ClearReactiveSystems();
			parentSystems.Execute();

			Assert.AreEqual(0, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsDeactivatesReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			_systems.DeactivateReactiveSystems();
			_systems.Execute();

			Assert.AreEqual(0, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsDeactivatesSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new JCMG.EntitasRedux.Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			parentSystems.DeactivateReactiveSystems();
			parentSystems.Execute();

			Assert.AreEqual(0, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsActivatesReactiveSystems()
		{
			var system = CreateReactiveSystem(_ctx);

			_systems.Add(system);

			_systems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			_systems.DeactivateReactiveSystems();
			_systems.ActivateReactiveSystems();
			_systems.Execute();

			Assert.AreEqual(0, system.didExecute);

			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.didExecute);
		}

		[NUnit.Framework.Test]
		public void SystemsReactivatesSystemsRecursively()
		{
			var system = CreateReactiveSystem(_ctx);
			_systems.Add(system);

			var parentSystems = new JCMG.EntitasRedux.Systems();
			parentSystems.Add(_systems);

			parentSystems.Initialize();

			Assert.AreEqual(1, system.didInitialize);

			parentSystems.DeactivateReactiveSystems();
			parentSystems.ActivateReactiveSystems();
			parentSystems.Execute();

			Assert.AreEqual(0, system.didExecute);

			_ctx.CreateEntity().AddComponentA();
			_systems.Execute();

			Assert.AreEqual(1, system.didExecute);
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
