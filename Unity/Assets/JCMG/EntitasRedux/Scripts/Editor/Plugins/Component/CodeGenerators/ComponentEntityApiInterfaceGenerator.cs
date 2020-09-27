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

using System.IO;
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ComponentEntityApiInterfaceGenerator : AbstractGenerator
	{
		public override string Name
		{
			get { return "Component (Entity API Interface)"; }
		}

		private const string STANDARD_TEMPLATE =
@"public partial interface I${ComponentName}Entity
{
	${ComponentType} ${validComponentName} { get; }
	bool Has${ComponentName} { get; }

	void Add${ComponentName}(${newMethodParameters});
	void Replace${ComponentName}(${newMethodParameters});
	void Remove${ComponentName}();
}
";

		private const string FLAG_TEMPLATE =
@"public partial interface I${ComponentName}Entity
{
	bool ${prefixedComponentName} { get; set; }
}
";

		private const string ENTITY_INTERFACE_TEMPLATE = "public partial class ${EntityType} : I${ComponentName}Entity { }\n";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.ShouldGenerateMethods())
				.Where(d => d.GetContextNames().Length > 1)
				.SelectMany(Generate)
				.ToArray();
		}

		private CodeGenFile[] Generate(ComponentData data)
		{
			return new[]
				{
					GenerateInterface(data)
				}
				.Concat(data.GetContextNames().Select(contextName => GenerateEntityInterface(contextName, data)))
				.ToArray();
		}

		private CodeGenFile GenerateInterface(ComponentData data)
		{
			var template = data.GetMemberData().Length == 0
				? FLAG_TEMPLATE
				: STANDARD_TEMPLATE;

			return new CodeGenFile(
				"Components" +
				Path.DirectorySeparatorChar +
				"Interfaces" +
				Path.DirectorySeparatorChar +
				"I" +
				data.ComponentName() +
				"Entity.cs",
				template.Replace(data, string.Empty),
				GetType().FullName);
		}

		private CodeGenFile GenerateEntityInterface(string contextName, ComponentData data)
		{
			return new CodeGenFile(
				contextName +
				Path.DirectorySeparatorChar +
				"Components" +
				Path.DirectorySeparatorChar +
				data.ComponentNameWithContext(contextName).AddComponentSuffix() +
				".cs",
				ENTITY_INTERFACE_TEMPLATE.Replace(data, contextName),
				GetType().FullName);
		}
	}
}
