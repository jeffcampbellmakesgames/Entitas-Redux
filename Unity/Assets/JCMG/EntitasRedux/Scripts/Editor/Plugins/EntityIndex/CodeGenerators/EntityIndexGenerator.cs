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
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class EntityIndexGenerator : ICodeGenerator
	{
		private const string CLASS_TEMPLATE =
@"public partial class Contexts
{
${indexConstants}

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeEntityIndices()
	{
${addIndices}
	}
}

public static class ContextsExtensions
{
${getIndices}
}";

		private const string INDEX_CONSTANTS_TEMPLATE = @"	public const string ${IndexName} = ""${IndexName}"";";

		private const string ADD_INDEX_TEMPLATE =
			@"		${contextName}.AddEntityIndex(new ${IndexType}<${ContextName}Entity, ${KeyType}>(
			${IndexName},
			${contextName}.GetGroup(${ContextName}Matcher.${Matcher}),
			(e, c) => ((${ComponentType})c).${MemberName}));";

		private const string ADD_CUSTOM_INDEX_TEMPLATE =
			@"		${contextName}.AddEntityIndex(new ${IndexType}(${contextName}));";

		private const string GET_INDEX_TEMPLATE =
@"	public static System.Collections.Generic.HashSet<${ContextName}Entity> GetEntitiesWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName})
	{
		return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntities(${MemberName});
	}";

		private const string GET_PRIMARY_INDEX_TEMPLATE =
@"	public static ${ContextName}Entity GetEntityWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName})
	{
		return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntity(${MemberName});
	}";

		private const string CUSTOM_METHOD_TEMPLATE =
@"	public static ${ReturnType} ${MethodName}(this ${ContextName}Context context, ${methodArgs})
	{
		return ((${IndexType})(context.GetEntityIndex(Contexts.${IndexName}))).${MethodName}(${args});
	}
";

		private CodeGenFile[] GenerateEntityIndices(EntityIndexData[] data)
		{
			var indexConstants = string.Join(
				"\n",
				data
					.Select(
						d => INDEX_CONSTANTS_TEMPLATE
							.Replace(
								"${IndexName}",
								d.GetHasMultiple()
									? d.GetEntityIndexName() + d.GetMemberName().UppercaseFirst()
									: d.GetEntityIndexName()))
					.ToArray());

			var addIndices = string.Join(
				"\n\n",
				data
					.Select(GenerateAddMethods)
					.ToArray());

			var getIndices = string.Join(
				"\n\n",
				data
					.Select(GenerateGetMethods)
					.ToArray());

			var fileContent = CLASS_TEMPLATE
				.Replace("${indexConstants}", indexConstants)
				.Replace("${addIndices}", addIndices)
				.Replace("${getIndices}", getIndices);

			return new[]
			{
				new CodeGenFile(
					"Contexts.cs",
					fileContent,
					GetType().FullName)
			};
		}

		private string GenerateAddMethods(EntityIndexData data)
		{
			return string.Join(
				"\n",
				data.GetContextNames()
					.Aggregate(
						new List<string>(),
						(addMethods, contextName) =>
						{
							addMethods.Add(GenerateAddMethod(data, contextName));
							return addMethods;
						})
					.ToArray());
		}

		private string GenerateAddMethod(EntityIndexData data, string contextName)
		{
			return data.IsCustom()
				? GenerateCustomMethods(data)
				: GenerateMethods(data, contextName);
		}

		private string GenerateCustomMethods(EntityIndexData data)
		{
			return ADD_CUSTOM_INDEX_TEMPLATE
				.Replace("${contextName}", data.GetContextNames()[0])
				.Replace("${IndexType}", data.GetEntityIndexType());
		}

		private string GenerateMethods(EntityIndexData data, string contextName)
		{
			return ADD_INDEX_TEMPLATE
				.Replace("${contextName}", contextName)
				.Replace("${ContextName}", contextName)
				.Replace(
					"${IndexName}",
					data.GetHasMultiple()
						? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
						: data.GetEntityIndexName())
				.Replace("${Matcher}", data.GetEntityIndexName())
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${ComponentType}", data.GetComponentType())
				.Replace("${MemberName}", data.GetMemberName())
				.Replace(
					"${componentName}",
					data.GetComponentType()
						.ToComponentName()
						.LowercaseFirst()
						.AddPrefixIfIsKeyword());
		}

		private string GenerateGetMethods(EntityIndexData data)
		{
			return string.Join(
				"\n\n",
				data.GetContextNames()
					.Aggregate(
						new List<string>(),
						(getMethods, contextName) =>
						{
							getMethods.Add(GenerateGetMethod(data, contextName));
							return getMethods;
						})
					.ToArray());
		}

		private string GenerateGetMethod(EntityIndexData data, string contextName)
		{
			var template = "";
			if (data.GetEntityIndexType() == "JCMG.EntitasRedux.EntityIndex")
			{
				template = GET_INDEX_TEMPLATE;
			}
			else if (data.GetEntityIndexType() == "JCMG.EntitasRedux.PrimaryEntityIndex")
			{
				template = GET_PRIMARY_INDEX_TEMPLATE;
			}
			else
			{
				return GetCustomMethods(data);
			}

			return template
				.Replace("${ContextName}", contextName)
				.Replace(
					"${IndexName}",
					data.GetHasMultiple()
						? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
						: data.GetEntityIndexName())
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${MemberName}", data.GetMemberName());
		}

		private string GetCustomMethods(EntityIndexData data)
		{
			return string.Join(
				"\n",
				data.GetCustomMethods()
					.Select(
						m => CUSTOM_METHOD_TEMPLATE
							.Replace("${ReturnType}", m.returnType)
							.Replace("${MethodName}", m.methodName)
							.Replace("${ContextName}", data.GetContextNames()[0])
							.Replace(
								"${methodArgs}",
								string.Join(", ", m.parameters.Select(p => p.compilableTypeString + " " + p.name).ToArray()))
							.Replace("${IndexType}", data.GetEntityIndexType())
							.Replace(
								"${IndexName}",
								data.GetHasMultiple()
									? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
									: data.GetEntityIndexName())
							.Replace("${args}", string.Join(", ", m.parameters.Select(p => p.name).ToArray())))
					.ToArray());
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Entity Index";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var entityIndexData = data
				.OfType<EntityIndexData>()
				.OrderBy(d => d.GetEntityIndexName())
				.ToArray();

			return entityIndexData.Length == 0
				? new CodeGenFile[0]
				: GenerateEntityIndices(entityIndexData);
		}
	}
}
