public partial class VisualDebugEntity
{
	public SomeStructComponent SomeStruct { get { return (SomeStructComponent)GetComponent(VisualDebugComponentsLookup.SomeStruct); } }
	public bool HasSomeStruct { get { return HasComponent(VisualDebugComponentsLookup.SomeStruct); } }

	public void AddSomeStruct(ExampleContent.VisualDebugging.SomeStruct newValue)
	{
		var index = VisualDebugComponentsLookup.SomeStruct;
		var component = (SomeStructComponent)CreateComponent(index, typeof(SomeStructComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSomeStruct(ExampleContent.VisualDebugging.SomeStruct newValue)
	{
		var index = VisualDebugComponentsLookup.SomeStruct;
		var component = (SomeStructComponent)CreateComponent(index, typeof(SomeStructComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySomeStructTo(SomeStructComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.SomeStruct;
		var component = (SomeStructComponent)CreateComponent(index, typeof(SomeStructComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSomeStruct()
	{
		RemoveComponent(VisualDebugComponentsLookup.SomeStruct);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherSomeStruct;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> SomeStruct
	{
		get
		{
			if (_matcherSomeStruct == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.SomeStruct);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherSomeStruct = matcher;
			}

			return _matcherSomeStruct;
		}
	}
}
