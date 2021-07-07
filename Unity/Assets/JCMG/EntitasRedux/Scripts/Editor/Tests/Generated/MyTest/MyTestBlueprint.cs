
using JCMG.EntitasRedux.Blueprints;
using UnityEngine;

/// <summary>
/// Represents a group of <see cref="JCMG.EntitasRedux.IComponent"/> instances that can be copied to one or more
/// <see cref="MyTestEntity"/>.
/// </summary>
[CreateAssetMenu(fileName = "NewMyTestBlueprint", menuName = "EntitasRedux/MyTest/MyTestBlueprint")]
public partial class MyTestBlueprint : BlueprintBase, IMyTestBlueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name="entity"/>.
	/// </summary>
	/// <param name="entity"></param>
	public void ApplyToEntity(MyTestEntity entity)
	{
		for (var i = 0; i < _components.Count; i++)
		{
			var component = _components[i];
			if(_components[i] == null)
			{
				continue;
			}

			var index = MyTestComponentsLookup.GetComponentIndex(component);
			if (index != -1)
			{
				entity.CopyComponentTo(component);
			}
			else
			{
				Debug.LogWarningFormat(
					JCMG.EntitasRedux.RuntimeConstants.COMPONENT_INDEX_DOES_NOT_EXIST_FOR_TYPE_FORMAT,
					component.GetType(),
					typeof(MyTestComponentsLookup));
			}
		}
	}

	protected override void OnValidate()
	{
		base.OnValidate();

		// Remove any components that no longer belong to this context.
		for (var i = _components.Count - 1; i >= 0; i--)
		{
			var index = MyTestComponentsLookup.GetComponentIndex(_components[i]);
			if (index == -1)
			{
				_components.RemoveAt(i);
			}
		}
	}
}
