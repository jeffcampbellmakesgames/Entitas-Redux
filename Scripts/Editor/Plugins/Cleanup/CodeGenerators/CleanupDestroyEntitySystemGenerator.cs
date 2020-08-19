using JCMG.Genesis.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JCMG.EntitasRedux.Editor.Plugins.Cleanup.CodeGenerators
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
		private const string FILENAME = "Destroy${ContextName}EntitiesWith${componentName}System.cs";
		private const string FILEPATH = "Cleanup/Systems/";

		// Template
		private const string FILE_TEMPLATE = @"using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class Destroy${ContextName}EntitiesWith${componentName}System : ICleanupSystem
{
	private readonly IGroup<${ContextName}Entity> _group;
	private readonly List<${ContextName}Entity> _entities;

	public Destroy${ContextName}EntitiesWith${componentName}System(IContext<${ContextName}Entity> context)
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
			var absoluteFilePath = Path.Combine(FILEPATH, filename);
			var fileContents = FILE_TEMPLATE.Replace(data, contextName);

			return new CodeGenFile(absoluteFilePath, fileContents, NAME);
		}
	}
}
