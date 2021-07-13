public partial class TestEntity
{
	public EntitasRedux.Tests.MultipleEntityIndicesComponent MultipleEntityIndices { get { return (EntitasRedux.Tests.MultipleEntityIndicesComponent)GetComponent(TestComponentsLookup.MultipleEntityIndices); } }
	public bool HasMultipleEntityIndices { get { return HasComponent(TestComponentsLookup.MultipleEntityIndices); } }

	public void AddMultipleEntityIndices(string newValue, string newValue2)
	{
		var index = TestComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		component.value2 = newValue2;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMultipleEntityIndices(string newValue, string newValue2)
	{
		var index = TestComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		component.value2 = newValue2;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMultipleEntityIndicesTo(EntitasRedux.Tests.MultipleEntityIndicesComponent copyComponent)
	{
		var index = TestComponentsLookup.MultipleEntityIndices;
		var component = (EntitasRedux.Tests.MultipleEntityIndicesComponent)CreateComponent(index, typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		component.value2 = copyComponent.value2;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMultipleEntityIndices()
	{
		RemoveComponent(TestComponentsLookup.MultipleEntityIndices);
	}
}

public partial class TestEntity : IMultipleEntityIndicesEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMultipleEntityIndices;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MultipleEntityIndices
	{
		get
		{
			if (_matcherMultipleEntityIndices == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MultipleEntityIndices);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMultipleEntityIndices = matcher;
			}

			return _matcherMultipleEntityIndices;
		}
	}
}
