public partial class Test2Entity
{
	public EntitasRedux.Tests.MultipleEntityIndicesComponent MultipleEntityIndices { get { return (EntitasRedux.Tests.MultipleEntityIndicesComponent)GetComponent(Test2ComponentsLookup.MultipleEntityIndices); } }
	public bool HasMultipleEntityIndices { get { return HasComponent(Test2ComponentsLookup.MultipleEntityIndices); } }

	public void AddMultipleEntityIndices(string newValue, string newValue2)
	{
		var index = Test2ComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		component.value2 = newValue2;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMultipleEntityIndices(string newValue, string newValue2)
	{
		var index = Test2ComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		component.value2 = newValue2;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMultipleEntityIndicesTo(EntitasRedux.Tests.MultipleEntityIndicesComponent copyComponent)
	{
		var index = Test2ComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		component.value2 = copyComponent.value2;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMultipleEntityIndices()
	{
		RemoveComponent(Test2ComponentsLookup.MultipleEntityIndices);
	}
}

public partial class Test2Entity : IMultipleEntityIndicesEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherMultipleEntityIndices;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> MultipleEntityIndices
	{
		get
		{
			if (_matcherMultipleEntityIndices == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.MultipleEntityIndices);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherMultipleEntityIndices = matcher;
			}

			return _matcherMultipleEntityIndices;
		}
	}
}
