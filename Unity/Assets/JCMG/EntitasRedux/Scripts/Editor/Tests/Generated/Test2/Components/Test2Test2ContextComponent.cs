public partial class Test2Entity
{
	public EntitasRedux.Tests.Test2ContextComponent Test2Context { get { return (EntitasRedux.Tests.Test2ContextComponent)GetComponent(Test2ComponentsLookup.Test2Context); } }
	public bool HasTest2Context { get { return HasComponent(Test2ComponentsLookup.Test2Context); } }

	public void AddTest2Context(string newValue)
	{
		var index = Test2ComponentsLookup.Test2Context;
		var component = (EntitasRedux.Tests.Test2ContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.Test2ContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTest2Context(string newValue)
	{
		var index = Test2ComponentsLookup.Test2Context;
		var component = (EntitasRedux.Tests.Test2ContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.Test2ContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTest2ContextTo(EntitasRedux.Tests.Test2ContextComponent copyComponent)
	{
		var index = Test2ComponentsLookup.Test2Context;
		var component = (EntitasRedux.Tests.Test2ContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.Test2ContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTest2Context()
	{
		RemoveComponent(Test2ComponentsLookup.Test2Context);
	}
}

public partial class Test2Entity : ITest2ContextEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherTest2Context;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> Test2Context
	{
		get
		{
			if (_matcherTest2Context == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.Test2Context);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherTest2Context = matcher;
			}

			return _matcherTest2Context;
		}
	}
}
