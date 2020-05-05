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

using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Blueprints.Editor.Plugins
{
	internal sealed class BlueprintsGenerator : ICodeGenerator
	{
		private const string CLASS_TEMPLATE =
			@"using JCMG.EntitasRedux.Blueprints;

public static class BlueprintsExtension {

${blueprints}
}
";

		private const string GETTER_TEMPLATE =
			@"    public static Blueprint ${ValidPropertyName}(this Blueprints blueprints) {
        return blueprints.GetBlueprint(""${Name}"");
    }";

		private string GenerateBlueprintGetters(string[] blueprintNames)
		{
			return string.Join(
				"\n",
				blueprintNames
					.Select(
						name => GETTER_TEMPLATE
							.Replace("${ValidPropertyName}", ValidPropertyName(name))
							.Replace("${Name}", name))
					.ToArray());
		}

		private static string ValidPropertyName(string name)
		{
			return name
				.Replace(" ", string.Empty)
				.Replace("-", string.Empty)
				.Replace("(", string.Empty)
				.Replace(")", string.Empty);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Blueprint";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var blueprintNames = data
				.OfType<BlueprintData>()
				.Select(d => d.GetBlueprintName())
				.OrderBy(name => name)
				.ToArray();

			if (blueprintNames.Length == 0)
			{
				return new CodeGenFile[0];
			}

			var blueprints = CLASS_TEMPLATE.Replace("${blueprints}", GenerateBlueprintGetters(blueprintNames));
			return new[]
			{
				new CodeGenFile(
					"GeneratedBlueprints.cs",
					blueprints,
					GetType().FullName)
			};
		}
	}
}
