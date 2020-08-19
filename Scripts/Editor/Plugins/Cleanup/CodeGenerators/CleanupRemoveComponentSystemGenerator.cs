using JCMG.Genesis.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JCMG.EntitasRedux.Editor.Plugins.Cleanup.CodeGenerators
{
	/// <summary>
	/// A code generator that creates a system for removing all of a specific type of component from any entity that has
	/// one at the end of the frame.
	/// </summary>
	internal sealed class CleanupRemoveComponentSystemGenerator : ICodeGenerator
	{
		public string Name => NAME;
		public int Priority => PRIORITY;
		public bool RunInDryMode => DEFAULT_DRY_RUN_MODE;

		private const string NAME = "Cleanup Remove Component System";
		private const int PRIORITY = 0;
		private const bool DEFAULT_DRY_RUN_MODE = true;

		// Filename and path
		private const string FILENAME = "Remove${componentName}From${ContextName}EntitiesSystem.cs";
		private const string FILEPATH = "Cleanup/Systems/";

		// Tokens
		private const string REMOVE_COMPONENT_LOGIC = "${RemoveComponentLogic}";

		// Template
		private const string FLAG_REMOVE_LOGIC = "_entities[i].${prefixedComponentName} = false;";
		private const string NON_FLAG_REMOVE_LOGIC = "_entities[i].Remove${componentName}();";

		private const string FILE_TEMPLATE = @"using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class Remove${componentName}From${ContextName}EntitiesSystem : ICleanupSystem
{
	private readonly IGroup<${ContextName}Entity> _group;
	private readonly List<${ContextName}Entity> _entities;

	public Remove${componentName}From${ContextName}EntitiesSystem(IContext<${ContextName}Entity> context)
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
			${RemoveComponentLogic}
		}
	}
}
";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.HasCleanupRemoveComponentData())
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
			var absoluteFilePath = Path.Combine(FILEPATH, filename);

			var removeComponentLogic = data.IsFlag()
				? FLAG_REMOVE_LOGIC.Replace(data, contextName)
				: NON_FLAG_REMOVE_LOGIC.Replace(data, contextName);

			var fileContents = FILE_TEMPLATE
				.Replace(data, contextName)
				.Replace(REMOVE_COMPONENT_LOGIC, removeComponentLogic);

			return new CodeGenFile(absoluteFilePath, fileContents, NAME);
		}
	}
}
