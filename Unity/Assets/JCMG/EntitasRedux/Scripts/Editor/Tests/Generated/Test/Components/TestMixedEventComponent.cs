public partial class TestEntity
{
	public EntitasRedux.Tests.MixedEventComponent MixedEvent { get { return (EntitasRedux.Tests.MixedEventComponent)GetComponent(TestComponentsLookup.MixedEvent); } }
	public bool HasMixedEvent { get { return HasComponent(TestComponentsLookup.MixedEvent); } }

	public void AddMixedEvent(string newValue)
	{
		var index = TestComponentsLookup.MixedEvent;
		var component = (EntitasRedux.Tests.MixedEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MixedEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMixedEvent(string newValue)
	{
		var index = TestComponentsLookup.MixedEvent;
		var component = (EntitasRedux.Tests.MixedEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MixedEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMixedEventTo(EntitasRedux.Tests.MixedEventComponent copyComponent)
	{
		var index = TestComponentsLookup.MixedEvent;
		var component = (EntitasRedux.Tests.MixedEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MixedEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMixedEvent()
	{
		RemoveComponent(TestComponentsLookup.MixedEvent);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMixedEvent;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MixedEvent
	{
		get
		{
			if (_matcherMixedEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MixedEvent);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMixedEvent = matcher;
			}

			return _matcherMixedEvent;
		}
	}
}
