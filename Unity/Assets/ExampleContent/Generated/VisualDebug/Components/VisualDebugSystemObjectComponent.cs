public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.SystemObjectComponent SystemObject { get { return (ExampleContent.VisualDebugging.SystemObjectComponent)GetComponent(VisualDebugComponentsLookup.SystemObject); } }
	public bool HasSystemObject { get { return HasComponent(VisualDebugComponentsLookup.SystemObject); } }

	public void AddSystemObject(object newSystemObject)
	{
		var index = VisualDebugComponentsLookup.SystemObject;
		var component = (ExampleContent.VisualDebugging.SystemObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SystemObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.systemObject = newSystemObject;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSystemObject(object newSystemObject)
	{
		var index = VisualDebugComponentsLookup.SystemObject;
		var component = (ExampleContent.VisualDebugging.SystemObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SystemObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.systemObject = newSystemObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySystemObjectTo(ExampleContent.VisualDebugging.SystemObjectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.SystemObject;
		var component = (ExampleContent.VisualDebugging.SystemObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.SystemObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.systemObject = copyComponent.systemObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSystemObject()
	{
		RemoveComponent(VisualDebugComponentsLookup.SystemObject);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherSystemObject;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> SystemObject
	{
		get
		{
			if (_matcherSystemObject == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.SystemObject);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherSystemObject = matcher;
			}

			return _matcherSystemObject;
		}
	}
}
