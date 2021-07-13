public partial class TestEntity
{
	public EntitasRedux.Tests.ComponentWithFieldsAndProperties ComponentWithFieldsAndProperties { get { return (EntitasRedux.Tests.ComponentWithFieldsAndProperties)GetComponent(TestComponentsLookup.ComponentWithFieldsAndProperties); } }
	public bool HasComponentWithFieldsAndProperties { get { return HasComponent(TestComponentsLookup.ComponentWithFieldsAndProperties); } }

	public void AddComponentWithFieldsAndProperties(string newPublicField, string newPublicProperty)
	{
		var index = TestComponentsLookup.ComponentWithFieldsAndProperties;
		var component = (EntitasRedux.Tests.ComponentWithFieldsAndProperties)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFieldsAndProperties));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = newPublicField;
		component.publicProperty = newPublicProperty;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceComponentWithFieldsAndProperties(string newPublicField, string newPublicProperty)
	{
		var index = TestComponentsLookup.ComponentWithFieldsAndProperties;
		var component = (EntitasRedux.Tests.ComponentWithFieldsAndProperties)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFieldsAndProperties));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = newPublicField;
		component.publicProperty = newPublicProperty;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyComponentWithFieldsAndPropertiesTo(EntitasRedux.Tests.ComponentWithFieldsAndProperties copyComponent)
	{
		var index = TestComponentsLookup.ComponentWithFieldsAndProperties;
		var component = (EntitasRedux.Tests.ComponentWithFieldsAndProperties)CreateComponent(index, typeof(EntitasRedux.Tests.ComponentWithFieldsAndProperties));
		#if !ENTITAS_REDUX_NO_IMPL
		component.publicField = copyComponent.publicField;
		component.publicProperty = copyComponent.publicProperty;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveComponentWithFieldsAndProperties()
	{
		RemoveComponent(TestComponentsLookup.ComponentWithFieldsAndProperties);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherComponentWithFieldsAndProperties;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> ComponentWithFieldsAndProperties
	{
		get
		{
			if (_matcherComponentWithFieldsAndProperties == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.ComponentWithFieldsAndProperties);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherComponentWithFieldsAndProperties = matcher;
			}

			return _matcherComponentWithFieldsAndProperties;
		}
	}
}
