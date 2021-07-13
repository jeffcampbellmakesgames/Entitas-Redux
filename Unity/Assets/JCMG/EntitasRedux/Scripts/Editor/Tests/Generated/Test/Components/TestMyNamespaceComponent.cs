public partial class TestEntity
{
	public EntitasRedux.Tests.MyNamespaceComponent MyNamespace { get { return (EntitasRedux.Tests.MyNamespaceComponent)GetComponent(TestComponentsLookup.MyNamespace); } }
	public bool HasMyNamespace { get { return HasComponent(TestComponentsLookup.MyNamespace); } }

	public void AddMyNamespace(string newValue)
	{
		var index = TestComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyNamespace(string newValue)
	{
		var index = TestComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyNamespaceTo(EntitasRedux.Tests.MyNamespaceComponent copyComponent)
	{
		var index = TestComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyNamespace()
	{
		RemoveComponent(TestComponentsLookup.MyNamespace);
	}
}

public partial class TestEntity : IMyNamespaceEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMyNamespace;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MyNamespace
	{
		get
		{
			if (_matcherMyNamespace == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MyNamespace);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMyNamespace = matcher;
			}

			return _matcherMyNamespace;
		}
	}
}
