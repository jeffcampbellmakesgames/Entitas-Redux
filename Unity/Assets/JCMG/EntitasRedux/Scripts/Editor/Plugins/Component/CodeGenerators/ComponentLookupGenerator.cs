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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ComponentLookupGenerator : AbstractGenerator
	{
		public override string Name
		{
			get { return "Component (Lookup)"; }
		}

		private const string TEMPLATE =
			@"public static class ${Lookup} {

${componentConstantsList}

${totalComponentsConstant}

    public static readonly string[] ComponentNames = {
${componentNamesList}
    };

    public static readonly System.Type[] ComponentTypes = {
${componentTypesList}
    };
}
";

		private const string COMPONENT_CONSTANT_TEMPLATE = @"    public const int ${ComponentName} = ${Index};";
		private const string TOTAL_COMPONENTS_CONSTANT_TEMPLATE = @"    public const int TotalComponents = ${totalComponents};";
		private const string COMPONENT_NAME_TEMPLATE = @"        ""${ComponentName}""";
		private const string COMPONENT_TYPE_TEMPLATE = @"        typeof(${ComponentType})";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var lookups = GenerateLookups(
				data
					.OfType<ComponentData>()
					.Where(d => d.ShouldGenerateIndex())
					.ToArray());

			var existingFileNames = new HashSet<string>(lookups.Select(file => file.FileName));

			var emptyLookups = GenerateEmptyLookups(
					data
						.OfType<ContextData>()
						.ToArray())
				.Where(file => !existingFileNames.Contains(file.FileName))
				.ToArray();

			return lookups.Concat(emptyLookups).ToArray();
		}

		private CodeGenFile[] GenerateEmptyLookups(ContextData[] data)
		{
			var emptyData = new ComponentData[0];
			return data
				.Select(d => GenerateComponentsLookupClass(d.GetContextName(), emptyData))
				.ToArray();
		}

		private CodeGenFile[] GenerateLookups(ComponentData[] data)
		{
			var contextNameToComponentData = data
				.Aggregate(
					new Dictionary<string, List<ComponentData>>(),
					(dict, d) =>
					{
						var contextNames = d.GetContextNames();
						foreach (var contextName in contextNames)
						{
							if (!dict.ContainsKey(contextName))
							{
								dict.Add(contextName, new List<ComponentData>());
							}

							dict[contextName].Add(d);
						}

						return dict;
					});

			foreach (var key in contextNameToComponentData.Keys.ToArray())
			{
				contextNameToComponentData[key] = contextNameToComponentData[key]
					.OrderBy(d => d.GetTypeName())
					.ToList();
			}

			return contextNameToComponentData
				.Select(kv => GenerateComponentsLookupClass(kv.Key, kv.Value.ToArray()))
				.ToArray();
		}

		private CodeGenFile GenerateComponentsLookupClass(string contextName, ComponentData[] data)
		{
			var componentConstantsList = string.Join(
				"\n",
				data
					.Select(
						(d, index) => COMPONENT_CONSTANT_TEMPLATE
							.Replace("${ComponentName}", d.ComponentName())
							.Replace("${Index}", index.ToString()))
					.ToArray());

			var totalComponentsConstant = TOTAL_COMPONENTS_CONSTANT_TEMPLATE
				.Replace("${totalComponents}", data.Length.ToString());

			var componentNamesList = string.Join(
				",\n",
				data
					.Select(
						d => COMPONENT_NAME_TEMPLATE
							.Replace("${ComponentName}", d.ComponentName()))
					.ToArray());

			var componentTypesList = string.Join(
				",\n",
				data
					.Select(
						d => COMPONENT_TYPE_TEMPLATE
							.Replace("${ComponentType}", d.GetTypeName()))
					.ToArray());

			var fileContent = TEMPLATE
				.Replace("${Lookup}", contextName + CodeGeneratorExtensions.LOOKUP)
				.Replace("${componentConstantsList}", componentConstantsList)
				.Replace("${totalComponentsConstant}", totalComponentsConstant)
				.Replace("${componentNamesList}", componentNamesList)
				.Replace("${componentTypesList}", componentTypesList);

			return new CodeGenFile(
				contextName +
				Path.DirectorySeparatorChar +
				contextName +
				"ComponentsLookup.cs",
				fileContent,
				GetType().FullName);
		}
	}
}
