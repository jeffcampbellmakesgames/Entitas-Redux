public partial class TestEntity
{
	public FlagEntityEventAddedListenerComponent FlagEntityEventAddedListener { get { return (FlagEntityEventAddedListenerComponent)GetComponent(TestComponentsLookup.FlagEntityEventAddedListener); } }
	public bool HasFlagEntityEventAddedListener { get { return HasComponent(TestComponentsLookup.FlagEntityEventAddedListener); } }

	public void AddFlagEntityEventAddedListener(System.Collections.Generic.List<IFlagEntityEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.FlagEntityEventAddedListener;
		var component = (FlagEntityEventAddedListenerComponent)CreateComponent(index, typeof(FlagEntityEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceFlagEntityEventAddedListener(System.Collections.Generic.List<IFlagEntityEventAddedListener> newValue)
	{
		var index = TestComponentsLookup.FlagEntityEventAddedListener;
		var component = (FlagEntityEventAddedListenerComponent)CreateComponent(index, typeof(FlagEntityEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyFlagEntityEventAddedListenerTo(FlagEntityEventAddedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.FlagEntityEventAddedListener;
		var component = (FlagEntityEventAddedListenerComponent)CreateComponent(index, typeof(FlagEntityEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveFlagEntityEventAddedListener()
	{
		RemoveComponent(TestComponentsLookup.FlagEntityEventAddedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherFlagEntityEventAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> FlagEntityEventAddedListener
	{
		get
		{
			if (_matcherFlagEntityEventAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.FlagEntityEventAddedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherFlagEntityEventAddedListener = matcher;
			}

			return _matcherFlagEntityEventAddedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddFlagEntityEventAddedListener(IFlagEntityEventAddedListener value)
	{
		var listeners = HasFlagEntityEventAddedListener
			? FlagEntityEventAddedListener.value
			: new System.Collections.Generic.List<IFlagEntityEventAddedListener>();
		listeners.Add(value);
		ReplaceFlagEntityEventAddedListener(listeners);
	}

	public void RemoveFlagEntityEventAddedListener(IFlagEntityEventAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = FlagEntityEventAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveFlagEntityEventAddedListener();
		}
		else
		{
			ReplaceFlagEntityEventAddedListener(listeners);
		}
	}
}
