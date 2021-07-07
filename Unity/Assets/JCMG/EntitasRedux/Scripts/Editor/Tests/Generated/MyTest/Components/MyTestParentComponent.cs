public partial class MyTestEntity
{
	public EntitasRedux.Tests.ParentComponent Parent { get { return (EntitasRedux.Tests.ParentComponent)GetComponent(MyTestComponentsLookup.Parent); } }
	public bool HasParent { get { return HasComponent(MyTestComponentsLookup.Parent); } }

	public void AddParent(float newValue)
	{
		var index = MyTestComponentsLookup.Parent;
		var component = (EntitasRedux.Tests.ParentComponent)CreateComponent(index, typeof(EntitasRedux.Tests.ParentComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceParent(float newValue)
	{
		var index = MyTestComponentsLookup.Parent;
		var component = (EntitasRedux.Tests.ParentComponent)CreateComponent(index, typeof(EntitasRedux.Tests.ParentComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyParentTo(EntitasRedux.Tests.ParentComponent copyComponent)
	{
		var index = MyTestComponentsLookup.Parent;
		var component = (EntitasRedux.Tests.ParentComponent)CreateComponent(index, typeof(EntitasRedux.Tests.ParentComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveParent()
	{
		RemoveComponent(MyTestComponentsLookup.Parent);
	}
}

public sealed partial class MyTestMatcher
{
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherParent;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> Parent
	{
		get
		{
			if (_matcherParent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.Parent);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherParent = matcher;
			}

			return _matcherParent;
		}
	}
}
