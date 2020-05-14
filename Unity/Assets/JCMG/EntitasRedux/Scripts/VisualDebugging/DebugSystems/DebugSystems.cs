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
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging
{
	public class DebugSystems : Systems
	{
		/// <summary>
		/// Returns the total number of <see cref="IInitializeSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalInitializeSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _initializeSystems.Count; i++)
				{
					var system = _initializeSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalInitializeSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="IUpdateSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalUpdateSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _updateSystems.Count; i++)
				{
					var system = _updateSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalUpdateSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="IFixedUpdateSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalFixedUpdateSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _fixedUpdateSystems.Count; i++)
				{
					var system = _fixedUpdateSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalFixedUpdateSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="ILateUpdateSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalLateUpdateSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _lateUpdateSystems.Count; i++)
				{
					var system = _lateUpdateSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalLateUpdateSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="IReactiveSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalReactiveSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _reactiveSystems.Count; i++)
				{
					var system = _reactiveSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalReactiveSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="ICleanupSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalCleanupSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _cleanupSystems.Count; i++)
				{
					var system = _cleanupSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalCleanupSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="ITearDownSystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalTearDownSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _tearDownSystems.Count; i++)
				{
					var system = _tearDownSystems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalTearDownSystemsCount : 1;
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the total number of <see cref="ISystem"/> instances in this <see cref="DebugSystems"/> and
		/// any child <see cref="DebugSystems"/>.
		/// </summary>
		public int TotalSystemsCount
		{
			get
			{
				var total = 0;
				for (var i = 0; i < _systems.Count; i++)
				{
					var system = _systems[i];
					total += system is DebugSystems debugSystems ? debugSystems.TotalSystemsCount : 1;
				}

				return total;
			}
		}

		public string Name => _name;

		public GameObject GameObject => _gameObject;

		public SystemInfo SystemInfo => _systemInfo;

		public double AverageUpdateDuration => _updateSystemInfos.Select(x => x.AverageUpdateDuration).Sum();

		public double AverageFixedUpdateDuration => _fixedUpdateSystemInfos.Select(x => x.AverageFixedUpdateDuration).Sum();

		public double AverageLateUpdateDuration => _lateUpdateSystemInfos.Select(x => x.AverageLateUpdateDuration).Sum();

		public double AverageReactiveDuration => _lateUpdateSystemInfos.Select(x => x.AverageReactiveDuration).Sum();

		public double AverageCleanupDuration => _cleanupSystemInfos.Select(x => x.AverageCleanupDuration).Sum();

		public double UpdateDuration => _updateDuration;

		public double FixedUpdateDuration => _fixedUpdateDuration;

		public double LateUpdateDuration => _lateUpdateDuration;

		public double ReactiveDuration => _reactiveDuration;

		public double CleanupDuration => _cleanupDuration;

		public IReadOnlyList<SystemInfo> InitializeSystemInfos => _initializeSystemInfos;

		public IReadOnlyList<SystemInfo> UpdateSystemInfos => _updateSystemInfos;

		public IReadOnlyList<SystemInfo> FixedUpdateSystemInfos => _fixedUpdateSystemInfos;

		public IReadOnlyList<SystemInfo> LateUpdateSystemInfos => _lateUpdateSystemInfos;

		public IReadOnlyList<SystemInfo> CleanupSystemInfos => _cleanupSystemInfos;

		public IReadOnlyList<SystemInfo> TearDownSystemInfos => _tearDownSystemInfos;

		public IReadOnlyList<SystemInfo> ReactiveSystemInfos => _reactiveSystemInfos;

		private string _name;
		private GameObject _gameObject;

		private double _reactiveDuration;
		private double _fixedUpdateDuration;
		private double _updateDuration;
		private double _lateUpdateDuration;
		private double _cleanupDuration;

		private List<SystemInfo> _allSystemInfos;
		private List<SystemInfo> _initializeSystemInfos;
		private List<SystemInfo> _updateSystemInfos;
		private List<SystemInfo> _fixedUpdateSystemInfos;
		private List<SystemInfo> _lateUpdateSystemInfos;
		private List<SystemInfo> _reactiveSystemInfos;
		private List<SystemInfo> _cleanupSystemInfos;
		private List<SystemInfo> _tearDownSystemInfos;

		private Stopwatch _stopwatch;
		private SystemInfo _systemInfo;

		private List<ISystem> _systems;

		public static AvgResetInterval avgResetInterval;

		static DebugSystems()
		{
			avgResetInterval = AvgResetInterval.Slow;
		}

		public DebugSystems(string name)
		{
			Initialize(name);
		}

		protected DebugSystems(bool noInit)
		{
		}

		protected void Initialize(string name)
		{
			_name = name;
			_gameObject = new GameObject(name);
			_gameObject.AddComponent<DebugSystemsBehaviour>().Init(this);

			_systemInfo = new SystemInfo(this);

			_allSystemInfos = new List<SystemInfo>();
			_initializeSystemInfos = new List<SystemInfo>();
			_updateSystemInfos = new List<SystemInfo>();
			_fixedUpdateSystemInfos = new List<SystemInfo>();
			_lateUpdateSystemInfos = new List<SystemInfo>();
			_reactiveSystemInfos = new List<SystemInfo>();
			_cleanupSystemInfos = new List<SystemInfo>();
			_tearDownSystemInfos = new List<SystemInfo>();

			_systems = new List<ISystem>();

			_stopwatch = new Stopwatch();
		}

		public override Systems Add(ISystem system)
		{
			_systems.Add(system);

			SystemInfo childSystemInfo;

			if (system is DebugSystems debugSystems)
			{
				childSystemInfo = debugSystems.SystemInfo;
				debugSystems.GameObject.transform.SetParent(_gameObject.transform, false);
			}
			else
			{
				childSystemInfo = new SystemInfo(system);
			}

			_allSystemInfos.Add(childSystemInfo);

			childSystemInfo.parentSystemInfo = _systemInfo;

			if (childSystemInfo.IsInitializeSystems)
			{
				_initializeSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsUpdateSystems || childSystemInfo.IsReactiveSystems)
			{
				_updateSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsFixedUpdateSystems)
			{
				_fixedUpdateSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsLateUpdateSystems)
			{
				_lateUpdateSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsReactiveSystems)
			{
				_reactiveSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsCleanupSystems)
			{
				_cleanupSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsTearDownSystems)
			{
				_tearDownSystemInfos.Add(childSystemInfo);
			}

			return base.Add(system);
		}

		public void ResetDurations()
		{
			for (var i = 0; i < _allSystemInfos.Count; i++)
			{
				var systemInfo = _allSystemInfos[i];
				systemInfo.ResetFrameDurations();
			}

			for (var i = 0; i < _systems.Count; i++)
			{
				var system = _systems[i];
				if (system is DebugSystems debugSystems)
				{
					debugSystems.ResetDurations();
				}
			}
		}

		public override void Initialize()
		{
			for (var i = 0; i < _initializeSystems.Count; i++)
			{
				var systemInfo = _initializeSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_initializeSystems[i].Initialize();
					_stopwatch.Stop();
					systemInfo.InitializationDuration = _stopwatch.Elapsed.TotalMilliseconds;
				}
			}
		}

		public override void Update()
		{
			_updateDuration = 0;
			if (Time.frameCount % (int)avgResetInterval == 0)
			{
				ResetDurations();
			}

			for (var i = 0; i < _updateSystems.Count; i++)
			{
				var systemInfo = _updateSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_updateSystems[i].Update();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_updateDuration += duration;
					systemInfo.AddUpdateDuration(duration);
				}
			}
		}

		public override void FixedUpdate()
		{
			_fixedUpdateDuration = 0;
			for (var i = 0; i < _fixedUpdateSystems.Count; i++)
			{
				var systemInfo = _fixedUpdateSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_fixedUpdateSystems[i].FixedUpdate();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_fixedUpdateDuration += duration;
					systemInfo.AddFixedUpdateDuration(duration);
				}
			}
		}

		public override void LateUpdate()
		{
			_lateUpdateDuration = 0;
			for (var i = 0; i < _lateUpdateSystems.Count; i++)
			{
				var systemInfo = _lateUpdateSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_lateUpdateSystems[i].LateUpdate();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_lateUpdateDuration += duration;
					systemInfo.AddLateUpdateDuration(duration);
				}
			}
		}

		public override void Execute()
		{
			_reactiveDuration = 0;
			for (var i = 0; i < _reactiveSystems.Count; i++)
			{
				var systemInfo = _reactiveSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_reactiveSystems[i].Execute();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_reactiveDuration += duration;
					systemInfo.AddReactiveDuration(duration);
				}
			}
		}

		public override void Cleanup()
		{
			_cleanupDuration = 0;
			for (var i = 0; i < _cleanupSystems.Count; i++)
			{
				var systemInfo = _cleanupSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_cleanupSystems[i].Cleanup();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_cleanupDuration += duration;
					systemInfo.AddCleanupDuration(duration);
				}
			}
		}

		public override void TearDown()
		{
			for (var i = 0; i < _tearDownSystems.Count; i++)
			{
				var systemInfo = _tearDownSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_tearDownSystems[i].TearDown();
					_stopwatch.Stop();
					systemInfo.TeardownDuration = _stopwatch.Elapsed.TotalMilliseconds;
				}
			}
		}
	}
}
