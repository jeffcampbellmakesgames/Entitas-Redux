public partial class Test2Entity
{
	public EntitasRedux.Tests.MyNamespaceComponent MyNamespace { get { return (EntitasRedux.Tests.MyNamespaceComponent)GetComponent(Test2ComponentsLookup.MyNamespace); } }
	public bool HasMyNamespace { get { return HasComponent(Test2ComponentsLookup.MyNamespace); } }

	public void AddMyNamespace(string newValue)
	{
		var index = Test2ComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyNamespace(string newValue)
	{
		var index = Test2ComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyNamespaceTo(EntitasRedux.Tests.MyNamespaceComponent copyComponent)
	{
		var index = Test2ComponentsLookup.MyNamespace;
		var component = (EntitasRedux.Tests.MyNamespaceComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MyNamespaceComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyNamespace()
	{
		RemoveComponent(Test2ComponentsLookup.MyNamespace);
	}
}

public partial class Test2Entity : IMyNamespaceEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherMyNamespace;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> MyNamespace
	{
		get
		{
			if (_matcherMyNamespace == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.MyNamespace);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherMyNamespace = matcher;
			}

			return _matcherMyNamespace;
		}
	}
}
