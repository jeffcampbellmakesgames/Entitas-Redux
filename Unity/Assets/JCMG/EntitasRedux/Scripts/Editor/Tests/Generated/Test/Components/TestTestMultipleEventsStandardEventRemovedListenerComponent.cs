public partial class TestEntity
{
	public TestMultipleEventsStandardEventRemovedListenerComponent TestMultipleEventsStandardEventRemovedListener { get { return (TestMultipleEventsStandardEventRemovedListenerComponent)GetComponent(TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener); } }
	public bool HasTestMultipleEventsStandardEventRemovedListener { get { return HasComponent(TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener); } }

	public void AddTestMultipleEventsStandardEventRemovedListener(System.Collections.Generic.List<ITestMultipleEventsStandardEventRemovedListener> newValue)
	{
		var index = TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener;
		var component = (TestMultipleEventsStandardEventRemovedListenerComponent)CreateComponent(index, typeof(TestMultipleEventsStandardEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTestMultipleEventsStandardEventRemovedListener(System.Collections.Generic.List<ITestMultipleEventsStandardEventRemovedListener> newValue)
	{
		var index = TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener;
		var component = (TestMultipleEventsStandardEventRemovedListenerComponent)CreateComponent(index, typeof(TestMultipleEventsStandardEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTestMultipleEventsStandardEventRemovedListenerTo(TestMultipleEventsStandardEventRemovedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener;
		var component = (TestMultipleEventsStandardEventRemovedListenerComponent)CreateComponent(index, typeof(TestMultipleEventsStandardEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTestMultipleEventsStandardEventRemovedListener()
	{
		RemoveComponent(TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherTestMultipleEventsStandardEventRemovedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> TestMultipleEventsStandardEventRemovedListener
	{
		get
		{
			if (_matcherTestMultipleEventsStandardEventRemovedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.TestMultipleEventsStandardEventRemovedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherTestMultipleEventsStandardEventRemovedListener = matcher;
			}

			return _matcherTestMultipleEventsStandardEventRemovedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddTestMultipleEventsStandardEventRemovedListener(ITestMultipleEventsStandardEventRemovedListener value)
	{
		var listeners = HasTestMultipleEventsStandardEventRemovedListener
			? TestMultipleEventsStandardEventRemovedListener.value
			: new System.Collections.Generic.List<ITestMultipleEventsStandardEventRemovedListener>();
		listeners.Add(value);
		ReplaceTestMultipleEventsStandardEventRemovedListener(listeners);
	}

	public void RemoveTestMultipleEventsStandardEventRemovedListener(ITestMultipleEventsStandardEventRemovedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = TestMultipleEventsStandardEventRemovedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveTestMultipleEventsStandardEventRemovedListener();
		}
		else
		{
			ReplaceTestMultipleEventsStandardEventRemovedListener(listeners);
		}
	}
}
