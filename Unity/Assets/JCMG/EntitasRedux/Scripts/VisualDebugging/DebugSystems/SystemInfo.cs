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

using System;

namespace JCMG.EntitasRedux.VisualDebugging
{
	[Flags]
	public enum SystemInterfaceFlags
	{
		None = 0,
		InitializeSystem = 1 << 1,
		ExecuteSystem = 1 << 2,
		CleanupSystem = 1 << 3,
		TearDownSystem = 1 << 4,
		ReactiveSystem = 1 << 5
	}

	public class SystemInfo
	{
		public ISystem System
		{
			get { return _system; }
		}

		public string SystemName
		{
			get { return _systemName; }
		}

		public bool IsInitializeSystems
		{
			get { return (_interfaceFlags & SystemInterfaceFlags.InitializeSystem) == SystemInterfaceFlags.InitializeSystem; }
		}

		public bool IsExecuteSystems
		{
			get { return (_interfaceFlags & SystemInterfaceFlags.ExecuteSystem) == SystemInterfaceFlags.ExecuteSystem; }
		}

		public bool IsCleanupSystems
		{
			get { return (_interfaceFlags & SystemInterfaceFlags.CleanupSystem) == SystemInterfaceFlags.CleanupSystem; }
		}

		public bool IsTearDownSystems
		{
			get { return (_interfaceFlags & SystemInterfaceFlags.TearDownSystem) == SystemInterfaceFlags.TearDownSystem; }
		}

		public bool IsReactiveSystems
		{
			get { return (_interfaceFlags & SystemInterfaceFlags.ReactiveSystem) == SystemInterfaceFlags.ReactiveSystem; }
		}

		public double InitializationDuration { get; set; }

		public double AccumulatedExecutionDuration
		{
			get { return _accumulatedExecutionDuration; }
		}

		public double MinExecutionDuration
		{
			get { return _minExecutionDuration; }
		}

		public double MaxExecutionDuration
		{
			get { return _maxExecutionDuration; }
		}

		public double AverageExecutionDuration
		{
			get { return _executionDurationsCount == 0 ? 0 : _accumulatedExecutionDuration / _executionDurationsCount; }
		}

		public double AccumulatedCleanupDuration
		{
			get { return _accumulatedCleanupDuration; }
		}

		public double MinCleanupDuration
		{
			get { return _minCleanupDuration; }
		}

		public double MaxCleanupDuration
		{
			get { return _maxCleanupDuration; }
		}

		public double AverageCleanupDuration
		{
			get { return _cleanupDurationsCount == 0 ? 0 : _accumulatedCleanupDuration / _cleanupDurationsCount; }
		}

		public double CleanupDuration { get; set; }
		public double TeardownDuration { get; set; }

		public bool AreAllParentsActive
		{
			get { return parentSystemInfo == null || parentSystemInfo.isActive && parentSystemInfo.AreAllParentsActive; }
		}

		private readonly SystemInterfaceFlags _interfaceFlags;

		private readonly ISystem _system;
		private readonly string _systemName;

		private double _accumulatedCleanupDuration;

		private double _accumulatedExecutionDuration;
		private int _cleanupDurationsCount;
		private int _executionDurationsCount;
		private double _maxCleanupDuration;
		private double _maxExecutionDuration;
		private double _minCleanupDuration;
		private double _minExecutionDuration;
		public bool isActive;

		public SystemInfo parentSystemInfo;

		public SystemInfo(ISystem system)
		{
			_system = system;
			_interfaceFlags = GetInterfaceFlags(system);

			_systemName = system is DebugSystems debugSystem
				? debugSystem.Name
				: system.GetType().Name.RemoveSystemSuffix();

			isActive = true;
		}

		public void AddExecutionDuration(double executionDuration)
		{
			if (executionDuration < _minExecutionDuration || _minExecutionDuration == 0)
			{
				_minExecutionDuration = executionDuration;
			}

			if (executionDuration > _maxExecutionDuration)
			{
				_maxExecutionDuration = executionDuration;
			}

			_accumulatedExecutionDuration += executionDuration;
			_executionDurationsCount += 1;
		}

		public void AddCleanupDuration(double cleanupDuration)
		{
			if (cleanupDuration < _minCleanupDuration || _minCleanupDuration == 0)
			{
				_minCleanupDuration = cleanupDuration;
			}

			if (cleanupDuration > _maxCleanupDuration)
			{
				_maxCleanupDuration = cleanupDuration;
			}

			_accumulatedCleanupDuration += cleanupDuration;
			_cleanupDurationsCount += 1;
		}

		public void ResetDurations()
		{
			_accumulatedExecutionDuration = 0;
			_executionDurationsCount = 0;

			_accumulatedCleanupDuration = 0;
			_cleanupDurationsCount = 0;
		}

		private static SystemInterfaceFlags GetInterfaceFlags(ISystem system)
		{
			var flags = SystemInterfaceFlags.None;
			if (system is IInitializeSystem)
			{
				flags |= SystemInterfaceFlags.InitializeSystem;
			}

			if (system is IReactiveSystem)
			{
				flags |= SystemInterfaceFlags.ReactiveSystem;
			}
			else if (system is IExecuteSystem)
			{
				flags |= SystemInterfaceFlags.ExecuteSystem;
			}

			if (system is ICleanupSystem)
			{
				flags |= SystemInterfaceFlags.CleanupSystem;
			}

			if (system is ITearDownSystem)
			{
				flags |= SystemInterfaceFlags.TearDownSystem;
			}

			return flags;
		}
	}
}
