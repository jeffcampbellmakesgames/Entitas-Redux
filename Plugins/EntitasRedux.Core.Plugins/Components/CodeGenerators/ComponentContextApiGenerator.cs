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
	internal sealed class ComponentContextApiGenerator : AbstractGenerator
	{
		public override string Name
		{
			get { return "Component (Context API)"; }
		}

		private const string STANDARD_TEMPLATE =
			@"public partial class ${ContextType} {

	public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }
	public ${ComponentType} ${validComponentName} { get { return ${componentName}Entity.${componentName}; } }
	public bool Has${ComponentName} { get { return ${componentName}Entity != null; } }

	public ${EntityType} Set${ComponentName}(${newMethodParameters})
	{
		if (Has${ComponentName})
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				""Could not set ${ComponentName}!\n"" +
				this +
				"" already has an entity with ${ComponentType}!"",
				""You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}()."");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.Add${ComponentName}(${newMethodArgs});
		#endif
		return entity;
	}

	public void Replace${ComponentName}(${newMethodParameters})
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = ${componentName}Entity;
		if (entity == null)
		{
			entity = Set${ComponentName}(${newMethodArgs});
		}
		else
		{
			entity.Replace${ComponentName}(${newMethodArgs});
		}
		#endif
	}

	public void Remove${ComponentName}()
	{
		${componentName}Entity.Destroy();
	}
}
";

		private const string FLAG_TEMPLATE =
@"public partial class ${ContextType}
{
	public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }

	public bool ${prefixedComponentName}
	{
		get { return ${componentName}Entity != null; }
		set
		{
			var entity = ${componentName}Entity;
			if (value != (entity != null))
			{
				if (value)
				{
					CreateEntity().${prefixedComponentName} = true;
				}
				else
				{
					entity.Destroy();
				}
			}
		}
	}
}
";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.ShouldGenerateMethods())
				.Where(d => d.IsUnique())
				.SelectMany(Generate)
				.ToArray();
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

			return new CodeGenFile(
				contextName +
				Path.DirectorySeparatorChar +
				"Components" +
				Path.DirectorySeparatorChar +
				data.ComponentNameWithContext(contextName).AddComponentSuffix() +
				".cs",
				template.Replace(data, contextName),
				GetType().FullName);
		}
	}
}
