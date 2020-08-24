using System.Collections.Generic;
using UnityEngine;

namespace JCMG.EntitasRedux.Blueprints
{
	public abstract class BlueprintBase : ScriptableObject
	{
		internal List<IComponent> Components => _components;

		[SerializeReference]
		protected List<IComponent> _components;

		protected virtual void OnEnable()
		{
			if (_components == null)
			{
				_components = new List<IComponent>();
			}
		}

		internal void Validate()
		{
			OnValidate();
		}

		protected virtual void OnValidate()
		{
			// Remove components that have become null due to serialization issues or other reason
			if (_components != null)
			{
				for (var i = _components.Count - 1; i >= 0; i--)
				{
					if (_components[i] == null)
					{
						_components.RemoveAt(i);
					}
				}
			}
		}
	}
}
