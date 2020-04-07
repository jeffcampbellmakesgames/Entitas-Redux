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
using System.Linq;
using UnityEditor;
using UnityEngine;
using EditorGUILayoutTools = JCMG.EntitasRedux.Editor.EditorGUILayoutTools;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	[CustomEditor(typeof(DebugSystemsBehaviour))]
	internal sealed class DebugSystemsInspector : UnityEditor.Editor
	{
		private enum SortMethod
		{
			OrderOfOccurrence,

			Name,
			NameDescending,

			ExecutionTime,
			ExecutionTimeDescending
		}

		private int _lastRenderedFrameCount;
		private GUIContent _pauseButtonContent;

		private GUIContent _stepButtonContent;
		private Queue<float> _systemMonitorData;

		private Graph _systemsMonitor;
		private SortMethod _systemSortMethod;

		private float _threshold;
		private const int SYSTEM_MONITOR_DATA_LENGTH = 60;

		private static bool _showDetails;
		private static bool _showSystemsMonitor = true;
		private static bool _showSystemsList = true;

		private static bool _showInitializeSystems = true;
		private static bool _showExecuteSystems = true;
		private static bool _showCleanupSystems = true;
		private static bool _showTearDownSystems = true;
		private static bool _hideEmptySystems = true;
		private static string _systemNameSearchString = string.Empty;

		public override void OnInspectorGUI()
		{
			var debugSystemsBehaviour = (DebugSystemsBehaviour)target;
			var systems = debugSystemsBehaviour.Systems;

			EditorGUILayout.Space();
			DrawSystemsOverview(systems);

			EditorGUILayout.Space();
			DrawSystemsMonitor(systems);

			EditorGUILayout.Space();
			DrawSystemList(systems);

			EditorGUILayout.Space();

			EditorUtility.SetDirty(target);
		}

		private static void DrawSystemsOverview(DebugSystems systems)
		{
			_showDetails = Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("Details", _showDetails);
			if (_showDetails)
			{
				Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
				{
					EditorGUILayout.LabelField(systems.Name, EditorStyles.boldLabel);
					EditorGUILayout.LabelField("Initialize Systems", systems.TotalInitializeSystemsCount.ToString());
					EditorGUILayout.LabelField("Execute Systems", systems.TotalExecuteSystemsCount.ToString());
					EditorGUILayout.LabelField("Cleanup Systems", systems.TotalCleanupSystemsCount.ToString());
					EditorGUILayout.LabelField("TearDown Systems", systems.TotalTearDownSystemsCount.ToString());
					EditorGUILayout.LabelField("Total Systems", systems.TotalSystemsCount.ToString());
				}
				Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
			}
		}

		private void DrawSystemsMonitor(DebugSystems systems)
		{
			if (_systemsMonitor == null)
			{
				_systemsMonitor = new Graph(SYSTEM_MONITOR_DATA_LENGTH);
				_systemMonitorData = new Queue<float>(new float[SYSTEM_MONITOR_DATA_LENGTH]);
			}

			_showSystemsMonitor = Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("Performance", _showSystemsMonitor);
			if (_showSystemsMonitor)
			{
				Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.BeginVertical();
						{
							EditorGUILayout.LabelField("Execution duration", systems.ExecuteDuration.ToString());
							EditorGUILayout.LabelField("Cleanup duration", systems.CleanupDuration.ToString());
						}
						EditorGUILayout.EndVertical();

						if (_stepButtonContent == null)
						{
							_stepButtonContent = EditorGUIUtility.IconContent("StepButton On");
						}

						if (_pauseButtonContent == null)
						{
							_pauseButtonContent = EditorGUIUtility.IconContent("PauseButton On");
						}

						systems.paused = GUILayout.Toggle(systems.paused, _pauseButtonContent, "CommandLeft");

						if (GUILayout.Button(_stepButtonContent, "CommandRight"))
						{
							systems.paused = true;
							systems.StepExecute();
							systems.StepCleanup();
							AddDuration((float)systems.ExecuteDuration + (float)systems.CleanupDuration);
						}
					}
					EditorGUILayout.EndHorizontal();

					if (!EditorApplication.isPaused && !systems.paused)
					{
						AddDuration((float)systems.ExecuteDuration + (float)systems.CleanupDuration);
					}

					_systemsMonitor.Draw(_systemMonitorData.ToArray(), 80f);
				}
				Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
			}
		}

		private void DrawSystemList(DebugSystems systems)
		{
			_showSystemsList = Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("Systems", _showSystemsList);
			if (_showSystemsList)
			{
				Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
				{
					EditorGUILayout.BeginHorizontal();
					{
						DebugSystems.avgResetInterval = (AvgResetInterval)EditorGUILayout.EnumPopup(
							"Reset average duration Ø",
							DebugSystems.avgResetInterval);
						if (GUILayout.Button("Reset Ø now", EditorStyles.miniButton, GUILayout.Width(88)))
						{
							systems.ResetDurations();
						}
					}
					EditorGUILayout.EndHorizontal();

					_threshold = EditorGUILayout.Slider(
						"Threshold Ø ms",
						_threshold,
						0f,
						33f);

					_hideEmptySystems = EditorGUILayout.Toggle("Hide empty systems", _hideEmptySystems);
					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					{
						_systemSortMethod = (SortMethod)EditorGUILayout.EnumPopup(
							_systemSortMethod,
							EditorStyles.popup,
							GUILayout.Width(150));
						_systemNameSearchString = EditorGUILayoutTools.SearchTextField(_systemNameSearchString);
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					_showInitializeSystems = Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle(
						"Initialize Systems",
						_showInitializeSystems);
					if (_showInitializeSystems && ShouldShowSystems(systems, SystemInterfaceFlags.InitializeSystem))
					{
						Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
						{
							var systemsDrawn = DrawSystemInfos(systems, SystemInterfaceFlags.InitializeSystem);
							if (systemsDrawn == 0)
							{
								EditorGUILayout.LabelField(string.Empty);
							}
						}
						Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
					}

					_showExecuteSystems =
						Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("Execute Systems", _showExecuteSystems);
					if (_showExecuteSystems && ShouldShowSystems(systems, SystemInterfaceFlags.ExecuteSystem))
					{
						Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
						{
							var systemsDrawn = DrawSystemInfos(systems, SystemInterfaceFlags.ExecuteSystem);
							if (systemsDrawn == 0)
							{
								EditorGUILayout.LabelField(string.Empty);
							}
						}
						Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
					}

					_showCleanupSystems =
						Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("Cleanup Systems", _showCleanupSystems);
					if (_showCleanupSystems && ShouldShowSystems(systems, SystemInterfaceFlags.CleanupSystem))
					{
						Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
						{
							var systemsDrawn = DrawSystemInfos(systems, SystemInterfaceFlags.CleanupSystem);
							if (systemsDrawn == 0)
							{
								EditorGUILayout.LabelField(string.Empty);
							}
						}
						Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
					}

					_showTearDownSystems =
						Genesis.Editor.EditorGUILayoutTools.DrawSectionHeaderToggle("TearDown Systems", _showTearDownSystems);
					if (_showTearDownSystems && ShouldShowSystems(systems, SystemInterfaceFlags.TearDownSystem))
					{
						Genesis.Editor.EditorGUILayoutTools.BeginSectionContent();
						{
							var systemsDrawn = DrawSystemInfos(systems, SystemInterfaceFlags.TearDownSystem);
							if (systemsDrawn == 0)
							{
								EditorGUILayout.LabelField(string.Empty);
							}
						}
						Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
					}
				}
				Genesis.Editor.EditorGUILayoutTools.EndSectionContent();
			}
		}

		private int DrawSystemInfos(DebugSystems systems, SystemInterfaceFlags type)
		{
			SystemInfo[] systemInfos = null;

			switch (type)
			{
				case SystemInterfaceFlags.InitializeSystem:
					systemInfos = systems.InitializeSystemInfos
						.Where(systemInfo => systemInfo.InitializationDuration >= _threshold)
						.ToArray();
					break;
				case SystemInterfaceFlags.ExecuteSystem:
					systemInfos = systems.ExecuteSystemInfos
						.Where(systemInfo => systemInfo.AverageExecutionDuration >= _threshold)
						.ToArray();
					break;
				case SystemInterfaceFlags.CleanupSystem:
					systemInfos = systems.CleanupSystemInfos
						.Where(systemInfo => systemInfo.CleanupDuration >= _threshold)
						.ToArray();
					break;
				case SystemInterfaceFlags.TearDownSystem:
					systemInfos = systems.TearDownSystemInfos
						.Where(systemInfo => systemInfo.TeardownDuration >= _threshold)
						.ToArray();
					break;
			}

			systemInfos = GetSortedSystemInfos(systemInfos, _systemSortMethod);

			var systemsDrawn = 0;
			foreach (var systemInfo in systemInfos)
			{
				if (systemInfo.System is DebugSystems debugSystems)
				{
					if (!ShouldShowSystems(debugSystems, type))
					{
						continue;
					}
				}

				if (EditorGUILayoutTools.MatchesSearchString(systemInfo.SystemName.ToLower(), _systemNameSearchString.ToLower()))
				{
					EditorGUILayout.BeginHorizontal();
					{
						var indent = EditorGUI.indentLevel;
						EditorGUI.indentLevel = 0;

						var wasActive = systemInfo.isActive;
						if (systemInfo.AreAllParentsActive)
						{
							systemInfo.isActive = EditorGUILayout.Toggle(systemInfo.isActive, GUILayout.Width(20));
						}
						else
						{
							EditorGUI.BeginDisabledGroup(true);
							{
								EditorGUILayout.Toggle(false, GUILayout.Width(20));
							}
						}

						EditorGUI.EndDisabledGroup();

						EditorGUI.indentLevel = indent;

						if (systemInfo.isActive != wasActive)
						{
							if (systemInfo.System is IReactiveSystem reactiveSystem)
							{
								if (systemInfo.isActive)
								{
									reactiveSystem.Activate();
								}
								else
								{
									reactiveSystem.Deactivate();
								}
							}
						}

						switch (type)
						{
							case SystemInterfaceFlags.InitializeSystem:
								EditorGUILayout.LabelField(
									systemInfo.SystemName,
									systemInfo.InitializationDuration.ToString(),
									GetSystemStyle(systemInfo, SystemInterfaceFlags.InitializeSystem));
								break;
							case SystemInterfaceFlags.ExecuteSystem:
								var avgE = string.Format("Ø {0:00.000}", systemInfo.AverageExecutionDuration).PadRight(12);
								var minE = string.Format("▼ {0:00.000}", systemInfo.MinExecutionDuration).PadRight(12);
								var maxE = string.Format("▲ {0:00.000}", systemInfo.MaxExecutionDuration);
								EditorGUILayout.LabelField(
									systemInfo.SystemName,
									avgE + minE + maxE,
									GetSystemStyle(systemInfo, SystemInterfaceFlags.ExecuteSystem));
								break;
							case SystemInterfaceFlags.CleanupSystem:
								var avgC = string.Format("Ø {0:00.000}", systemInfo.AverageCleanupDuration).PadRight(12);
								var minC = string.Format("▼ {0:00.000}", systemInfo.MinCleanupDuration).PadRight(12);
								var maxC = string.Format("▲ {0:00.000}", systemInfo.MaxCleanupDuration);
								EditorGUILayout.LabelField(
									systemInfo.SystemName,
									avgC + minC + maxC,
									GetSystemStyle(systemInfo, SystemInterfaceFlags.CleanupSystem));
								break;
							case SystemInterfaceFlags.TearDownSystem:
								EditorGUILayout.LabelField(
									systemInfo.SystemName,
									systemInfo.TeardownDuration.ToString(),
									GetSystemStyle(systemInfo, SystemInterfaceFlags.TearDownSystem));
								break;
						}
					}
					EditorGUILayout.EndHorizontal();

					systemsDrawn += 1;
				}

				if (systemInfo.System is DebugSystems debugSystem)
				{
					var indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel += 1;
					systemsDrawn += DrawSystemInfos(debugSystem, type);
					EditorGUI.indentLevel = indent;
				}
			}

			return systemsDrawn;
		}

		private static SystemInfo[] GetSortedSystemInfos(SystemInfo[] systemInfos, SortMethod sortMethod)
		{
			if (sortMethod == SortMethod.Name)
			{
				return systemInfos
					.OrderBy(systemInfo => systemInfo.SystemName)
					.ToArray();
			}

			if (sortMethod == SortMethod.NameDescending)
			{
				return systemInfos
					.OrderByDescending(systemInfo => systemInfo.SystemName)
					.ToArray();
			}

			if (sortMethod == SortMethod.ExecutionTime)
			{
				return systemInfos
					.OrderBy(systemInfo => systemInfo.AverageExecutionDuration)
					.ToArray();
			}

			if (sortMethod == SortMethod.ExecutionTimeDescending)
			{
				return systemInfos
					.OrderByDescending(systemInfo => systemInfo.AverageExecutionDuration)
					.ToArray();
			}

			return systemInfos;
		}

		private static bool ShouldShowSystems(DebugSystems systems, SystemInterfaceFlags type)
		{
			if (!_hideEmptySystems)
			{
				return true;
			}

			switch (type)
			{
				case SystemInterfaceFlags.InitializeSystem:
					return systems.TotalInitializeSystemsCount > 0;
				case SystemInterfaceFlags.ExecuteSystem:
					return systems.TotalExecuteSystemsCount > 0;
				case SystemInterfaceFlags.CleanupSystem:
					return systems.TotalCleanupSystemsCount > 0;
				case SystemInterfaceFlags.TearDownSystem:
					return systems.TotalTearDownSystemsCount > 0;
				default:
					return true;
			}
		}

		private GUIStyle GetSystemStyle(SystemInfo systemInfo, SystemInterfaceFlags systemFlag)
		{
			var style = new GUIStyle(GUI.skin.label);
			var color = systemInfo.IsReactiveSystems && EditorGUIUtility.isProSkin
				? Color.white
				: style.normal.textColor;

			if (systemFlag == SystemInterfaceFlags.ExecuteSystem &&
			    systemInfo.AverageExecutionDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
			{
				color = Color.red;
			}

			if (systemFlag == SystemInterfaceFlags.CleanupSystem &&
			    systemInfo.AverageCleanupDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
			{
				color = Color.red;
			}

			style.normal.textColor = color;

			return style;
		}

		private void AddDuration(float duration)
		{
			// OnInspectorGUI is called twice per frame - only add duration once
			if (Time.renderedFrameCount != _lastRenderedFrameCount)
			{
				_lastRenderedFrameCount = Time.renderedFrameCount;

				if (_systemMonitorData.Count >= SYSTEM_MONITOR_DATA_LENGTH)
				{
					_systemMonitorData.Dequeue();
				}

				_systemMonitorData.Enqueue(duration);
			}
		}
	}
}
