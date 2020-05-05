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
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging
{
	public enum AvgResetInterval
	{
		Always = 1,
		VeryFast = 30,
		Fast = 60,
		Normal = 120,
		Slow = 300,
		Never = int.MaxValue
	}

	public class DebugSystems : Systems
	{
		public int TotalInitializeSystemsCount
		{
			get
			{
				var total = 0;
				foreach (var system in _initializeSystems)
				{
					total += system is DebugSystems debugSystems ? debugSystems.TotalInitializeSystemsCount : 1;
				}

				return total;
			}
		}

		public int TotalExecuteSystemsCount
		{
			get
			{
				var total = 0;
				foreach (var system in _executeSystems)
				{
					total += system is DebugSystems debugSystems ? debugSystems.TotalExecuteSystemsCount : 1;
				}

				return total;
			}
		}

		public int TotalCleanupSystemsCount
		{
			get
			{
				var total = 0;
				foreach (var system in _cleanupSystems)
				{
					total += system is DebugSystems debugSystems ? debugSystems.TotalCleanupSystemsCount : 1;
				}

				return total;
			}
		}

		public int TotalTearDownSystemsCount
		{
			get
			{
				var total = 0;
				foreach (var system in _tearDownSystems)
				{
					total += system is DebugSystems debugSystems ? debugSystems.TotalTearDownSystemsCount : 1;
				}

				return total;
			}
		}

		public int TotalSystemsCount
		{
			get
			{
				var total = 0;
				foreach (var system in _systems)
				{
					total += system is DebugSystems debugSystems ? debugSystems.TotalSystemsCount : 1;
				}

				return total;
			}
		}

		public int InitializeSystemsCount
		{
			get { return _initializeSystems.Count; }
		}

		public int ExecuteSystemsCount
		{
			get { return _executeSystems.Count; }
		}

		public int CleanupSystemsCount
		{
			get { return _cleanupSystems.Count; }
		}

		public int TearDownSystemsCount
		{
			get { return _tearDownSystems.Count; }
		}

		public string Name
		{
			get { return _name; }
		}

		public GameObject GameObject
		{
			get { return _gameObject; }
		}

		public SystemInfo SystemInfo
		{
			get { return _systemInfo; }
		}

		public double ExecuteDuration
		{
			get { return _executeDuration; }
		}

		public double CleanupDuration
		{
			get { return _cleanupDuration; }
		}

		public SystemInfo[] InitializeSystemInfos
		{
			get { return _initializeSystemInfos.ToArray(); }
		}

		public SystemInfo[] ExecuteSystemInfos
		{
			get { return _executeSystemInfos.ToArray(); }
		}

		public SystemInfo[] CleanupSystemInfos
		{
			get { return _cleanupSystemInfos.ToArray(); }
		}

		public SystemInfo[] TearDownSystemInfos
		{
			get { return _tearDownSystemInfos.ToArray(); }
		}

		private double _cleanupDuration;
		private List<SystemInfo> _cleanupSystemInfos;

		private double _executeDuration;
		private List<SystemInfo> _executeSystemInfos;
		private GameObject _gameObject;

		private List<SystemInfo> _initializeSystemInfos;

		private string _name;

		private Stopwatch _stopwatch;
		private SystemInfo _systemInfo;

		private List<ISystem> _systems;
		private List<SystemInfo> _tearDownSystemInfos;

		public bool paused;

		public static AvgResetInterval avgResetInterval = AvgResetInterval.Never;

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

			_systems = new List<ISystem>();
			_initializeSystemInfos = new List<SystemInfo>();
			_executeSystemInfos = new List<SystemInfo>();
			_cleanupSystemInfos = new List<SystemInfo>();
			_tearDownSystemInfos = new List<SystemInfo>();

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

			childSystemInfo.parentSystemInfo = _systemInfo;

			if (childSystemInfo.IsInitializeSystems)
			{
				_initializeSystemInfos.Add(childSystemInfo);
			}

			if (childSystemInfo.IsExecuteSystems || childSystemInfo.IsReactiveSystems)
			{
				_executeSystemInfos.Add(childSystemInfo);
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
			foreach (var systemInfo in _executeSystemInfos)
			{
				systemInfo.ResetDurations();
			}

			foreach (var system in _systems)
			{
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

		public override void Execute()
		{
			if (!paused)
			{
				StepExecute();
			}
		}

		public override void Cleanup()
		{
			if (!paused)
			{
				StepCleanup();
			}
		}

		public void StepExecute()
		{
			_executeDuration = 0;
			if (Time.frameCount % (int)avgResetInterval == 0)
			{
				ResetDurations();
			}

			for (var i = 0; i < _executeSystems.Count; i++)
			{
				var systemInfo = _executeSystemInfos[i];
				if (systemInfo.isActive)
				{
					_stopwatch.Reset();
					_stopwatch.Start();
					_executeSystems[i].Execute();
					_stopwatch.Stop();
					var duration = _stopwatch.Elapsed.TotalMilliseconds;
					_executeDuration += duration;
					systemInfo.AddExecutionDuration(duration);
				}
			}
		}

		public void StepCleanup()
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
