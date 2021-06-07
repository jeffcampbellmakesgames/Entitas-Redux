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
using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class ComponentGenerator : ICodeGenerator
	{
		private const string COMPONENT_TEMPLATE =
@"[JCMG.EntitasRedux.DontGenerate(false)]
public sealed class ${FullComponentName} : JCMG.EntitasRedux.IComponent
{
	public ${Type} value;
}
";

		private CodeGenFile Generate(ComponentData data)
		{
			var fullComponentName = data.GetTypeName().RemoveDots();
			return new CodeGenFile(
				"Components" +
				Path.DirectorySeparatorChar +
				fullComponentName +
				".cs",
				COMPONENT_TEMPLATE
					.Replace("${FullComponentName}", fullComponentName)
					.Replace("${Type}", data.GetObjectTypeName()),
				GetType().FullName);
		}

		public string Name
		{
			get { return "Component"; }
		}

		public int Priority
		{
			get { return 0; }
		}

		public bool RunInDryMode
		{
			get { return true; }
		}

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.ShouldGenerateComponent())
				.Select(Generate)
				.ToArray();
		}
	}
}
