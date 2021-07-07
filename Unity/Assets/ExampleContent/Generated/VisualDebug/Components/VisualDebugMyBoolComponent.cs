public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.MyBoolComponent MyBool { get { return (ExampleContent.VisualDebugging.MyBoolComponent)GetComponent(VisualDebugComponentsLookup.MyBool); } }
	public bool HasMyBool { get { return HasComponent(VisualDebugComponentsLookup.MyBool); } }

	public void AddMyBool(bool newMyBool)
	{
		var index = VisualDebugComponentsLookup.MyBool;
		var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myBool = newMyBool;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyBool(bool newMyBool)
	{
		var index = VisualDebugComponentsLookup.MyBool;
		var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myBool = newMyBool;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyBoolTo(ExampleContent.VisualDebugging.MyBoolComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.MyBool;
		var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myBool = copyComponent.myBool;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyBool()
	{
		RemoveComponent(VisualDebugComponentsLookup.MyBool);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyBool;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyBool
	{
		get
		{
			if (_matcherMyBool == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyBool);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherMyBool = matcher;
			}

			return _matcherMyBool;
		}
	}
}
