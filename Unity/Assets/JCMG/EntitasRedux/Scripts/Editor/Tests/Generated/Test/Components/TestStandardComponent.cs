public partial class TestEntity
{
	public EntitasRedux.Tests.StandardComponent Standard { get { return (EntitasRedux.Tests.StandardComponent)GetComponent(TestComponentsLookup.Standard); } }
	public bool HasStandard { get { return HasComponent(TestComponentsLookup.Standard); } }

	public void AddStandard(string newValue)
	{
		var index = TestComponentsLookup.Standard;
		var component = (EntitasRedux.Tests.StandardComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceStandard(string newValue)
	{
		var index = TestComponentsLookup.Standard;
		var component = (EntitasRedux.Tests.StandardComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyStandardTo(EntitasRedux.Tests.StandardComponent copyComponent)
	{
		var index = TestComponentsLookup.Standard;
		var component = (EntitasRedux.Tests.StandardComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveStandard()
	{
		RemoveComponent(TestComponentsLookup.Standard);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherStandard;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> Standard
	{
		get
		{
			if (_matcherStandard == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.Standard);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherStandard = matcher;
			}

			return _matcherStandard;
		}
	}
}
