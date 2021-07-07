public partial class TestEntity
{
	public AnyFlagEventRemovedListenerComponent AnyFlagEventRemovedListener { get { return (AnyFlagEventRemovedListenerComponent)GetComponent(TestComponentsLookup.AnyFlagEventRemovedListener); } }
	public bool HasAnyFlagEventRemovedListener { get { return HasComponent(TestComponentsLookup.AnyFlagEventRemovedListener); } }

	public void AddAnyFlagEventRemovedListener(System.Collections.Generic.List<IAnyFlagEventRemovedListener> newValue)
	{
		var index = TestComponentsLookup.AnyFlagEventRemovedListener;
		var component = (AnyFlagEventRemovedListenerComponent)CreateComponent(index, typeof(AnyFlagEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceAnyFlagEventRemovedListener(System.Collections.Generic.List<IAnyFlagEventRemovedListener> newValue)
	{
		var index = TestComponentsLookup.AnyFlagEventRemovedListener;
		var component = (AnyFlagEventRemovedListenerComponent)CreateComponent(index, typeof(AnyFlagEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyAnyFlagEventRemovedListenerTo(AnyFlagEventRemovedListenerComponent copyComponent)
	{
		var index = TestComponentsLookup.AnyFlagEventRemovedListener;
		var component = (AnyFlagEventRemovedListenerComponent)CreateComponent(index, typeof(AnyFlagEventRemovedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveAnyFlagEventRemovedListener()
	{
		RemoveComponent(TestComponentsLookup.AnyFlagEventRemovedListener);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherAnyFlagEventRemovedListener;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> AnyFlagEventRemovedListener
	{
		get
		{
			if (_matcherAnyFlagEventRemovedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.AnyFlagEventRemovedListener);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherAnyFlagEventRemovedListener = matcher;
			}

			return _matcherAnyFlagEventRemovedListener;
		}
	}
}

public partial class TestEntity
{
	public void AddAnyFlagEventRemovedListener(IAnyFlagEventRemovedListener value)
	{
		var listeners = HasAnyFlagEventRemovedListener
			? AnyFlagEventRemovedListener.value
			: new System.Collections.Generic.List<IAnyFlagEventRemovedListener>();
		listeners.Add(value);
		ReplaceAnyFlagEventRemovedListener(listeners);
	}

	public void RemoveAnyFlagEventRemovedListener(IAnyFlagEventRemovedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = AnyFlagEventRemovedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveAnyFlagEventRemovedListener();
		}
		else
		{
			ReplaceAnyFlagEventRemovedListener(listeners);
		}
	}
}
