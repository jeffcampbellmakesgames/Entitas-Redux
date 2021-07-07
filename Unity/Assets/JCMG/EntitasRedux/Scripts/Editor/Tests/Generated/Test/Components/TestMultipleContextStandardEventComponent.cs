public partial class TestEntity
{
	public EntitasRedux.Tests.MultipleContextStandardEventComponent MultipleContextStandardEvent { get { return (EntitasRedux.Tests.MultipleContextStandardEventComponent)GetComponent(TestComponentsLookup.MultipleContextStandardEvent); } }
	public bool HasMultipleContextStandardEvent { get { return HasComponent(TestComponentsLookup.MultipleContextStandardEvent); } }

	public void AddMultipleContextStandardEvent(string newValue)
	{
		var index = TestComponentsLookup.MultipleContextStandardEvent;
		var component = (EntitasRedux.Tests.MultipleContextStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleContextStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMultipleContextStandardEvent(string newValue)
	{
		var index = TestComponentsLookup.MultipleContextStandardEvent;
		var component = (EntitasRedux.Tests.MultipleContextStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleContextStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMultipleContextStandardEventTo(EntitasRedux.Tests.MultipleContextStandardEventComponent copyComponent)
	{
		var index = TestComponentsLookup.MultipleContextStandardEvent;
		var component = (EntitasRedux.Tests.MultipleContextStandardEventComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleContextStandardEventComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMultipleContextStandardEvent()
	{
		RemoveComponent(TestComponentsLookup.MultipleContextStandardEvent);
	}
}

public partial class TestEntity : IMultipleContextStandardEventEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMultipleContextStandardEvent;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MultipleContextStandardEvent
	{
		get
		{
			if (_matcherMultipleContextStandardEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MultipleContextStandardEvent);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMultipleContextStandardEvent = matcher;
			}

			return _matcherMultipleContextStandardEvent;
		}
	}
}
