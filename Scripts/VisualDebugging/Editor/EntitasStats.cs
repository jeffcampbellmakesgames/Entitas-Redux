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
using System.Collections.Generic;
using System.Linq;
using JCMG.Genesis.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	internal static class EntitasStats
	{
		[MenuItem("Tools/JCMG/EntitasRedux/Show Stats", false, 200)]
		public static void ShowStats()
		{
			var stats = string.Join(
				"\n",
				GetStats()
					.Select(kv => kv.Key + ": " + kv.Value)
					.ToArray());

			Debug.Log(stats);
			EditorUtility.DisplayDialog("EntitasRedux Stats", stats, "Close");
		}

		public static Dictionary<string, int> GetStats()
		{
			var types = ReflectionTools.GetAvailableAssemblies().SelectMany(x => x.GetTypes());

			var components = types
				.Where(type => type.ImplementsInterface<IComponent>())
				.ToArray();

			var systems = types
				.Where(IsSystem)
				.ToArray();

			var contexts = GetContexts(components);

			var stats = new Dictionary<string, int>
			{
				{
					"Total Components", components.Length
				},
				{
					"Systems", systems.Length
				}
			};

			foreach (var context in contexts)
			{
				stats.Add("Components in " + context.Key, context.Value);
			}

			return stats;
		}

		private static Dictionary<string, int> GetContexts(Type[] components)
		{
			return components.Aggregate(
				new Dictionary<string, int>(),
				(contexts, type) =>
				{
					var contextNames = GetContextNamesOrDefault(type);
					foreach (var contextName in contextNames)
					{
						if (!contexts.ContainsKey(contextName))
						{
							contexts.Add(contextName, 0);
						}

						contexts[contextName] += 1;
					}

					return contexts;
				});
		}


		private static string[] GetContextNames(Type type)
		{
			return Attribute
				.GetCustomAttributes(type)
				.OfType<ContextAttribute>()
				.Select(attr => attr.contextName)
				.ToArray();
		}

		private static string[] GetContextNamesOrDefault(Type type)
		{
			var contextNames = GetContextNames(type);
			if (contextNames.Length == 0)
			{
				contextNames = new[]
				{
					"Default"
				};
			}

			return contextNames;
		}

		private static bool IsSystem(Type type)
		{
			return type.ImplementsInterface<ISystem>() &&
			       type != typeof(ReactiveSystem<>) &&
			       type != typeof(MultiReactiveSystem<,>) &&
			       type != typeof(Systems) &&
			       type != typeof(DebugSystems) &&
			       type != typeof(JobSystem<>) &&
			       type.FullName != "Feature";
		}
	}
}
