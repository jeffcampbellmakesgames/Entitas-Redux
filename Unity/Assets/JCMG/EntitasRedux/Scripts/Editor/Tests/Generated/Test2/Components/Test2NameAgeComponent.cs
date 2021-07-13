public partial class Test2Entity
{
	public EntitasRedux.Tests.NameAgeComponent NameAge { get { return (EntitasRedux.Tests.NameAgeComponent)GetComponent(Test2ComponentsLookup.NameAge); } }
	public bool HasNameAge { get { return HasComponent(Test2ComponentsLookup.NameAge); } }

	public void AddNameAge(string newName, int newAge)
	{
		var index = Test2ComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = newName;
		component.age = newAge;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceNameAge(string newName, int newAge)
	{
		var index = Test2ComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = newName;
		component.age = newAge;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyNameAgeTo(EntitasRedux.Tests.NameAgeComponent copyComponent)
	{
		var index = Test2ComponentsLookup.NameAge;
		var component = (EntitasRedux.Tests.NameAgeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.NameAgeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.name = copyComponent.name;
		component.age = copyComponent.age;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveNameAge()
	{
		RemoveComponent(Test2ComponentsLookup.NameAge);
	}
}

public partial class Test2Entity : INameAgeEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherNameAge;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> NameAge
	{
		get
		{
			if (_matcherNameAge == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.NameAge);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherNameAge = matcher;
			}

			return _matcherNameAge;
		}
	}
}
