public partial class Test2Entity
{
	public EntitasRedux.Tests.EntityIndexComponent EntityIndex { get { return (EntitasRedux.Tests.EntityIndexComponent)GetComponent(Test2ComponentsLookup.EntityIndex); } }
	public bool HasEntityIndex { get { return HasComponent(Test2ComponentsLookup.EntityIndex); } }

	public void AddEntityIndex(string newValue)
	{
		var index = Test2ComponentsLookup.EntityIndex;
		var component = (EntitasRedux.Tests.EntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceEntityIndex(string newValue)
	{
		var index = Test2ComponentsLookup.EntityIndex;
		var component = (EntitasRedux.Tests.EntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyEntityIndexTo(EntitasRedux.Tests.EntityIndexComponent copyComponent)
	{
		var index = Test2ComponentsLookup.EntityIndex;
		var component = (EntitasRedux.Tests.EntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.EntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveEntityIndex()
	{
		RemoveComponent(Test2ComponentsLookup.EntityIndex);
	}
}

public partial class Test2Entity : IEntityIndexEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherEntityIndex;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> EntityIndex
	{
		get
		{
			if (_matcherEntityIndex == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.EntityIndex);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherEntityIndex = matcher;
			}

			return _matcherEntityIndex;
		}
	}
}
