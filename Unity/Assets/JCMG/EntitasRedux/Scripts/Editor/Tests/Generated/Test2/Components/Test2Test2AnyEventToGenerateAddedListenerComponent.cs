public partial class Test2Entity
{
	public Test2AnyEventToGenerateAddedListenerComponent Test2AnyEventToGenerateAddedListener { get { return (Test2AnyEventToGenerateAddedListenerComponent)GetComponent(Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener); } }
	public bool HasTest2AnyEventToGenerateAddedListener { get { return HasComponent(Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener); } }

	public void AddTest2AnyEventToGenerateAddedListener(System.Collections.Generic.List<ITest2AnyEventToGenerateAddedListener> newValue)
	{
		var index = Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener;
		var component = (Test2AnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(Test2AnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTest2AnyEventToGenerateAddedListener(System.Collections.Generic.List<ITest2AnyEventToGenerateAddedListener> newValue)
	{
		var index = Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener;
		var component = (Test2AnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(Test2AnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTest2AnyEventToGenerateAddedListenerTo(Test2AnyEventToGenerateAddedListenerComponent copyComponent)
	{
		var index = Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener;
		var component = (Test2AnyEventToGenerateAddedListenerComponent)CreateComponent(index, typeof(Test2AnyEventToGenerateAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTest2AnyEventToGenerateAddedListener()
	{
		RemoveComponent(Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener);
	}
}

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherTest2AnyEventToGenerateAddedListener;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> Test2AnyEventToGenerateAddedListener
	{
		get
		{
			if (_matcherTest2AnyEventToGenerateAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.Test2AnyEventToGenerateAddedListener);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherTest2AnyEventToGenerateAddedListener = matcher;
			}

			return _matcherTest2AnyEventToGenerateAddedListener;
		}
	}
}

public partial class Test2Entity
{
	public void AddTest2AnyEventToGenerateAddedListener(ITest2AnyEventToGenerateAddedListener value)
	{
		var listeners = HasTest2AnyEventToGenerateAddedListener
			? Test2AnyEventToGenerateAddedListener.value
			: new System.Collections.Generic.List<ITest2AnyEventToGenerateAddedListener>();
		listeners.Add(value);
		ReplaceTest2AnyEventToGenerateAddedListener(listeners);
	}

	public void RemoveTest2AnyEventToGenerateAddedListener(ITest2AnyEventToGenerateAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = Test2AnyEventToGenerateAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveTest2AnyEventToGenerateAddedListener();
		}
		else
		{
			ReplaceTest2AnyEventToGenerateAddedListener(listeners);
		}
	}
}
