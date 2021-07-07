public partial class Test2Entity
{
	public EntitasRedux.Tests.MultipleEventsStandardEventComponent MultipleEventsStandardEvent { get { return (EntitasRedux.Tests.MultipleEventsStandardEventComponent)GetComponent(Test2ComponentsLookup.MultipleEventsStandardEvent); } }
	public bool HasMultipleEventsStandardEvent { get { return HasComponent(Test2ComponentsLookup.MultipleEventsStandardEvent); } }

	public void AddMultipleEventsStandardEvent(string newValue)
	{
		var index = Test2ComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMultipleEventsStandardEvent(string newValue)
	{
		var index = Test2ComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMultipleEventsStandardEventTo(EntitasRedux.Tests.MultipleEventsStandardEventComponent copyComponent)
	{
		var index = Test2ComponentsLookup.MultipleEventsStandardEvent;
		var component = (EntitasRedux.Tests.MultipleEventsStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMultipleEventsStandardEvent()
	{
		RemoveComponent(Test2ComponentsLookup.MultipleEventsStandardEvent);
	}
}

public partial class Test2Entity : IMultipleEventsStandardEventEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherMultipleEventsStandardEvent;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> MultipleEventsStandardEvent
	{
		get
		{
			if (_matcherMultipleEventsStandardEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.MultipleEventsStandardEvent);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherMultipleEventsStandardEvent = matcher;
			}

			return _matcherMultipleEventsStandardEvent;
		}
	}
}
