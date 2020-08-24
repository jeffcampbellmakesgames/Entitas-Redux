using UnityEditor;

namespace JCMG.EntitasRedux.Blueprints.Editor
{
	[CustomEditor(typeof(BlueprintBehaviourBase), editorForChildClasses:true)]
	internal sealed class BlueprintBehaviourBaseInspector : BlueprintInspector
	{
		private BlueprintBehaviourBase _blueprintBehaviourBase;

		protected override void OnEnable()
		{
			// Get blueprint type info
			_blueprintBehaviourBase = (BlueprintBehaviourBase)target;

			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			DrawComponentSelectorGUI(_blueprintBehaviourBase.Components, OnComponentAdded);
			DrawComponentArrayGUI(_blueprintBehaviourBase.Components);
		}

		private void OnComponentAdded()
		{
			_blueprintBehaviourBase.Validate();

			EditorUtility.SetDirty(target);
		}

		protected override void OnComponentRemoved(IComponent component)
		{
			// If the component was removed, set the target as dirty.
			if (_blueprintBehaviourBase.Components.Remove(component))
			{
				_blueprintBehaviourBase.Validate();

				EditorUtility.SetDirty(target);
			}
		}
	}
}
