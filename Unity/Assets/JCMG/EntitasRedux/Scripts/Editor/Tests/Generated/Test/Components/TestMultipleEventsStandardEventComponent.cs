public partial class TestEntity
{
	public EntitasRedux.Tests.MultipleEventsStandardEventComponent MultipleEventsStandardEvent { get { return (EntitasRedux.Tests.MultipleEventsStandardEventComponent)GetComponent(TestComponentsLookup.MultipleEventsStandardEvent); } }
	public bool HasMultipleEventsStandardEvent { get { return HasComponent(TestComponentsLookup.MultipleEventsStandardEvent); } }

	public void AddMultipleEventsStandardEvent(string newValue)
	{
		var index = TestComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMultipleEventsStandardEvent(string newValue)
	{
		var index = TestComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMultipleEventsStandardEventTo(EntitasRedux.Tests.MultipleEventsStandardEventComponent copyComponent)
	{
		var index = TestComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMultipleEventsStandardEvent()
	{
		RemoveComponent(TestComponentsLookup.MultipleEventsStandardEvent);
	}
}

public partial class TestEntity : IMultipleEventsStandardEventEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMultipleEventsStandardEvent;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MultipleEventsStandardEvent
	{
		get
		{
			if (_matcherMultipleEventsStandardEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MultipleEventsStandardEvent);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMultipleEventsStandardEvent = matcher;
			}

			return _matcherMultipleEventsStandardEvent;
		}
	}
}
