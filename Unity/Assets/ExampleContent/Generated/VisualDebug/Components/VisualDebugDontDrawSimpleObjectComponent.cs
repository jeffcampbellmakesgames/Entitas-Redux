public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent DontDrawSimpleObject { get { return (ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent)GetComponent(VisualDebugComponentsLookup.DontDrawSimpleObject); } }
	public bool HasDontDrawSimpleObject { get { return HasComponent(VisualDebugComponentsLookup.DontDrawSimpleObject); } }

	public void AddDontDrawSimpleObject(ExampleContent.VisualDebugging.SimpleObject newSimpleObject)
	{
		var index = VisualDebugComponentsLookup.DontDrawSimpleObject;
		var component = (ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = newSimpleObject;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceDontDrawSimpleObject(ExampleContent.VisualDebugging.SimpleObject newSimpleObject)
	{
		var index = VisualDebugComponentsLookup.DontDrawSimpleObject;
		var component = (ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = newSimpleObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyDontDrawSimpleObjectTo(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.DontDrawSimpleObject;
		var component = (ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.simpleObject = copyComponent.simpleObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveDontDrawSimpleObject()
	{
		RemoveComponent(VisualDebugComponentsLookup.DontDrawSimpleObject);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherDontDrawSimpleObject;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> DontDrawSimpleObject
	{
		get
		{
			if (_matcherDontDrawSimpleObject == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.DontDrawSimpleObject);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherDontDrawSimpleObject = matcher;
			}

			return _matcherDontDrawSimpleObject;
		}
	}
}
