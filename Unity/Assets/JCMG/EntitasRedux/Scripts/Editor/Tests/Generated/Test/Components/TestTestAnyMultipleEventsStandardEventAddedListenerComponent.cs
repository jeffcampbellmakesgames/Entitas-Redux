public partial class TestEntity
{
	public TestAnyMultipleEventsStandardEventAddedListenerComponent TestAnyMultipleEventsStandardEventAddedListener { get { return (TestAnyMultipleEventsStandardEventAddedListenerComponent)GetComponent(TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener); } }
	public bool HasTestAnyMultipleEventsStandardEventAddedListener { get { return HasComponent(TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener); } }

	public void AddTestAnyMultipleEventsStandardEventAddedListener(System.Collections.Generic.List<ITestAnyMultipleEventsStandardEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener;
		var component = (TestAnyMultipleEventsStandardEventAddedListenerComponent)CreateComponent(index, typeof(TestAnyMultipleEventsStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTestAnyMultipleEventsStandardEventAddedListener(System.Collections.Generic.List<ITestAnyMultipleEventsStandardEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener;
		var component = (TestAnyMultipleEventsStandardEventAddedListenerComponent)CreateComponent(index, typeof(TestAnyMultipleEventsStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTestAnyMultipleEventsStandardEventAddedListenerTo(TestAnyMultipleEventsStandardEventAddedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener;
		var component = (TestAnyMultipleEventsStandardEventAddedListenerComponent)CreateComponent(index, typeof(TestAnyMultipleEventsStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTestAnyMultipleEventsStandardEventAddedListener()
	{
		RemoveComponent(TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherTestAnyMultipleEventsStandardEventAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> TestAnyMultipleEventsStandardEventAddedListener
	{
		get
		{
			if (_matcherTestAnyMultipleEventsStandardEventAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.TestAnyMultipleEventsStandardEventAddedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherTestAnyMultipleEventsStandardEventAddedListener = matcher;
			}

			return _matcherTestAnyMultipleEventsStandardEventAddedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddTestAnyMultipleEventsStandardEventAddedListener(ITestAnyMultipleEventsStandardEventAddedListener value)
	{
		var listeners = HasTestAnyMultipleEventsStandardEventAddedListener
			? TestAnyMultipleEventsStandardEventAddedListener.value
			: new System.Collections.Generic.List<ITestAnyMultipleEventsStandardEventAddedListener>();
		listeners.Add(value);
		ReplaceTestAnyMultipleEventsStandardEventAddedListener(listeners);
	}

	public void RemoveTestAnyMultipleEventsStandardEventAddedListener(ITestAnyMultipleEventsStandardEventAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = TestAnyMultipleEventsStandardEventAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveTestAnyMultipleEventsStandardEventAddedListener();
		}
		else
		{
			ReplaceTestAnyMultipleEventsStandardEventAddedListener(listeners);
		}
	}
}
