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
		FixedUpdateSystem = 1 << 6,
		UpdateSystem = 1 << 2,
		LateUpdateSystem = 1 << 7,
		ReactiveSystem = 1 << 5,
		CleanupSystem = 1 << 3,
		TearDownSystem = 1 << 4
	}

	public class SystemInfo
	{
		public ISystem System { get; }

		public string SystemName { get; }

		public bool AreAllParentsActive =>
			parentSystemInfo == null || parentSystemInfo.isActive && parentSystemInfo.AreAllParentsActive;

		#region SystemInterfaceFlag Properties

		public bool IsInitializeSystems =>
			(_interfaceFlags & SystemInterfaceFlags.InitializeSystem) == SystemInterfaceFlags.InitializeSystem;

		public bool IsUpdateSystems =>
			(_interfaceFlags & SystemInterfaceFlags.UpdateSystem) == SystemInterfaceFlags.UpdateSystem;

		public bool IsFixedUpdateSystems =>
			(_interfaceFlags & SystemInterfaceFlags.FixedUpdateSystem) == SystemInterfaceFlags.FixedUpdateSystem;

		public bool IsLateUpdateSystems =>
			(_interfaceFlags & SystemInterfaceFlags.LateUpdateSystem) == SystemInterfaceFlags.LateUpdateSystem;

		public bool IsCleanupSystems =>
			(_interfaceFlags & SystemInterfaceFlags.CleanupSystem) == SystemInterfaceFlags.CleanupSystem;

		public bool IsTearDownSystems =>
			(_interfaceFlags & SystemInterfaceFlags.TearDownSystem) == SystemInterfaceFlags.TearDownSystem;

		public bool IsReactiveSystems =>
			(_interfaceFlags & SystemInterfaceFlags.ReactiveSystem) == SystemInterfaceFlags.ReactiveSystem;

		#endregion

		#region FixedUpdate System Properties

		public double MinFixedUpdateDuration => _minFixedUpdateDuration;

		public double MaxFixedUpdateDuration => _maxFixedUpdateDuration;

		public double AccumulatedFixedUpdateDuration => _accumulatedFixedUpdateDuration;

		public double AverageFixedUpdateDuration =>
			_fixedUpdateDurationsCount == 0 ? 0 : _accumulatedFixedUpdateDuration / _fixedUpdateDurationsCount;

		#endregion

		#region Update System Properties

		public double MinUpdateDuration => _minUpdateDuration;

		public double MaxUpdateDuration => _maxUpdateDuration;

		public double AccumulatedUpdateDuration => _accumulatedUpdateDuration;

		public double AverageUpdateDuration =>
			_updateDurationsCount == 0 ? 0 : _accumulatedUpdateDuration / _updateDurationsCount;

		#endregion

		#region LateUpdate System Properties

		public double MinLateUpdateDuration => _minLateUpdateDuration;

		public double MaxLateUpdateDuration => _maxLateUpdateDuration;

		public double AccumulatedLateUpdateDuration => _accumulatedLateUpdateDuration;

		public double AverageLateUpdateDuration =>
			_lateUpdateDurationsCount == 0 ? 0 : _accumulatedLateUpdateDuration / _lateUpdateDurationsCount;

		#endregion

		#region Reactive System Properties

		public double MinReactiveDuration => _minReactiveDuration;

		public double MaxReactiveDuration => _maxReactiveDuration;

		public double AccumulatedReactiveDuration => _accumulatedReactiveDuration;

		public double AverageReactiveDuration =>
			_reactiveDurationsCount == 0 ? 0 : _accumulatedReactiveDuration / _reactiveDurationsCount;

		#endregion

		#region Cleanup System Properties

		public double CleanupDuration { get; set; }

		public double MinCleanupDuration => _minCleanupDuration;

		public double MaxCleanupDuration => _maxCleanupDuration;

		public double AccumulatedCleanupDuration => _accumulatedCleanupDuration;

		public double AverageCleanupDuration =>
			_cleanupDurationsCount == 0 ? 0 : _accumulatedCleanupDuration / _cleanupDurationsCount;

		#endregion

		#region Initialization and Teardown Properties

		public double InitializationDuration { get; set; }

		public double TeardownDuration { get; set; }

		#endregion

		public bool isActive;

		public SystemInfo parentSystemInfo;

		// Update
		private double _maxUpdateDuration;
		private double _minUpdateDuration;
		private double _accumulatedUpdateDuration;
		private int _updateDurationsCount;

		// Fixed Update
		private double _minFixedUpdateDuration;
		private double _maxFixedUpdateDuration;
		private double _accumulatedFixedUpdateDuration;
		private int _fixedUpdateDurationsCount;

		// Late Update
		private double _minLateUpdateDuration;
		private double _maxLateUpdateDuration;
		private double _accumulatedLateUpdateDuration;
		private int _lateUpdateDurationsCount;

		// Reactive
		private double _maxReactiveDuration;
		private double _minReactiveDuration;
		private double _accumulatedReactiveDuration;
		private int _reactiveDurationsCount;

		// Cleanup
		private int _cleanupDurationsCount;
		private double _maxCleanupDuration;
		private double _minCleanupDuration;
		private double _accumulatedCleanupDuration;

		private readonly SystemInterfaceFlags _interfaceFlags;

		public SystemInfo(ISystem system)
		{
			System = system;
			_interfaceFlags = GetInterfaceFlags(system);

			SystemName = system is DebugSystems debugSystem
				? debugSystem.Name
				: system.GetType().Name.RemoveSystemSuffix();

			isActive = true;
		}

		public void AddFixedUpdateDuration(double duration)
		{
			if (duration < _minFixedUpdateDuration || Math.Abs(_minFixedUpdateDuration) < 0.001)
			{
				_minFixedUpdateDuration = duration;
			}

			if (duration > _maxFixedUpdateDuration)
			{
				_maxFixedUpdateDuration = duration;
			}

			_accumulatedFixedUpdateDuration += duration;
			_fixedUpdateDurationsCount += 1;
		}

		public void AddUpdateDuration(double duration)
		{
			if (duration < _minUpdateDuration || Math.Abs(_minUpdateDuration) < 0.001)
			{
				_minUpdateDuration = duration;
			}

			if (duration > _maxUpdateDuration)
			{
				_maxUpdateDuration = duration;
			}

			_accumulatedUpdateDuration += duration;
			_updateDurationsCount += 1;
		}

		public void AddLateUpdateDuration(double duration)
		{
			if (duration < _minLateUpdateDuration || Math.Abs(_minLateUpdateDuration) < 0.001)
			{
				_minLateUpdateDuration = duration;
			}

			if (duration > _maxLateUpdateDuration)
			{
				_maxLateUpdateDuration = duration;
			}

			_accumulatedLateUpdateDuration += duration;
			_lateUpdateDurationsCount += 1;
		}

		public void AddReactiveDuration(double duration)
		{
			if (duration < _minReactiveDuration || Math.Abs(_minReactiveDuration) < 0.001)
			{
				_minReactiveDuration = duration;
			}

			if (duration > _maxReactiveDuration)
			{
				_maxReactiveDuration = duration;
			}

			_accumulatedReactiveDuration += duration;
			_reactiveDurationsCount += 1;
		}

		public void AddCleanupDuration(double cleanupDuration)
		{
			if (cleanupDuration < _minCleanupDuration || Math.Abs(_minCleanupDuration) < 0.001)
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

		/// <summary>
		/// Resets the duration times and counts for all relevant render frame system metrics.
		/// </summary>
		public void ResetFrameDurations()
		{
			// Fixed Update
			_accumulatedFixedUpdateDuration = 0;
			_fixedUpdateDurationsCount = 0;

			// Update
			_accumulatedUpdateDuration = 0;
			_updateDurationsCount = 0;

			// Late Update
			_accumulatedLateUpdateDuration = 0;
			_lateUpdateDurationsCount = 0;

			// Execute
			_accumulatedReactiveDuration = 0;
			_reactiveDurationsCount = 0;

			// Cleanup
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

			if (system is IFixedUpdateSystem)
			{
				flags |= SystemInterfaceFlags.FixedUpdateSystem;
			}

			if (system is IUpdateSystem)
			{
				flags |= SystemInterfaceFlags.UpdateSystem;
			}

			if (system is ILateUpdateSystem)
			{
				flags |= SystemInterfaceFlags.LateUpdateSystem;
			}

			if (system is IReactiveSystem)
			{
				flags |= SystemInterfaceFlags.ReactiveSystem;
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
