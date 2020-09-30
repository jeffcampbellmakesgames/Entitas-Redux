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
using System.Linq;
using NUnit.Framework;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	/// <summary>
	/// A data provider that associates cleanup information with a component that has the <see cref="CleanupAttribute"/>.
	/// </summary>
	internal sealed class CleanupComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			var attrs = Attribute.GetCustomAttributes(type)
				.OfType<CleanupAttribute>()
				.ToArray();

			if (attrs.Length > 0)
			{
				var cleanupData = attrs
					.Select(attr => attr.CleanupMode)
					.Distinct()
					.ToArray();

				data.SetCleanupData(cleanupData);
			}
		}
	}

	internal static class CleanupComponentDataExtension
	{
		private const string CLEANUP_DATA_KEY = "Component.Cleanup.Data";

		public static void SetCleanupData(this ComponentData data, CleanupMode[] cleanupData)
		{
			data[CLEANUP_DATA_KEY] = cleanupData;
		}

		public static bool HasCleanupData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY);
		}

		public static bool HasCleanupRemoveComponentData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY) &&
				   ((CleanupMode[])data[CLEANUP_DATA_KEY]).Any(x => x == CleanupMode.RemoveComponent);
		}

		public static bool HasCleanupDestroyEntityData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY) &&
				   ((CleanupMode[])data[CLEANUP_DATA_KEY]).Any(x => x == CleanupMode.DestroyEntity);
		}

		public static string GetCleanupRemoveSystemClassName(this ComponentData data, string contextName)
		{
			Assert.IsTrue(data.HasCleanupRemoveComponentData());

			const string CLASS_NAME = "Remove${componentName}From${ContextName}EntitiesSystem";

			return CLASS_NAME.Replace(contextName).Replace(data, contextName);
		}

		public static string GetCleanupDestroySystemClassName(this ComponentData data, string contextName)
		{
			Assert.IsTrue(data.HasCleanupDestroyEntityData());

			const string CLASS_NAME = "Destroy${ContextName}EntitiesWith${componentName}System";

			return CLASS_NAME.Replace(contextName).Replace(data, contextName);
		}
	}
}
