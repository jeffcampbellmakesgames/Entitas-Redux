public partial class TestEntity
{
	public TestAnyEventToGenerateAddedListenerComponent TestAnyEventToGenerateAddedListener { get { return (TestAnyEventToGenerateAddedListenerComponent)GetComponent(TestComponentsLookup.TestAnyEventToGenerateAddedListener); } }
	public bool HasTestAnyEventToGenerateAddedListener { get { return HasComponent(TestComponentsLookup.TestAnyEventToGenerateAddedListener); } }

	public void AddTestAnyEventToGenerateAddedListener(System.Collections.Generic.List<ITestAnyEventToGenerateAddedListener> newValue)
	{
		var index = TestComponentsLookup.TestAnyEventToGenerateAddedListener;
		var component = (TestAnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(TestAnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTestAnyEventToGenerateAddedListener(System.Collections.Generic.List<ITestAnyEventToGenerateAddedListener> newValue)
	{
		var index = TestComponentsLookup.TestAnyEventToGenerateAddedListener;
		var component = (TestAnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(TestAnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTestAnyEventToGenerateAddedListenerTo(TestAnyEventToGenerateAddedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.TestAnyEventToGenerateAddedListener;
		var component = (TestAnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(TestAnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTestAnyEventToGenerateAddedListener()
	{
		RemoveComponent(TestComponentsLookup.TestAnyEventToGenerateAddedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherTestAnyEventToGenerateAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> TestAnyEventToGenerateAddedListener
	{
		get
		{
			if (_matcherTestAnyEventToGenerateAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.TestAnyEventToGenerateAddedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherTestAnyEventToGenerateAddedListener = matcher;
			}

			return _matcherTestAnyEventToGenerateAddedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddTestAnyEventToGenerateAddedListener(ITestAnyEventToGenerateAddedListener value)
	{
		var listeners = HasTestAnyEventToGenerateAddedListener
			? TestAnyEventToGenerateAddedListener.value
			: new System.Collections.Generic.List<ITestAnyEventToGenerateAddedListener>();
		listeners.Add(value);
		ReplaceTestAnyEventToGenerateAddedListener(listeners);
	}

	public void RemoveTestAnyEventToGenerateAddedListener(ITestAnyEventToGenerateAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = TestAnyEventToGenerateAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveTestAnyEventToGenerateAddedListener();
		}
		else
		{
			ReplaceTestAnyEventToGenerateAddedListener(listeners);
		}
	}
}
