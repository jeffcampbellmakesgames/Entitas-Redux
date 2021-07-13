
using JCMG.EntitasRedux;

public partial class ExampleEntity
{
	/// <summary>
	/// Copies <paramref name="component"/> to this entity as a new component instance.
	/// </summary>
	public void CopyComponentTo(IComponent component)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		if (component is ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent AnCleanupDestroyEntity)
		{
			IsAnCleanupDestroyEntity = true;
		}
		else if (component is ExampleContent.VisualDebugging.AnCleanupRemoveComponent AnCleanupRemove)
		{
			IsAnCleanupRemove = true;
		}
		#endif
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>.
	/// </summary>
	public void CopyTo(ExampleEntity copyToEntity)
	{
		for (var i = 0; i < ExampleComponentsLookup.TotalComponents; ++i)
		{
			if (HasComponent(i))
			{
				if (copyToEntity.HasComponent(i))
				{
					throw new EntityAlreadyHasComponentException(
						i,
						"Cannot copy component '" +
						ExampleComponentsLookup.ComponentNames[i] +
						"' to " +
						this +
						"!",
						"If replacement is intended, please call CopyTo() with `replaceExisting` set to true.");
				}

				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>; if <paramref name="replaceExisting"/>
	/// is true any of the components that <paramref name="copyToEntity"/> has that this entity has will be replaced,
	/// otherwise they will be skipped.
	/// </summary>
	public void CopyTo(ExampleEntity copyToEntity, bool replaceExisting)
	{
		for (var i = 0; i < ExampleComponentsLookup.TotalComponents; ++i)
		{
			if (!HasComponent(i))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(i) || replaceExisting)
			{
				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies components on this entity at <paramref name="indices"/> in the <see cref="ExampleComponentsLookup"/> to
	/// <paramref name="copyToEntity"/>. If <paramref name="replaceExisting"/> is true any of the components that
	/// <paramref name="copyToEntity"/> has that this entity has will be replaced, otherwise they will be skipped.
	/// </summary>
	public void CopyTo(ExampleEntity copyToEntity, bool replaceExisting, params int[] indices)
	{
		for (var i = 0; i < indices.Length; ++i)
		{
			var index = indices[i];

			// Validate that the index is within range of the component lookup
			if (index < 0 && index >= ExampleComponentsLookup.TotalComponents)
			{
				const string OUT_OF_RANGE_WARNING =
					"Component Index [{0}] is out of range for [{1}].";

				const string HINT = "Please ensure any CopyTo indices are valid.";

				throw new IndexOutOfLookupRangeException(
					string.Format(OUT_OF_RANGE_WARNING, index, nameof(ExampleComponentsLookup)),
					HINT);
			}

			if (!HasComponent(index))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(index) || replaceExisting)
			{
				var component = GetComponent(index);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}
}
