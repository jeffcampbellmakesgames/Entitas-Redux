using JCMG.EntitasRedux.Editor.Plugins;
using JCMG.Genesis.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JCMG.EntitasRedux.Blueprints.Editor.Plugins
{
	internal sealed class BlueprintClassGenerator : ICodeGenerator
	{
		public string Name => NAME;

		public int Priority => DEFAULT_PRIORITY;

		public bool RunInDryMode => DEFAULT_DRY_RUN_MODE;

		private const string NAME = "Blueprint Class Generator";
		private const int DEFAULT_PRIORITY = 0;
		private const bool DEFAULT_DRY_RUN_MODE = true;

		// Code-generation format strings
		private const string BLUEPRINT_BEHAVIOUR_CLASS_TEMPLATE = @"
using JCMG.EntitasRedux.Blueprints;
using UnityEngine;

/// <summary>
/// Represents a group of <see cref=""JCMG.EntitasRedux.IComponent""/> instances that can be copied to one or more
/// <see cref=""${ContextName}Entity""/>.
/// </summary>
[AddComponentMenu(""EntitasRedux/${ContextName}/${ContextName}BlueprintBehaviour"")]
public partial class ${ContextName}BlueprintBehaviour : BlueprintBehaviourBase, I${ContextName}Blueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name=""entity""/>.
	/// </summary>
	/// <param name=""entity""></param>
	public void ApplyToEntity(${ContextName}Entity entity)
	{
		for (var i = 0; i < _components.Count; i++)
		{
			var component = _components[i];
			if(_components[i] == null)
			{
				continue;
			}

			var index = ${ContextName}ComponentsLookup.GetComponentIndex(component);
			if (index != -1)
			{
				entity.AddComponent(index, component);
			}
			else
			{
				Debug.LogWarningFormat(
					JCMG.EntitasRedux.RuntimeConstants.COMPONENT_INDEX_DOES_NOT_EXIST_FOR_TYPE_FORMAT,
					component.GetType(),
					typeof(${ContextName}ComponentsLookup));
			}
		}
	}

	protected override void OnValidate()
	{
		base.OnValidate();

		// Remove any components that no longer belong to this context.
		for (var i = _components.Count - 1; i >= 0; i--)
		{
			var index = ${ContextName}ComponentsLookup.GetComponentIndex(_components[i]);
			if (index == -1)
			{
				_components.RemoveAt(i);
			}
		}
	}
}
";

		private const string BLUEPRINT_CONFIG_CLASS_TEMPLATE = @"
using JCMG.EntitasRedux.Blueprints;
using UnityEngine;

/// <summary>
/// Represents a group of <see cref=""JCMG.EntitasRedux.IComponent""/> instances that can be copied to one or more
/// <see cref=""${ContextName}Entity""/>.
/// </summary>
[CreateAssetMenu(fileName = ""New${ContextName}Blueprint"", menuName = ""EntitasRedux/${ContextName}/${ContextName}Blueprint"")]
public partial class ${ContextName}Blueprint : BlueprintBase, I${ContextName}Blueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name=""entity""/>.
	/// </summary>
	/// <param name=""entity""></param>
	public void ApplyToEntity(${ContextName}Entity entity)
	{
		for (var i = 0; i < _components.Count; i++)
		{
			var component = _components[i];
			if(_components[i] == null)
			{
				continue;
			}

			var index = ${ContextName}ComponentsLookup.GetComponentIndex(component);
			if (index != -1)
			{
				entity.AddComponent(index, component);
			}
			else
			{
				Debug.LogWarningFormat(
					JCMG.EntitasRedux.RuntimeConstants.COMPONENT_INDEX_DOES_NOT_EXIST_FOR_TYPE_FORMAT,
					component.GetType(),
					typeof(${ContextName}ComponentsLookup));
			}
		}
	}

	protected override void OnValidate()
	{
		base.OnValidate();

		// Remove any components that no longer belong to this context.
		for (var i = _components.Count - 1; i >= 0; i--)
		{
			var index = ${ContextName}ComponentsLookup.GetComponentIndex(_components[i]);
			if (index == -1)
			{
				_components.RemoveAt(i);
			}
		}
	}
}
";

		private const string BLUEPRINT_INTERFACE_TEMPLATE = @"
/// <summary>
/// Represents a group of <see cref=""JCMG.EntitasRedux.IComponent""/> instances that can be copied to one or more
/// <see cref=""${ContextName}Entity""/>.
/// </summary>
public interface I${ContextName}Blueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name=""entity""/>.
	/// </summary>
	/// <param name=""entity""></param>
	void ApplyToEntity(${ContextName}Entity entity);
}
";

		// Find and replace tokens
		private const string CONTEXT_NAME_TOKEN = "${ContextName}";

		private const string BLUEPRINT_BEHAVIOUR_FILENAME_FORMAT = "${ContextName}BlueprintBehaviour.cs";
		private const string BLUEPRINT_CONFIG_FILENAME_FORMAT = "${ContextName}Blueprint.cs";
		private const string BLUEPRINT_INTERFACE_FILENAME_FORMAT = "I${ContextName}Blueprint.cs";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var codeGenFilesResult = new List<CodeGenFile>();

			// Create a blueprint class per context
			var allContextData = data.OfType<ContextData>().ToArray();
			for (var i = 0; i < allContextData.Length; i++)
			{
				var contextData = allContextData[i];
				var contextName = contextData.GetContextName();

				// Create Blueprint Interface
				var interfaceFilename = BLUEPRINT_INTERFACE_FILENAME_FORMAT.Replace(CONTEXT_NAME_TOKEN, contextName);
				var interfaceAbsoluteFilename = Path.Combine(contextName, interfaceFilename);
				var interfaceFileContents = BLUEPRINT_INTERFACE_TEMPLATE.Replace(CONTEXT_NAME_TOKEN, contextName);
				codeGenFilesResult.Add(new CodeGenFile(interfaceAbsoluteFilename, interfaceFileContents, NAME));

				// Create Blueprint ScriptableObject
				var blueprintFilename = BLUEPRINT_CONFIG_FILENAME_FORMAT.Replace(CONTEXT_NAME_TOKEN, contextName);
				var blueprintAbsoluteFilename = Path.Combine(contextName, blueprintFilename);
				var blueprintFileContents = BLUEPRINT_CONFIG_CLASS_TEMPLATE.Replace(CONTEXT_NAME_TOKEN, contextName);
				codeGenFilesResult.Add(new CodeGenFile(blueprintAbsoluteFilename, blueprintFileContents, NAME));

				// Create Blueprint MonoBehaviour
				var behaviourFilename = BLUEPRINT_BEHAVIOUR_FILENAME_FORMAT.Replace(CONTEXT_NAME_TOKEN, contextName);
				var behaviourAbsoluteFilename = Path.Combine(contextName, behaviourFilename);
				var behaviourFileContents = BLUEPRINT_BEHAVIOUR_CLASS_TEMPLATE.Replace(CONTEXT_NAME_TOKEN, contextName);
				codeGenFilesResult.Add(new CodeGenFile(behaviourAbsoluteFilename, behaviourFileContents, NAME));
			}

			return codeGenFilesResult.ToArray();
		}
	}
}
