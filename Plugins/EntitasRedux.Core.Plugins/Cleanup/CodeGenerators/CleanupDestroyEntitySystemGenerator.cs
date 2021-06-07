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
using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	/// <summary>
	/// A code generator that creates a system for destroying all entities that have a specific type of component at
	/// the end of the frame.
	/// </summary>
	internal sealed class CleanupDestroyEntitySystemGenerator : ICodeGenerator
	{
		public string Name => NAME;
		public int Priority => PRIORITY;
		public bool RunInDryMode => DEFAULT_DRY_RUN_MODE;

		private const string NAME = "Cleanup Destroy Entity System";
		private const int PRIORITY = 0;
		private const bool DEFAULT_DRY_RUN_MODE = true;

		// Filename and path
		private const string SYSTEM_CLASS_NAME = "{ClassName}";
		private const string FILENAME = "Destroy${ContextName}EntitiesWith${componentName}System.cs";
		private const string FILEPATH = "${ContextName}/Systems/";

		// Template
		private const string FILE_TEMPLATE = @"using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class {ClassName} : ICleanupSystem
{
	private readonly IGroup<${ContextName}Entity> _group;
	private readonly List<${ContextName}Entity> _entities;

	public {ClassName}(IContext<${ContextName}Entity> context)
	{
		_group = context.GetGroup(${ContextName}Matcher.${componentName});
		_entities = new List<${ContextName}Entity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].Destroy();
		}
	}
}
";
		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.HasCleanupDestroyEntityData())
				.SelectMany(GenerateCleanupSystems)
				.ToArray();
		}

		private IEnumerable<CodeGenFile> GenerateCleanupSystems(ComponentData data)
		{
			return data
				.GetContextNames()
				.Select(contextName => GenerateCleanupSystem(contextName, data));
		}

		private CodeGenFile GenerateCleanupSystem(string contextName, ComponentData data)
		{
			var filename = FILENAME
				.Replace(contextName)
				.Replace(data, contextName);
			var absoluteFilePath = Path.Combine(FILEPATH.Replace(contextName), filename);
			var fileContents = FILE_TEMPLATE
				.Replace(data, contextName)
				.Replace(SYSTEM_CLASS_NAME, data.GetCleanupDestroySystemClassName(contextName));

			return new CodeGenFile(absoluteFilePath, fileContents, NAME);
		}
	}
}
