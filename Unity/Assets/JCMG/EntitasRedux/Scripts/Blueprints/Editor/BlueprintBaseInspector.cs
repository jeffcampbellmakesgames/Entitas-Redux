using UnityEditor;

namespace JCMG.EntitasRedux.Blueprints.Editor
{
	[CustomEditor(typeof(BlueprintBase), editorForChildClasses:true)]
	internal sealed class BlueprintBaseInspector : BlueprintInspector
	{
		private BlueprintBase _blueprintBase;

		protected override void OnEnable()
		{
			// Get blueprint type info
			_blueprintBase = (BlueprintBase)target;

			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			DrawComponentSelectorGUI(_blueprintBase.Components, OnComponentAdded);
			DrawComponentArrayGUI(_blueprintBase.Components);
		}

		private void OnComponentAdded()
		{
			_blueprintBase.Validate();

			EditorUtility.SetDirty(target);
		}

		protected override void OnComponentRemoved(IComponent component)
		{
			// If the component was removed, set the target as dirty.
			if (_blueprintBase.Components.Remove(component))
			{
				_blueprintBase.Validate();

				EditorUtility.SetDirty(target);
			}
		}
	}
}
