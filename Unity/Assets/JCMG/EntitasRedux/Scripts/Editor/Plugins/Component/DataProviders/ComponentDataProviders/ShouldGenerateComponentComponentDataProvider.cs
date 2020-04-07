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
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ShouldGenerateComponentComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			var shouldGenerateComponent = !type.ImplementsInterface<IComponent>();
			data.ShouldGenerateComponent(shouldGenerateComponent);
			if (shouldGenerateComponent)
			{
				data.SetObjectTypeName(type.ToCompilableString());
			}
		}
	}

	internal static class ShouldGenerateComponentComponentDataExtension
	{
		public const string COMPONENT_GENERATE_COMPONENT = "Component.Generate.Object";
		public const string COMPONENT_OBJECT_TYPE = "Component.ObjectTypeName";

		public static bool ShouldGenerateComponent(this ComponentData data)
		{
			return (bool)data[COMPONENT_GENERATE_COMPONENT];
		}

		public static void ShouldGenerateComponent(this ComponentData data, bool generate)
		{
			data[COMPONENT_GENERATE_COMPONENT] = generate;
		}

		public static string GetObjectTypeName(this ComponentData data)
		{
			return (string)data[COMPONENT_OBJECT_TYPE];
		}

		public static void SetObjectTypeName(this ComponentData data, string type)
		{
			data[COMPONENT_OBJECT_TYPE] = type;
		}
	}
}
