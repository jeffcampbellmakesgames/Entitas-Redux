public partial class Test2Entity
{
	public Test2AnyMultipleContextStandardEventAddedListenerComponent Test2AnyMultipleContextStandardEventAddedListener { get { return (Test2AnyMultipleContextStandardEventAddedListenerComponent)GetComponent(Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener); } }
	public bool HasTest2AnyMultipleContextStandardEventAddedListener { get { return HasComponent(Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener); } }

	public void AddTest2AnyMultipleContextStandardEventAddedListener(System.Collections.Generic.List<ITest2AnyMultipleContextStandardEventAddedListener> newValue)
	{
		var index = Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener;
		var component = (Test2AnyMultipleContextStandardEventAddedListenerComponent)CreateComponent(index, typeof(Test2AnyMultipleContextStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTest2AnyMultipleContextStandardEventAddedListener(System.Collections.Generic.List<ITest2AnyMultipleContextStandardEventAddedListener> newValue)
	{
		var index = Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener;
		var component = (Test2AnyMultipleContextStandardEventAddedListenerComponent)CreateComponent(index, typeof(Test2AnyMultipleContextStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTest2AnyMultipleContextStandardEventAddedListenerTo(Test2AnyMultipleContextStandardEventAddedListenerComponent copyComponent)
	{
		var index = Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener;
		var component = (Test2AnyMultipleContextStandardEventAddedListenerComponent)CreateComponent(index, typeof(Test2AnyMultipleContextStandardEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTest2AnyMultipleContextStandardEventAddedListener()
	{
		RemoveComponent(Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener);
	}
}

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherTest2AnyMultipleContextStandardEventAddedListener;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> Test2AnyMultipleContextStandardEventAddedListener
	{
		get
		{
			if (_matcherTest2AnyMultipleContextStandardEventAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.Test2AnyMultipleContextStandardEventAddedListener);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherTest2AnyMultipleContextStandardEventAddedListener = matcher;
			}

			return _matcherTest2AnyMultipleContextStandardEventAddedListener;
		}
	}
}

public partial class Test2Entity
{
	public void AddTest2AnyMultipleContextStandardEventAddedListener(ITest2AnyMultipleContextStandardEventAddedListener value)
	{
		var listeners = HasTest2AnyMultipleContextStandardEventAddedListener
			? Test2AnyMultipleContextStandardEventAddedListener.value
			: new System.Collections.Generic.List<ITest2AnyMultipleContextStandardEventAddedListener>();
		listeners.Add(value);
		ReplaceTest2AnyMultipleContextStandardEventAddedListener(listeners);
	}

	public void RemoveTest2AnyMultipleContextStandardEventAddedListener(ITest2AnyMultipleContextStandardEventAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = Test2AnyMultipleContextStandardEventAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveTest2AnyMultipleContextStandardEventAddedListener();
		}
		else
		{
			ReplaceTest2AnyMultipleContextStandardEventAddedListener(listeners);
		}
	}
}
