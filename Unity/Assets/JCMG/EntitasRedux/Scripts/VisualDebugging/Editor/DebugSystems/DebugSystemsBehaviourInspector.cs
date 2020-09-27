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

using JCMG.EntitasRedux.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	[CustomEditor(typeof(DebugSystemsBehaviour))]
	internal sealed class DebugSystemsBehaviourInspector : UnityEditor.Editor
	{
		private enum SortMethod
		{
			Name,
			NameDescending,
			ExecutionTime,
			ExecutionTimeDescending
		}

		private int _lastRenderedFrameCount;

		private Queue<float> _fixedUpdateSystemMonitorData;
		private Queue<float> _updateSystemMonitorData;
		private Queue<float> _lateUpdateSystemMonitorData;

		private Graph _systemsMonitor;
		private SortMethod _systemSortMethod;

		private float _threshold;

		private static bool _showDetails;
		private static bool _showSystemsMonitor = true;
		private static bool _showSystemsList = true;

		private static bool _showInitializeSystems = true;
		private static bool _showFixedUpdateSystems = true;
		private static bool _showUpdateSystems = true;
		private static bool _showLateUpdateSystems = true;
		private static bool _showReactiveSystems = true;
		private static bool _showCleanupSystems = true;
		private static bool _showTearDownSystems = true;
		private static bool _hideEmptySystems = true;
		private static string _systemNameSearchString = string.Empty;

		private static readonly StringBuilder STRING_BUILDER;
		private static readonly GUILayoutOption[] LEGEND_LABEL_OPTIONS;

		private static GUILayoutOption[] _leftLabelGUILayoutOptions;

		// Data Capture
		private const int SYSTEM_MONITOR_DATA_LENGTH = 60;

		// Legend
		private const int BOX_HEIGHT_WIDTH = 16;

		// Top-Level Section Titles
		private const string DETAILS_TITLE = "Overview";
		private const string SYSTEMS_TITLE = "Systems";
		private const string PERFORMANCE_TITLE = "Performance";

		// Monitor UI
		private const string AVERAGE_PERFORMANCE_TITLE = "Average System Performance";
		private const string GRAPH_LEGEND = "Legend";
		private const string FIXED_UPDATE_LEGEND = "Fixed Update";
		private const string UPDATE_LEGEND = "Update";
		private const string LATE_UPDATE_LEGEND = "Late Update";

		// System Section Titles
		private const string INITIALIZE_SYSTEMS_TITLE = "Initialize Systems";
		private const string FIXED_UPDATE_SYSTEMS_TITLE = "Fixed Update Systems";
		private const string UPDATE_SYSTEMS_TITLE = "Update Systems";
		private const string LATE_UPDATE_SYSTEMS_TITLE = "Late Update Systems";
		private const string REACTIVE_SYSTEMS_TITLE = "Reactive Systems";
		private const string CLEANUP_SYSTEMS_TITLE = "Cleanup Systems";
		private const string TEARDOWN_SYSTEMS_TITLE = "TearDown Systems";

		// Count Labels
		private const string INITIALIZE_SYSTEMS_COUNT_LABEL = "Initialize Systems";
		private const string FIXED_UPDATE_SYSTEMS_COUNT_LABEL = "Fixed Update Systems";
		private const string UPDATE_SYSTEMS_COUNT_LABEL = "Update Systems";
		private const string LATE_UPDATE_SYSTEMS_COUNT_LABEL = "Late Update Systems";
		private const string REACTIVE_SYSTEMS_COUNT_LABEL = "Reactive Systems";
		private const string CLEANUP_SYSTEMS_COUNT_LABEL = "Cleanup Systems";
		private const string TEARDOWN_SYSTEMS_COUNT_LABEL = "TearDown Systems";
		private const string TOTAL_SYSTEMS_COUNT_LABEL = "Total Systems";

		// Duration Labels
		private const string DURATION_TIME_FORMAT = "{0,10:###0.00}";
		private const string FIXED_UPDATE_DURATION_LABEL = "Fixed Update Duration";
		private const string UPDATE_DURATION_LABEL = "Update Duration";
		private const string LATE_UPDATE_DURATION_LABEL = "Late Update Duration";
		private const string REACTIVE_DURATION_LABEL = "Reactive Duration";
		private const string CLEANUP_DURATION_LABEL = "Cleanup Duration";

		// Profiling Controls
		private const string HIDE_EMPTY_SYSTEMS_LABEL = "Hide Empty Systems";
		private const string THRESHOLD_SLIDER_LABEL = "Threshold Average in ms";
		private const string RESET_AVERAGE_DURATION_LABEL = "Reset Average Interval";
		private const string RESET_AVERAGE_NOW_LABEL = "Reset Average Now";

		// Help Boxes
		private const string PERFORMANCE_DESCRIPTION = "All performance statistics are meausured in milliseconds (ms).";

		private const string SYSTEMS_DESCRIPTION = "These are all systems running as a part of this Feature. Toggles " +
												   "are available to selectively filter which systems are running.";

		static DebugSystemsBehaviourInspector()
		{
			LEGEND_LABEL_OPTIONS = new[]
			{
				GUILayout.Width(BOX_HEIGHT_WIDTH),
				GUILayout.Height(BOX_HEIGHT_WIDTH),
			};
			STRING_BUILDER = new StringBuilder(200);
		}

		private void OnEnable()
		{
			if (_leftLabelGUILayoutOptions == null)
			{
				_leftLabelGUILayoutOptions = new []
				{
					GUILayout.Width(150f)
				};
			}
		}

		public override void OnInspectorGUI()
		{
			if (_systemsMonitor == null)
			{
				_systemsMonitor = new Graph(SYSTEM_MONITOR_DATA_LENGTH);

				_fixedUpdateSystemMonitorData = new Queue<float>(new float[SYSTEM_MONITOR_DATA_LENGTH]);
				_updateSystemMonitorData = new Queue<float>(new float[SYSTEM_MONITOR_DATA_LENGTH]);
				_lateUpdateSystemMonitorData = new Queue<float>(new float[SYSTEM_MONITOR_DATA_LENGTH]);
			}

			var debugSystemsBehaviour = (DebugSystemsBehaviour)target;
			var systems = debugSystemsBehaviour.Systems;

			EditorGUILayout.HelpBox(PERFORMANCE_DESCRIPTION, MessageType.Info);

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
			_showDetails = EditorGUILayoutTools.DrawSectionHeaderToggle(DETAILS_TITLE, _showDetails);
			if (_showDetails)
			{
				using (new EditorGUILayout.VerticalScope(EntitasReduxStyles.SectionContent))
				{
					EditorGUILayout.LabelField(INITIALIZE_SYSTEMS_COUNT_LABEL, systems.TotalInitializeSystemsCount.ToString());
					EditorGUILayout.LabelField(FIXED_UPDATE_SYSTEMS_COUNT_LABEL, systems.TotalFixedUpdateSystemsCount.ToString());
					EditorGUILayout.LabelField(UPDATE_SYSTEMS_COUNT_LABEL, systems.TotalUpdateSystemsCount.ToString());
					EditorGUILayout.LabelField(LATE_UPDATE_SYSTEMS_COUNT_LABEL, systems.TotalLateUpdateSystemsCount.ToString());
					EditorGUILayout.LabelField(REACTIVE_SYSTEMS_COUNT_LABEL, systems.TotalReactiveSystemsCount.ToString());
					EditorGUILayout.LabelField(CLEANUP_SYSTEMS_COUNT_LABEL, systems.TotalCleanupSystemsCount.ToString());
					EditorGUILayout.LabelField(TEARDOWN_SYSTEMS_COUNT_LABEL, systems.TotalTearDownSystemsCount.ToString());
					EditorGUILayout.LabelField(TOTAL_SYSTEMS_COUNT_LABEL, systems.TotalSystemsCount.ToString());
				}
			}
		}

		private void DrawSystemsMonitor(DebugSystems systems)
		{
			_showSystemsMonitor = EditorGUILayoutTools.DrawSectionHeaderToggle(PERFORMANCE_TITLE, _showSystemsMonitor);
			if (_showSystemsMonitor)
			{
				using (new EditorGUILayout.VerticalScope(VisualDebugStyles.SectionContent))
				{
					// Draw Average Performance Stats
					using (new EditorGUILayout.VerticalScope())
					{
						EditorGUILayout.LabelField(AVERAGE_PERFORMANCE_TITLE, EditorStyles.boldLabel);

						DrawDurationLabel(FIXED_UPDATE_DURATION_LABEL, systems.AverageFixedUpdateDuration);
						DrawDurationLabel(UPDATE_DURATION_LABEL, systems.AverageUpdateDuration);
						DrawDurationLabel(LATE_UPDATE_DURATION_LABEL, systems.AverageLateUpdateDuration);
						DrawDurationLabel(REACTIVE_DURATION_LABEL, systems.AverageReactiveDuration);
						DrawDurationLabel(CLEANUP_DURATION_LABEL, systems.AverageCleanupDuration);
					}

					// Draw Legend
					using (var legendScope = new EditorGUILayout.VerticalScope())
					{
						using (new EditorGUILayout.HorizontalScope())
						{
							GUILayout.FlexibleSpace();
							EditorGUILayout.LabelField(GRAPH_LEGEND, EditorStyles.boldLabel, GUILayout.Width(60f));
							GUILayout.FlexibleSpace();
						}

						GUILayout.Space(5);

						using (new EditorGUILayout.HorizontalScope())
						{
							DrawLegendLabel(FIXED_UPDATE_LEGEND, VisualDebuggingPreferences.FixedUpdateColor);
							GUILayout.FlexibleSpace();

							DrawLegendLabel(UPDATE_LEGEND, VisualDebuggingPreferences.UpdateColor);
							GUILayout.FlexibleSpace();

							DrawLegendLabel(LATE_UPDATE_LEGEND, VisualDebuggingPreferences.LateUpdateColor);
						}
					}

					EditorGUILayout.Space(5);

					// Update Graph
					if (!EditorApplication.isPaused)
					{
						AddSystemDurations(systems);
					}

					// Draw Graph
					const float GRAPH_HEIGHT = 100f;
					_systemsMonitor.Draw(
						new[]
						{
							_fixedUpdateSystemMonitorData.ToArray(),
							_updateSystemMonitorData.ToArray(),
							_lateUpdateSystemMonitorData.ToArray()
						},
						Screen.width,
						GRAPH_HEIGHT,
						new[]
						{
							VisualDebuggingPreferences.FixedUpdateColor,
							VisualDebuggingPreferences.UpdateColor,
							VisualDebuggingPreferences.LateUpdateColor
						});
				}
			}
		}

		private void DrawLegendLabel(string label, Color color)
		{
			using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
			{
				var newGUIContent = new GUIContent(label);
				var size = EditorStyles.label.CalcSize(newGUIContent);
				EditorGUILayout.LabelField(newGUIContent, GUILayout.Width(size.x));

				var fixedUpdateLegendRect = GUILayoutUtility.GetRect(
					BOX_HEIGHT_WIDTH,
					BOX_HEIGHT_WIDTH,
					BOX_HEIGHT_WIDTH,
					BOX_HEIGHT_WIDTH,
					LEGEND_LABEL_OPTIONS);

				EditorGUILayoutTools.DrawRectWithBorder(
					fixedUpdateLegendRect,
					2f,
					color,
					Color.black);

				GUILayout.FlexibleSpace();
			}
		}

		private void DrawDurationLabel(string label, double duration)
		{
			using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
			{
				var size = EditorStyles.label.CalcSize(new GUIContent(label));
				EditorGUILayout.LabelField(label, _leftLabelGUILayoutOptions);

				EditorGUILayout.LabelField(
					string.Format(DURATION_TIME_FORMAT, duration),
					//VisualDebugStyles.RightAlignGUIStyle,
					GUILayout.MaxWidth(60f));
			}
		}

		private void DrawSystemList(DebugSystems systems)
		{
			_showSystemsList = EditorGUILayoutTools.DrawSectionHeaderToggle(SYSTEMS_TITLE, _showSystemsList);
			if (_showSystemsList)
			{
				using (new EditorGUILayout.VerticalScope(EntitasReduxStyles.SectionContent))
				{
					EditorGUILayout.HelpBox(SYSTEMS_DESCRIPTION, MessageType.Info);

					using (new EditorGUILayout.HorizontalScope())
					{
						DebugSystems.avgResetInterval = (AvgResetInterval)EditorGUILayout.EnumPopup(
							RESET_AVERAGE_DURATION_LABEL,
							DebugSystems.avgResetInterval);

						if (GUILayout.Button(RESET_AVERAGE_NOW_LABEL, EditorStyles.miniButton, GUILayout.Width(150f)))
						{
							systems.ResetDurations();
						}
					}

					_threshold = EditorGUILayout.Slider(
						THRESHOLD_SLIDER_LABEL,
						_threshold,
						0f,
						33f);

					_hideEmptySystems = EditorGUILayout.Toggle(HIDE_EMPTY_SYSTEMS_LABEL, _hideEmptySystems);
					EditorGUILayout.Space();

					using (new EditorGUILayout.HorizontalScope())
					{
						_systemSortMethod = (SortMethod)EditorGUILayout.EnumPopup(
							_systemSortMethod,
							EditorStyles.popup,
							GUILayout.Width(150));
						_systemNameSearchString = EditorGUILayoutTools.SearchTextField(_systemNameSearchString);
					}

					EditorGUILayout.Space();

					DrawSystemSection(INITIALIZE_SYSTEMS_TITLE, ref _showInitializeSystems, systems, SystemInterfaceFlags.InitializeSystem);
					DrawSystemSection(FIXED_UPDATE_SYSTEMS_TITLE, ref _showFixedUpdateSystems, systems, SystemInterfaceFlags.FixedUpdateSystem);
					DrawSystemSection(UPDATE_SYSTEMS_TITLE, ref _showUpdateSystems, systems, SystemInterfaceFlags.UpdateSystem);
					DrawSystemSection(LATE_UPDATE_SYSTEMS_TITLE, ref _showLateUpdateSystems, systems, SystemInterfaceFlags.LateUpdateSystem);
					DrawSystemSection(REACTIVE_SYSTEMS_TITLE, ref _showReactiveSystems, systems, SystemInterfaceFlags.ReactiveSystem);
					DrawSystemSection(CLEANUP_SYSTEMS_TITLE, ref _showCleanupSystems, systems, SystemInterfaceFlags.CleanupSystem);
					DrawSystemSection(TEARDOWN_SYSTEMS_TITLE, ref _showTearDownSystems, systems, SystemInterfaceFlags.TearDownSystem);
				}
			}
		}

		private void DrawSystemSection(string header,
									   ref bool showSystems,
									   DebugSystems systems,
									   SystemInterfaceFlags systemInterfaceFlags)
		{
			showSystems = EditorGUILayoutTools.DrawSectionHeaderToggle(header, showSystems);
			if (showSystems && ShouldShowSystems(systems, systemInterfaceFlags))
			{
				using (new EditorGUILayout.VerticalScope(EntitasReduxStyles.SectionContent))
				{
					var systemsDrawn = DrawSystemInfos(systems, systemInterfaceFlags);
					if (systemsDrawn == 0)
					{
						EditorGUILayout.LabelField(string.Empty);
					}
				}
			}
		}

		private int DrawSystemInfos(DebugSystems systems, SystemInterfaceFlags type)
		{
			IEnumerable<SystemInfo> systemInfos = null;

			switch (type)
			{
				case SystemInterfaceFlags.InitializeSystem:
					systemInfos = systems.InitializeSystemInfos
						.Where(systemInfo => systemInfo.InitializationDuration >= _threshold);
					break;
				case SystemInterfaceFlags.FixedUpdateSystem:
					systemInfos = systems.FixedUpdateSystemInfos
						.Where(systemInfo => systemInfo.AverageFixedUpdateDuration >= _threshold);
					break;
				case SystemInterfaceFlags.UpdateSystem:
					systemInfos = systems.UpdateSystemInfos
						.Where(systemInfo => systemInfo.AverageUpdateDuration >= _threshold);
					break;
				case SystemInterfaceFlags.LateUpdateSystem:
					systemInfos = systems.LateUpdateSystemInfos
						.Where(systemInfo => systemInfo.AverageLateUpdateDuration >= _threshold);
					break;
				case SystemInterfaceFlags.ReactiveSystem:
					systemInfos = systems.ReactiveSystemInfos
						.Where(systemInfo => systemInfo.AverageReactiveDuration >= _threshold);
					break;
				case SystemInterfaceFlags.CleanupSystem:
					systemInfos = systems.CleanupSystemInfos
						.Where(systemInfo => systemInfo.CleanupDuration >= _threshold);
					break;
				case SystemInterfaceFlags.TearDownSystem:
					systemInfos = systems.TearDownSystemInfos
						.Where(systemInfo => systemInfo.TeardownDuration >= _threshold);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
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
					using (new EditorGUILayout.HorizontalScope())
					{
						var indent = EditorGUI.indentLevel;
						EditorGUI.indentLevel = 0;

						var wasActive = systemInfo.isActive;
						var areParentsActive = systemInfo.AreAllParentsActive;
						if (areParentsActive)
						{
							systemInfo.isActive = EditorGUILayout.Toggle(systemInfo.isActive, GUILayout.Width(20));
						}
						else
						{
							using (new EditorGUI.DisabledScope(true))
							{
								EditorGUILayout.Toggle(false, GUILayout.Width(20));
							}
						}

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

						using (new EditorGUI.DisabledScope(!systemInfo.isActive || !areParentsActive))
						{
							var guiStyle = GetSystemStyle(systemInfo, type);

							switch (type)
							{
								case SystemInterfaceFlags.InitializeSystem:
									DrawSimpleSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.InitializationDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.FixedUpdateSystem:
									DrawDetailedSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.AverageFixedUpdateDuration,
										systemInfo.MinFixedUpdateDuration,
										systemInfo.MaxFixedUpdateDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.UpdateSystem:
									DrawDetailedSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.AverageUpdateDuration,
										systemInfo.MinUpdateDuration,
										systemInfo.MaxUpdateDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.LateUpdateSystem:
									DrawDetailedSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.AverageLateUpdateDuration,
										systemInfo.MinLateUpdateDuration,
										systemInfo.MaxLateUpdateDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.ReactiveSystem:
									DrawDetailedSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.AverageReactiveDuration,
										systemInfo.MinReactiveDuration,
										systemInfo.MaxReactiveDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.CleanupSystem:
									DrawDetailedSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.AverageCleanupDuration,
										systemInfo.MinCleanupDuration,
										systemInfo.MaxCleanupDuration,
										guiStyle);
									break;
								case SystemInterfaceFlags.TearDownSystem:
									DrawSimpleSystemInfoPerformance(
										systemInfo.SystemName,
										systemInfo.TeardownDuration,
										guiStyle);
									break;
								default:
									throw new ArgumentOutOfRangeException(nameof(type), type, null);
							}
						}
					}

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

		private void DrawSimpleSystemInfoPerformance(
			string systemName,
			double duration,
			GUIStyle style)
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(systemName, style);
				EditorGUILayout.LabelField(string.Format(DURATION_TIME_FORMAT, duration), style);
				GUILayout.FlexibleSpace();
			}
		}

		private void DrawDetailedSystemInfoPerformance(
			string systemName,
			double averageDuration,
			double minDuration,
			double maxDuration,
			GUIStyle style)
		{
			const string AVERAGE_FORMAT = "Avg {0:00.000}";
			const string MIN_FORMAT = "▼ {0:00.000}";
			const string MAX_FORMAT = "▲ {0:00.000}";

			STRING_BUILDER.Clear();
			STRING_BUILDER.Append(string.Format(AVERAGE_FORMAT, averageDuration).PadRight(14));
			STRING_BUILDER.Append(string.Format(MIN_FORMAT, minDuration).PadRight(12));
			STRING_BUILDER.Append(string.Format(MAX_FORMAT, maxDuration));

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(systemName, STRING_BUILDER.ToString(), style);
			}
		}

		private static IEnumerable<SystemInfo> GetSortedSystemInfos(IEnumerable<SystemInfo> systemInfos, SortMethod sortMethod)
		{
			switch (sortMethod)
			{
				case SortMethod.Name:
					return systemInfos.OrderBy(systemInfo => systemInfo.SystemName);
				case SortMethod.NameDescending:
					return systemInfos.OrderByDescending(systemInfo => systemInfo.SystemName);
				case SortMethod.ExecutionTime:
					return systemInfos.OrderBy(systemInfo => systemInfo.AverageUpdateDuration);
				case SortMethod.ExecutionTimeDescending:
					return systemInfos.OrderByDescending(systemInfo => systemInfo.AverageUpdateDuration);
				default:
					return systemInfos;
			}
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
				case SystemInterfaceFlags.FixedUpdateSystem:
					return systems.TotalFixedUpdateSystemsCount > 0;
				case SystemInterfaceFlags.UpdateSystem:
					return systems.TotalUpdateSystemsCount > 0;
				case SystemInterfaceFlags.LateUpdateSystem:
					return systems.TotalLateUpdateSystemsCount > 0;
				case SystemInterfaceFlags.ReactiveSystem:
					return systems.TotalReactiveSystemsCount > 0;
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

			if (systemFlag == SystemInterfaceFlags.FixedUpdateSystem &&
				systemInfo.AverageFixedUpdateDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
			{
				color = Color.red;
			}

			if (systemFlag == SystemInterfaceFlags.UpdateSystem &&
				systemInfo.AverageUpdateDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
			{
				color = Color.red;
			}

			if (systemFlag == SystemInterfaceFlags.LateUpdateSystem &&
				systemInfo.AverageLateUpdateDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
			{
				color = Color.red;
			}

			if (systemFlag == SystemInterfaceFlags.ReactiveSystem &&
				systemInfo.AverageReactiveDuration >= VisualDebuggingPreferences.SystemWarningThreshold)
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

		private void AddSystemDurations(DebugSystems systems)
		{
			// OnInspectorGUI is called twice per frame - only add duration once
			if (Time.renderedFrameCount != _lastRenderedFrameCount)
			{
				_lastRenderedFrameCount = Time.renderedFrameCount;

				// Execute System
				if (_updateSystemMonitorData.Count >= SYSTEM_MONITOR_DATA_LENGTH)
				{
					_updateSystemMonitorData.Dequeue();
				}

				_updateSystemMonitorData.Enqueue((float)systems.UpdateDuration);

				// Fixed Update System
				if (_fixedUpdateSystemMonitorData.Count >= SYSTEM_MONITOR_DATA_LENGTH)
				{
					_fixedUpdateSystemMonitorData.Dequeue();
				}

				_fixedUpdateSystemMonitorData.Enqueue((float)systems.FixedUpdateDuration);

				// Late Update System
				if (_lateUpdateSystemMonitorData.Count >= SYSTEM_MONITOR_DATA_LENGTH)
				{
					_lateUpdateSystemMonitorData.Dequeue();
				}

				_lateUpdateSystemMonitorData.Enqueue((float)systems.LateUpdateDuration + +(float)systems.CleanupDuration);
			}
		}
	}
}
