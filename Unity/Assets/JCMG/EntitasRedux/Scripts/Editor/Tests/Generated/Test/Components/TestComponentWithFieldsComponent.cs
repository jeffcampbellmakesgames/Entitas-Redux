public partial class TestEntity
{
	public EntitasRedux.Tests.ComponentWithFields ComponentWithFields { get { return (EntitasRedux.Tests.ComponentWithFields)GetComponent(TestComponentsLookup.ComponentWithFields); } }
	public bool HasComponentWithFields { get { return HasComponent(TestComponentsLookup.ComponentWithFields); } }

	public void AddComponentWithFields(string newPublicField)
	{
		var index = TestComponentsLookup.ComponentWithFields;
		var component = (EntitasRedux.Tests.ComponentWithFields)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFields));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = newPublicField;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceComponentWithFields(string newPublicField)
	{
		var index = TestComponentsLookup.ComponentWithFields;
		var component = (EntitasRedux.Tests.ComponentWithFields)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFields));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = newPublicField;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyComponentWithFieldsTo(EntitasRedux.Tests.ComponentWithFields copyComponent)
	{
		var index = TestComponentsLookup.ComponentWithFields;
		var component = (EntitasRedux.Tests.ComponentWithFields)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFields));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = copyComponent.publicField;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveComponentWithFields()
	{
		RemoveComponent(TestComponentsLookup.ComponentWithFields);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherComponentWithFields;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> ComponentWithFields
	{
		get
		{
			if (_matcherComponentWithFields == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.ComponentWithFields);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherComponentWithFields = matcher;
			}

			return _matcherComponentWithFields;
		}
	}
}
