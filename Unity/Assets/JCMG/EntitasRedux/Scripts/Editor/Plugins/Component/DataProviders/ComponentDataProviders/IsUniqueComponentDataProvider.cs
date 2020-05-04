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

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class IsUniqueComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			var isUnique = Attribute
				.GetCustomAttributes(type)
				.OfType<UniqueAttribute>()
				.Any();

			data.IsUnique(isUnique);
		}
	}

	internal static class IsUniqueComponentDataExtension
	{
		public const string COMPONENT_IS_UNIQUE = "Component.Unique";

		public static bool IsUnique(this ComponentData data)
		{
			return (bool)data[COMPONENT_IS_UNIQUE];
		}

		public static void IsUnique(this ComponentData data, bool isUnique)
		{
			data[COMPONENT_IS_UNIQUE] = isUnique;
		}
	}
}
