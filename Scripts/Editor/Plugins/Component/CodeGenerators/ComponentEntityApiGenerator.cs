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
using System.IO;
using System.Linq;
using System.Text;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ComponentEntityApiGenerator : AbstractGenerator
	{
		public override string Name
		{
			get { return "Component (Entity API)"; }
		}

		private static readonly StringBuilder _SB;

		static ComponentEntityApiGenerator()
		{
			_SB = new StringBuilder();
		}

		private const string STANDARD_TEMPLATE =
			@"public partial class ${EntityType} {

    public ${ComponentType} ${validComponentName} { get { return (${ComponentType})GetComponent(${Index}); } }
    public bool Has${ComponentName} { get { return HasComponent(${Index}); } }

    public void Add${ComponentName}(${newMethodParameters}) {
        var index = ${Index};
        var component = (${ComponentType})CreateComponent(index, typeof(${ComponentType}));
${memberAssignmentList}
        AddComponent(index, component);
    }

    public void Replace${ComponentName}(${newMethodParameters}) {
        var index = ${Index};
        var component = (${ComponentType})CreateComponent(index, typeof(${ComponentType}));
${memberAssignmentList}
        ReplaceComponent(index, component);
    }

	public void Copy${ComponentName}To(${ComponentType} copyComponent) {
        var index = ${Index};
        var component = (${ComponentType})CreateComponent(index, typeof(${ComponentType}));
${memberCopyAssignmentList}
        ReplaceComponent(index, component);
    }

    public void Remove${ComponentName}() {
        RemoveComponent(${Index});
    }
}
";

		private const string FLAG_TEMPLATE =
@"public partial class ${EntityType} {

    static readonly ${ComponentType} ${componentName}Component = new ${ComponentType}();

    public bool ${prefixedComponentName} {
        get { return HasComponent(${Index}); }
        set {
            if (value != ${prefixedComponentName}) {
                var index = ${Index};
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : ${componentName}Component;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}
";

		private const string COPY_COMPONENT_TO_METHOD_TEMPLATE =
@"
using JCMG.EntitasRedux;

public partial class ${ContextName}Entity
{
	public void CopyComponentTo(IComponent component)
	{
${copyToComponentList}
	}
}
";

		private const string COPY_COMPONENT_TO_IF_TEMPLATE =
@"		if (component is ${ComponentType} ${componentName})
		{
			${ComponentCall}
		}";

		private const string COPY_COMPONENT_TO_ELSE_IF_TEMPLATE =
@"		else if (component is ${ComponentType} ${componentName})
		{
			${ComponentCall}
		}";

		private const string COPY_COMPONENT_TO_TEMPLATE = "Copy${ComponentName}To(${componentName});";
		private const string SET_COMPONENT_FLAG_TEMPLATE = "${prefixedComponentName} = true;";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var methodComponentData = data
				.OfType<ComponentData>()
				.Where(d => d.ShouldGenerateMethods())
				.ToArray();

			var contextNameToComponentData = methodComponentData
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

			var codeGenFileList = new List<CodeGenFile>();
			var componentSpecificCodeGenFiles = methodComponentData
				.SelectMany(Generate)
				.ToArray();

			var copyToCodeGenFile = contextNameToComponentData
				.Select(y => GenerateCopyToMethodFile(y.Key, y.Value.ToArray()));

			codeGenFileList.AddRange(componentSpecificCodeGenFiles);
			codeGenFileList.AddRange(copyToCodeGenFile);

			return codeGenFileList.ToArray();
		}

		private CodeGenFile[] Generate(ComponentData data)
		{
			return data.GetContextNames()
				.Select(contextName => Generate(contextName, data))
				.ToArray();
		}

		private CodeGenFile Generate(string contextName, ComponentData data)
		{
			var template = data.GetMemberData().Length == 0
				? FLAG_TEMPLATE
				: STANDARD_TEMPLATE;

			var fileContent = template
				.Replace("${memberAssignmentList}", GetMemberAssignmentList(data.GetMemberData()))
				.Replace("${memberCopyAssignmentList}", GetMemberCopyAssignmentList(data.GetMemberData()))
				.Replace(data, contextName);

			return new CodeGenFile(
				contextName +
				Path.DirectorySeparatorChar +
				"Components" +
				Path.DirectorySeparatorChar +
				data.ComponentNameWithContext(contextName).AddComponentSuffix() +
				".cs",
				fileContent,
				GetType().FullName);
		}

		private string GetMemberAssignmentList(MemberData[] memberData)
		{
			return string.Join("\n", memberData.Select(info => info.GetMemberAssignment()).ToArray());
		}

		private string GetMemberCopyAssignmentList(MemberData[] memberData)
		{
			_SB.Clear();
			for (var i = 0; i < memberData.Length; i++)
			{
				var result = string.Empty;

				// If we have type information to work with and its a reference type, use more specific copy assignment
				var md = memberData[i];
				var memberType = md.memberType;

				if (memberType != null)
				{
					result = md.GetMemberCopyAssignment(memberType);
				}

				// If we don't have a more nuanced copy assignment, use straight assignment
				if(string.IsNullOrEmpty(result))
				{
					result = md.GetMemberCopyAssignment();
				}

				// Append the final copy member result to the method implementation.
				if (i < memberData.Length - 1)
				{
					_SB.AppendLine(result);
				}
				else
				{
					_SB.Append(result);
				}
			}

			return _SB.ToString();
		}

		private CodeGenFile GenerateCopyToMethodFile(string contextName, ComponentData[] componentData)
		{
			const string FILENAME_FORMAT = "${ContextName}Entity_CopyTo.cs";
			var filename = Path.Combine(
				Path.Combine(contextName, "Components"),
				FILENAME_FORMAT.Replace(contextName));

			_SB.Clear();
			for (var i = 0; i < componentData.Length; ++i)
			{
				var cd = componentData[i];
				var block_template = i == 0
					? COPY_COMPONENT_TO_IF_TEMPLATE
					: COPY_COMPONENT_TO_ELSE_IF_TEMPLATE;

				var statement_template = cd.IsFlag()
					? SET_COMPONENT_FLAG_TEMPLATE
					: COPY_COMPONENT_TO_TEMPLATE;

				var finalCodeSnippet = block_template
					.Replace(cd, contextName)
					.Replace("${ComponentCall}",
						statement_template.Replace(cd, contextName));

				if (i < componentData.Length - 1)
				{
					_SB.AppendLine(finalCodeSnippet);
				}
				else
				{
					_SB.Append(finalCodeSnippet);
				}
			}

			var fileContents = COPY_COMPONENT_TO_METHOD_TEMPLATE
				.Replace(contextName)
				.Replace("${copyToComponentList}", _SB.ToString());

			return new CodeGenFile(filename, fileContents, Name);
		}
	}
}
