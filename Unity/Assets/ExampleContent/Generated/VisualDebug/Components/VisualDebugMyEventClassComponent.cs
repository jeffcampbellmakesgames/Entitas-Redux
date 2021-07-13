public partial class VisualDebugEntity
{
	public MyEventClassComponent MyEventClass { get { return (MyEventClassComponent)GetComponent(VisualDebugComponentsLookup.MyEventClass); } }
	public bool HasMyEventClass { get { return HasComponent(VisualDebugComponentsLookup.MyEventClass); } }

	public void AddMyEventClass(ExampleContent.VisualDebugging.MyEventClass newValue)
	{
		var index = VisualDebugComponentsLookup.MyEventClass;
		var component = (MyEventClassComponent)CreateComponent(index, typeof(MyEventClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyEventClass(ExampleContent.VisualDebugging.MyEventClass newValue)
	{
		var index = VisualDebugComponentsLookup.MyEventClass;
		var component = (MyEventClassComponent)CreateComponent(index, typeof(MyEventClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyEventClassTo(MyEventClassComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.MyEventClass;
		var component = (MyEventClassComponent)CreateComponent(index, typeof(MyEventClassComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyEventClass()
	{
		RemoveComponent(VisualDebugComponentsLookup.MyEventClass);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyEventClass;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyEventClass
	{
		get
		{
			if (_matcherMyEventClass == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyEventClass);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherMyEventClass = matcher;
			}

			return _matcherMyEventClass;
		}
	}
}
