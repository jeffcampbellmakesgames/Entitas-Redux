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

using System.Collections.Generic;

namespace JCMG.EntitasRedux
{
	/// <summary>
	/// Systems provide a convenient way to group systems.
	/// You can add IInitializeSystem, IExecuteSystem, ICleanupSystem,
	/// ITearDownSystem, ReactiveSystem and other nested Systems instances.
	/// All systems will be initialized and executed based on the order
	/// you added them.
	/// </summary>
	public class Systems : IInitializeSystem,
	                       IExecuteSystem,
	                       IFixedUpdateSystem,
						   IUpdateSystem,
						   ILateUpdateSystem,
	                       ICleanupSystem,
	                       ITearDownSystem
	{
		protected readonly List<IExecuteSystem> _executeSystems;
		protected readonly List<IFixedUpdateSystem> _fixedUpdateSystems;
		protected readonly List<IUpdateSystem> _updateSystems;
		protected readonly List<ILateUpdateSystem> _lateUpdateSystems;

		protected readonly List<IInitializeSystem> _initializeSystems;
		protected readonly List<ICleanupSystem> _cleanupSystems;
		protected readonly List<ITearDownSystem> _tearDownSystems;

		/// <summary>
		/// Creates a new Systems instance.
		/// </summary>
		public Systems()
		{
			_executeSystems = new List<IExecuteSystem>();
			_fixedUpdateSystems = new List<IFixedUpdateSystem>();
			_updateSystems = new List<IUpdateSystem>();
			_lateUpdateSystems = new List<ILateUpdateSystem>();

			_initializeSystems = new List<IInitializeSystem>();
			_cleanupSystems = new List<ICleanupSystem>();
			_tearDownSystems = new List<ITearDownSystem>();
		}

		/// <summary>
		/// Adds the <see cref="ISystem"/> instance to the systems list.
		/// </summary>
		/// <param name="system"></param>
		/// <returns></returns>
		public virtual Systems Add(ISystem system)
		{
			if (system is IInitializeSystem initializeSystem)
			{
				_initializeSystems.Add(initializeSystem);
			}

			if (system is IExecuteSystem executeSystem)
			{
				_executeSystems.Add(executeSystem);
			}

			if (system is IFixedUpdateSystem fixedUpdateSystem)
			{
				_fixedUpdateSystems.Add(fixedUpdateSystem);
			}

			if (system is IUpdateSystem updateSystem)
			{
				_updateSystems.Add(updateSystem);
			}

			if (system is ILateUpdateSystem lateUpdateSystem)
			{
				_lateUpdateSystems.Add(lateUpdateSystem);
			}

			if (system is ICleanupSystem cleanupSystem)
			{
				_cleanupSystems.Add(cleanupSystem);
			}

			if (system is ITearDownSystem tearDownSystem)
			{
				_tearDownSystems.Add(tearDownSystem);
			}

			return this;
		}

		/// <summary>
		/// Activates all <see cref="IReactiveSystem"/> in the systems list.
		/// </summary>
		public void ActivateReactiveSystems()
		{
			for (var i = 0; i < _executeSystems.Count; i++)
			{
				var system = _executeSystems[i];
				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Activate();
				}

				if (system is Systems nestedSystems)
				{
					nestedSystems.ActivateReactiveSystems();
				}
			}
		}

		/// <summary>
		/// <para>Deactivates all <see cref="IReactiveSystem"/> in the systems list. This will also clear all
		/// <see cref="IReactiveSystem"/>s.</para>
		///
		/// <para>This is useful when you want to soft-restart your application and
		/// want to reuse your existing system instances.</para>
		/// </summary>
		public void DeactivateReactiveSystems()
		{
			for (var i = 0; i < _executeSystems.Count; i++)
			{
				var system = _executeSystems[i];
				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Deactivate();
				}

				if (system is Systems nestedSystems)
				{
					nestedSystems.DeactivateReactiveSystems();
				}
			}
		}

		/// <summary>
		/// Clears all <see cref="IReactiveSystem"/> in the systems list.
		/// </summary>
		public void ClearReactiveSystems()
		{
			for (var i = 0; i < _executeSystems.Count; i++)
			{
				var system = _executeSystems[i];
				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Clear();
				}

				if (system is Systems nestedSystems)
				{
					nestedSystems.ClearReactiveSystems();
				}
			}
		}

		/// <summary>
		/// Calls Execute() on all <see cref="IExecuteSystem"/> and other nested Systems instances in the order you
		/// added them.
		/// </summary>
		public virtual void Execute()
		{
			for (var i = 0; i < _executeSystems.Count; i++)
			{
				_executeSystems[i].Execute();
			}
		}

		/// <summary>
		/// Calls FixedUpdate() on all <see cref="IFixedUpdateSystem"/> and other nested systems instances in the order
		/// you added them.
		/// </summary>
		public void FixedUpdate()
		{
			for (var i = 0; i < _fixedUpdateSystems.Count; i++)
			{
				_fixedUpdateSystems[i].FixedUpdate();
			}
		}

		/// <summary>
		/// Calls Update() on all <see cref="IUpdateSystem"/> and other nested systems instances in the order you added
		/// them.
		/// </summary>
		public void Update()
		{
			for (var i = 0; i < _updateSystems.Count; i++)
			{
				_updateSystems[i].Update();
			}
		}

		/// <summary>
		/// Calls LateUpdate() on all <see cref="ILateUpdateSystem"/> and other nested systems instances in the order
		/// you added them.
		/// </summary>
		public void LateUpdate()
		{
			for (var i = 0; i < _lateUpdateSystems.Count; i++)
			{
				_lateUpdateSystems[i].LateUpdate();
			}
		}

		/// <summary>
		/// Calls Initialize() on all <see cref="IInitializeSystem"/> and other nested systems instances in the order
		/// you added them.
		/// </summary>
		public virtual void Initialize()
		{
			for (var i = 0; i < _initializeSystems.Count; i++)
			{
				_initializeSystems[i].Initialize();
			}
		}

		/// <summary>
		/// Calls Cleanup() on all <see cref="ICleanupSystem"/> and other nested systems instances in the order you
		/// added them.
		/// </summary>
		public virtual void Cleanup()
		{
			for (var i = 0; i < _cleanupSystems.Count; i++)
			{
				_cleanupSystems[i].Cleanup();
			}
		}

		/// <summary>
		/// Calls Teardown() on all <see cref="ITearDownSystem"/> and other nested systems instances in the order you
		/// added them.
		/// </summary>
		public virtual void TearDown()
		{
			for (var i = 0; i < _tearDownSystems.Count; i++)
			{
				_tearDownSystems[i].TearDown();
			}
		}
	}
}
