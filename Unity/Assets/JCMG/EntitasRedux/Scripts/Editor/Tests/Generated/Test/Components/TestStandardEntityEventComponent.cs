public partial class TestEntity
{
	public EntitasRedux.Tests.StandardEntityEventComponent StandardEntityEvent { get { return (EntitasRedux.Tests.StandardEntityEventComponent)GetComponent(TestComponentsLookup.StandardEntityEvent); } }
	public bool HasStandardEntityEvent { get { return HasComponent(TestComponentsLookup.StandardEntityEvent); } }

	public void AddStandardEntityEvent(string newValue)
	{
		var index = TestComponentsLookup.StandardEntityEvent;
		var component = (EntitasRedux.Tests.StandardEntityEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardEntityEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceStandardEntityEvent(string newValue)
	{
		var index = TestComponentsLookup.StandardEntityEvent;
		var component = (EntitasRedux.Tests.StandardEntityEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardEntityEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyStandardEntityEventTo(EntitasRedux.Tests.StandardEntityEventComponent copyComponent)
	{
		var index = TestComponentsLookup.StandardEntityEvent;
		var component = (EntitasRedux.Tests.StandardEntityEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.StandardEntityEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveStandardEntityEvent()
	{
		RemoveComponent(TestComponentsLookup.StandardEntityEvent);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherStandardEntityEvent;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> StandardEntityEvent
	{
		get
		{
			if (_matcherStandardEntityEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.StandardEntityEvent);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherStandardEntityEvent = matcher;
			}

			return _matcherStandardEntityEvent;
		}
	}
}
