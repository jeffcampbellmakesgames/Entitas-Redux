public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.PersonComponent Person { get { return (ExampleContent.VisualDebugging.PersonComponent)GetComponent(VisualDebugComponentsLookup.Person); } }
	public bool HasPerson { get { return HasComponent(VisualDebugComponentsLookup.Person); } }

	public void AddPerson(string newGender, string newName)
	{
		var index = VisualDebugComponentsLookup.Person;
		var component = (ExampleContent.VisualDebugging.PersonComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PersonComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gender = newGender;
		component.name = newName;
		#endif
		AddComponent(index, component);
	}

	public void ReplacePerson(string newGender, string newName)
	{
		var index = VisualDebugComponentsLookup.Person;
		var component = (ExampleContent.VisualDebugging.PersonComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PersonComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gender = newGender;
		component.name = newName;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyPersonTo(ExampleContent.VisualDebugging.PersonComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Person;
		var component = (ExampleContent.VisualDebugging.PersonComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PersonComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gender = copyComponent.gender;
		component.name = copyComponent.name;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemovePerson()
	{
		RemoveComponent(VisualDebugComponentsLookup.Person);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherPerson;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Person
	{
		get
		{
			if (_matcherPerson == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Person);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherPerson = matcher;
			}

			return _matcherPerson;
		}
	}
}
