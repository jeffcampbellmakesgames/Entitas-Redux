public partial class MyTestEntity
{
	public EntitasRedux.Tests.EntityIndexNoContextComponent EntityIndexNoContext { get { return (EntitasRedux.Tests.EntityIndexNoContextComponent)GetComponent(MyTestComponentsLookup.EntityIndexNoContext); } }
	public bool HasEntityIndexNoContext { get { return HasComponent(MyTestComponentsLookup.EntityIndexNoContext); } }

	public void AddEntityIndexNoContext(string newValue)
	{
		var index = MyTestComponentsLookup.EntityIndexNoContext;
		var component = (EntitasRedux.Tests.EntityIndexNoContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexNoContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceEntityIndexNoContext(string newValue)
	{
		var index = MyTestComponentsLookup.EntityIndexNoContext;
		var component = (EntitasRedux.Tests.EntityIndexNoContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexNoContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyEntityIndexNoContextTo(EntitasRedux.Tests.EntityIndexNoContextComponent copyComponent)
	{
		var index = MyTestComponentsLookup.EntityIndexNoContext;
		var component = (EntitasRedux.Tests.EntityIndexNoContextComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexNoContextComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveEntityIndexNoContext()
	{
		RemoveComponent(MyTestComponentsLookup.EntityIndexNoContext);
	}
}

public sealed partial class MyTestMatcher
{
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherEntityIndexNoContext;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> EntityIndexNoContext
	{
		get
		{
			if (_matcherEntityIndexNoContext == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.EntityIndexNoContext);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherEntityIndexNoContext = matcher;
			}

			return _matcherEntityIndexNoContext;
		}
	}
}
