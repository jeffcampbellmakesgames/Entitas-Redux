public partial class TestEntity
{
	public AnyStandardEventAddedListenerComponent AnyStandardEventAddedListener { get { return (AnyStandardEventAddedListenerComponent)GetComponent(TestComponentsLookup.AnyStandardEventAddedListener); } }
	public bool HasAnyStandardEventAddedListener { get { return HasComponent(TestComponentsLookup.AnyStandardEventAddedListener); } }

	public void AddAnyStandardEventAddedListener(System.Collections.Generic.List<IAnyStandardEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.AnyStandardEventAddedListener;
		var component = (AnyStandardEventAddedListenerComponent)CreateComponent(index, typeof(AnyStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceAnyStandardEventAddedListener(System.Collections.Generic.List<IAnyStandardEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.AnyStandardEventAddedListener;
		var component = (AnyStandardEventAddedListenerComponent)CreateComponent(index, typeof(AnyStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyAnyStandardEventAddedListenerTo(AnyStandardEventAddedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.AnyStandardEventAddedListener;
		var component = (AnyStandardEventAddedListenerComponent)CreateComponent(index, typeof(AnyStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveAnyStandardEventAddedListener()
	{
		RemoveComponent(TestComponentsLookup.AnyStandardEventAddedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherAnyStandardEventAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> AnyStandardEventAddedListener
	{
		get
		{
			if (_matcherAnyStandardEventAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.AnyStandardEventAddedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherAnyStandardEventAddedListener = matcher;
			}

			return _matcherAnyStandardEventAddedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddAnyStandardEventAddedListener(IAnyStandardEventAddedListener value)
	{
		var listeners = HasAnyStandardEventAddedListener
			? AnyStandardEventAddedListener.value
			: new System.Collections.Generic.List<IAnyStandardEventAddedListener>();
		listeners.Add(value);
		ReplaceAnyStandardEventAddedListener(listeners);
	}

	public void RemoveAnyStandardEventAddedListener(IAnyStandardEventAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = AnyStandardEventAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveAnyStandardEventAddedListener();
		}
		else
		{
			ReplaceAnyStandardEventAddedListener(listeners);
		}
	}
}
