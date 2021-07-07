
using JCMG.EntitasRedux;

public partial class Test2Entity
{
	/// <summary>
	/// Copies <paramref name="component"/> to this entity as a new component instance.
	/// </summary>
	public void CopyComponentTo(IComponent component)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		if (component is EntitasRedux.Tests.EntityIndexComponent EntityIndex)
		{
			CopyEntityIndexTo(EntityIndex);
		}
		else if (component is EntitasRedux.Tests.MultipleContextStandardEventComponent MultipleContextStandardEvent)
		{
			CopyMultipleContextStandardEventTo(MultipleContextStandardEvent);
		}
		else if (component is EntitasRedux.Tests.MultipleEntityIndicesComponent MultipleEntityIndices)
		{
			CopyMultipleEntityIndicesTo(MultipleEntityIndices);
		}
		else if (component is EntitasRedux.Tests.MultipleEventsStandardEventComponent MultipleEventsStandardEvent)
		{
			CopyMultipleEventsStandardEventTo(MultipleEventsStandardEvent);
		}
		else if (component is EntitasRedux.Tests.MyNamespaceComponent MyNamespace)
		{
			CopyMyNamespaceTo(MyNamespace);
		}
		else if (component is EntitasRedux.Tests.NameAgeComponent NameAge)
		{
			CopyNameAgeTo(NameAge);
		}
		else if (component is EntitasRedux.Tests.Test2ContextComponent Test2Context)
		{
			CopyTest2ContextTo(Test2Context);
		}
		else if (component is ClassToGenerateComponent ClassToGenerate)
		{
			CopyClassToGenerateTo(ClassToGenerate);
		}
		else if (component is EventToGenerateComponent EventToGenerate)
		{
			CopyEventToGenerateTo(EventToGenerate);
		}
		else if (component is UniqueClassToGenerateComponent UniqueClassToGenerate)
		{
			CopyUniqueClassToGenerateTo(UniqueClassToGenerate);
		}
		else if (component is Test2AnyMultipleContextStandardEventAddedListenerComponent Test2AnyMultipleContextStandardEventAddedListener)
		{
			CopyTest2AnyMultipleContextStandardEventAddedListenerTo(Test2AnyMultipleContextStandardEventAddedListener);
		}
		else if (component is Test2AnyMultipleEventsStandardEventAddedListenerComponent Test2AnyMultipleEventsStandardEventAddedListener)
		{
			CopyTest2AnyMultipleEventsStandardEventAddedListenerTo(Test2AnyMultipleEventsStandardEventAddedListener);
		}
		else if (component is Test2MultipleEventsStandardEventRemovedListenerComponent Test2MultipleEventsStandardEventRemovedListener)
		{
			CopyTest2MultipleEventsStandardEventRemovedListenerTo(Test2MultipleEventsStandardEventRemovedListener);
		}
		else if (component is Test2AnyEventToGenerateAddedListenerComponent Test2AnyEventToGenerateAddedListener)
		{
			CopyTest2AnyEventToGenerateAddedListenerTo(Test2AnyEventToGenerateAddedListener);
		}
		#endif
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>.
	/// </summary>
	public void CopyTo(Test2Entity copyToEntity)
	{
		for (var i = 0; i < Test2ComponentsLookup.TotalComponents; ++i)
		{
			if (HasComponent(i))
			{
				if (copyToEntity.HasComponent(i))
				{
					throw new EntityAlreadyHasComponentException(
						i,
						"Cannot copy component '" +
						Test2ComponentsLookup.ComponentNames[i] +
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
	public void CopyTo(Test2Entity copyToEntity, bool replaceExisting)
	{
		for (var i = 0; i < Test2ComponentsLookup.TotalComponents; ++i)
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
	/// Copies components on this entity at <paramref name="indices"/> in the <see cref="Test2ComponentsLookup"/> to
	/// <paramref name="copyToEntity"/>. If <paramref name="replaceExisting"/> is true any of the components that
	/// <paramref name="copyToEntity"/> has that this entity has will be replaced, otherwise they will be skipped.
	/// </summary>
	public void CopyTo(Test2Entity copyToEntity, bool replaceExisting, params int[] indices)
	{
		for (var i = 0; i < indices.Length; ++i)
		{
			var index = indices[i];

			// Validate that the index is within range of the component lookup
			if (index < 0 && index >= Test2ComponentsLookup.TotalComponents)
			{
				const string OUT_OF_RANGE_WARNING =
					"Component Index [{0}] is out of range for [{1}].";

				const string HINT = "Please ensure any CopyTo indices are valid.";

				throw new IndexOutOfLookupRangeException(
					string.Format(OUT_OF_RANGE_WARNING, index, nameof(Test2ComponentsLookup)),
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
