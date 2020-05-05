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
	internal sealed class FlagPrefixComponentDataProvider : IComponentDataProvider
	{
		private string GetFlagPrefix(Type type)
		{
			var attr = Attribute.GetCustomAttributes(type)
				.OfType<FlagPrefixAttribute>()
				.SingleOrDefault();

			return attr == null ? "is" : attr.prefix;
		}

		public void Provide(Type type, ComponentData data)
		{
			data.SetFlagPrefix(GetFlagPrefix(type));
		}
	}

	internal static class FlagPrefixComponentDataExtension
	{
		public const string COMPONENT_FLAG_PREFIX = "Component.FlagPrefix";

		public static string GetFlagPrefix(this ComponentData data)
		{
			return (string)data[COMPONENT_FLAG_PREFIX];
		}

		public static void SetFlagPrefix(this ComponentData data, string prefix)
		{
			data[COMPONENT_FLAG_PREFIX] = prefix;
		}
	}
}
