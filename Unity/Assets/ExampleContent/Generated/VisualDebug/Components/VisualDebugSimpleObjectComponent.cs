public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.SimpleObjectComponent SimpleObject { get { return (ExampleContent.VisualDebugging.SimpleObjectComponent)GetComponent(VisualDebugComponentsLookup.SimpleObject); } }
	public bool HasSimpleObject { get { return HasComponent(VisualDebugComponentsLookup.SimpleObject); } }

	public void AddSimpleObject(ExampleContent.VisualDebugging.SimpleObject newSimpleObject)
	{
		var index = VisualDebugComponentsLookup.SimpleObject;
		var component = (ExampleContent.VisualDebugging.SimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = newSimpleObject;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSimpleObject(ExampleContent.VisualDebugging.SimpleObject newSimpleObject)
	{
		var index = VisualDebugComponentsLookup.SimpleObject;
		var component = (ExampleContent.VisualDebugging.SimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = newSimpleObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySimpleObjectTo(ExampleContent.VisualDebugging.SimpleObjectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.SimpleObject;
		var component = (ExampleContent.VisualDebugging.SimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = copyComponent.simpleObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSimpleObject()
	{
		RemoveComponent(VisualDebugComponentsLookup.SimpleObject);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherSimpleObject;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> SimpleObject
	{
		get
		{
			if (_matcherSimpleObject == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.SimpleObject);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherSimpleObject = matcher;
			}

			return _matcherSimpleObject;
		}
	}
}
