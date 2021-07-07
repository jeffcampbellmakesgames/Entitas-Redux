public partial class TestEntity
{
	public EntitasRedux.Tests.NameAgeComponent NameAge { get { return (EntitasRedux.Tests.NameAgeComponent)GetComponent(TestComponentsLookup.NameAge); } }
	public bool HasNameAge { get { return HasComponent(TestComponentsLookup.NameAge); } }

	public void AddNameAge(string newName, int newAge)
	{
		var index = TestComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = newName;
		component.age = newAge;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceNameAge(string newName, int newAge)
	{
		var index = TestComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = newName;
		component.age = newAge;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyNameAgeTo(EntitasRedux.Tests.NameAgeComponent copyComponent)
	{
		var index = TestComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = copyComponent.name;
		component.age = copyComponent.age;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveNameAge()
	{
		RemoveComponent(TestComponentsLookup.NameAge);
	}
}

public partial class TestEntity : INameAgeEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherNameAge;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> NameAge
	{
		get
		{
			if (_matcherNameAge == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.NameAge);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherNameAge = matcher;
			}

			return _matcherNameAge;
		}
	}
}
