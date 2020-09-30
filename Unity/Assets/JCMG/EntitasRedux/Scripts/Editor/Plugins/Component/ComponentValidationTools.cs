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
using UnityEngine;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	/// <summary>
	/// Helper methods for validating <see cref="ComponentData"/> for code-generation.
	/// </summary>
	public static class ComponentValidationTools
	{
		/// <summary>
		/// Validates the contents of <paramref name="componentData"/> collection for code-generation.
		/// </summary>
		/// <param name="componentData"></param>
		public static void ValidateComponentData(ComponentData[] componentData)
		{
			// Validate there are not any duplicate components by name per context
			var foundDuplicateComponents = false;
			var contextNames = componentData.SelectMany(x => x.GetContextNames()).Distinct();
			foreach (var contextName in contextNames)
			{
				var dupesByNameGroup = componentData
					.Where(x => x.GetContextNames().Contains(contextName))
					.SelectMany(a => a.GetComponentNames())
					.GroupBy(y => y)
					.Where(z => z.Count() > 1)
					.ToArray();

				if (dupesByNameGroup.Any())
				{
					foundDuplicateComponents = true;

					const string ERROR_FORMAT = "[EntitasRedux] [{0}] for Context [{1}] has {2} separate Type instances " +
												"with the same name. Please ensure all components belonging to a single " +
												"context have unique names.";

					foreach (var dupeComponentGroup in dupesByNameGroup)
					{
						var firstComponent = dupeComponentGroup.First();
						Debug.LogErrorFormat(
							ERROR_FORMAT,
							firstComponent,
							contextName,
							dupeComponentGroup.Count());
					}
				}
			}

			if (foundDuplicateComponents)
			{
				throw new DuplicateComponentNameException();
			}
		}
	}
}
