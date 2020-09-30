using System.Collections.Generic;
using System.IO;
using System.Linq;
using JCMG.Genesis.Editor;
using UnityEngine.Assertions;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	/// <summary>
	/// A code generator that creates a Feature class containing all cleanup systems relevant for a particular context.
	/// </summary>
	internal sealed class ContextCleanupFeatureGenerator : ICodeGenerator
	{
		public string Name => NAME;
		public int Priority => PRIORITY;
		public bool RunInDryMode => DEFAULT_DRY_RUN_MODE;

		private const string NAME = "Context Cleanup Feature Generator";
		private const int PRIORITY = 0;
		private const bool DEFAULT_DRY_RUN_MODE = true;

		// Filename and path
		private const string FILENAME = "${ContextName}CleanupFeature.cs";
		private const string FILEPATH = "${ContextName}/Features/";

		// Template
		private const string SYSTEM_CLASS_NAME = "{ClassName}";
		private const string ADD_CLEANUP_SYSTEMS = "{AddCleanupSystems}";
		private const string ALLOCATE_AND_ADD_SYSTEM_LOGIC
			= "		Add(new {ClassName}(context));";

		private const string FILE_TEMPLATE = @"using JCMG.EntitasRedux;

public class ${ContextName}CleanupFeature : Feature
{
	public ${ContextName}CleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.${ContextName});
	}

	public ${ContextName}CleanupFeature(IContext<${ContextName}Entity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<${ContextName}Entity> context)
	{
{AddCleanupSystems}
	}
}
";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var codeGenFiles = new List<CodeGenFile>();
			var componentData = data
				.OfType<ComponentData>()
				.ToArray();
			var contextNames = componentData
				.SelectMany(x => x.GetContextNames())
				.Distinct()
				.ToArray();

			for (var i = 0; i < contextNames.Length; ++i)
			{
				var contextName = contextNames[i];
				var contextComponentData = componentData
					.Where(x => (x.HasCleanupDestroyEntityData() || x.HasCleanupRemoveComponentData()) &&
								x.GetContextNames().Contains(contextName));
				codeGenFiles.Add(GenerateSystems(contextName, contextComponentData));
			}

			return codeGenFiles.ToArray();
		}

		private CodeGenFile GenerateSystems(string contextName, IEnumerable<ComponentData> data)
		{
			var filename = FILENAME.Replace(contextName);
			var absoluteFilePath = Path.Combine(FILEPATH.Replace(contextName), filename);
			var addCleanupSystemsContent = string.Join("\n", data.Select(x => GetAddSystemContent(contextName, x)));
			var fileContents = FILE_TEMPLATE
				.Replace(ADD_CLEANUP_SYSTEMS, addCleanupSystemsContent)
				.Replace(contextName);

			return new CodeGenFile(absoluteFilePath, fileContents, NAME);
		}

		private string GetAddSystemContent(string contextName, ComponentData data)
		{
			var result = string.Empty;
			if (data.HasCleanupDestroyEntityData())
			{
				result = ALLOCATE_AND_ADD_SYSTEM_LOGIC.Replace(SYSTEM_CLASS_NAME, data.GetCleanupDestroySystemClassName(contextName));
			}
			else if (data.HasCleanupRemoveComponentData())
			{
				result = ALLOCATE_AND_ADD_SYSTEM_LOGIC.Replace(SYSTEM_CLASS_NAME, data.GetCleanupRemoveSystemClassName(contextName));
			}

			Assert.IsFalse(string.IsNullOrEmpty(result));

			return result;
		}
	}
}
