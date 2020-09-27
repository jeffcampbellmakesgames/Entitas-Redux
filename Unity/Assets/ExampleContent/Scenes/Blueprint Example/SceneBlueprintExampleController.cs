using UnityEngine;

namespace ExampleContent.Blueprints
{
	internal sealed class SceneBlueprintExampleController : MonoBehaviour
	{
		#pragma warning disable 0649
		[SerializeField]
		private VisualDebugBlueprint _blueprint;
		#pragma warning restore 0649

		private void Awake()
		{
			// Get the relevant context for this type of blueprint, spin up ten entities and apply the blueprint to
			// them. This results in all of those entities having the same components from the blueprint.
			var context = Contexts.SharedInstance.VisualDebug;
			for (var i = 0; i < 10; i++)
			{
				var entity = context.CreateEntity();
				_blueprint.ApplyToEntity(entity);
			}
		}
	}
}
