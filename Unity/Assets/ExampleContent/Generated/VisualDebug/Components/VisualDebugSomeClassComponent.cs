public partial class VisualDebugEntity
{
	public SomeClassComponent SomeClass { get { return (SomeClassComponent)GetComponent(VisualDebugComponentsLookup.SomeClass); } }
	public bool HasSomeClass { get { return HasComponent(VisualDebugComponentsLookup.SomeClass); } }

	public void AddSomeClass(ExampleContent.VisualDebugging.SomeClass newValue)
	{
		var index = VisualDebugComponentsLookup.SomeClass;
		var component = (SomeClassComponent)CreateComponent(index, typeof(SomeClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSomeClass(ExampleContent.VisualDebugging.SomeClass newValue)
	{
		var index = VisualDebugComponentsLookup.SomeClass;
		var component = (SomeClassComponent)CreateComponent(index, typeof(SomeClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySomeClassTo(SomeClassComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.SomeClass;
		var component = (SomeClassComponent)CreateComponent(index, typeof(SomeClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = (ExampleContent.VisualDebugging.SomeClass)copyComponent.value.Clone();
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSomeClass()
	{
		RemoveComponent(VisualDebugComponentsLookup.SomeClass);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherSomeClass;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> SomeClass
	{
		get
		{
			if (_matcherSomeClass == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.SomeClass);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherSomeClass = matcher;
			}

			return _matcherSomeClass;
		}
	}
}
