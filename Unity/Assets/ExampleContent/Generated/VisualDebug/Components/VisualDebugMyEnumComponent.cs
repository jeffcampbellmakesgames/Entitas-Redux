public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.MyEnumComponent MyEnum { get { return (ExampleContent.VisualDebugging.MyEnumComponent)GetComponent(VisualDebugComponentsLookup.MyEnum); } }
	public bool HasMyEnum { get { return HasComponent(VisualDebugComponentsLookup.MyEnum); } }

	public void AddMyEnum(ExampleContent.VisualDebugging.MyEnumComponent.MyEnum newMyEnum)
	{
		var index = VisualDebugComponentsLookup.MyEnum;
		var component = (ExampleContent.VisualDebugging.MyEnumComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyEnumComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myEnum = newMyEnum;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyEnum(ExampleContent.VisualDebugging.MyEnumComponent.MyEnum newMyEnum)
	{
		var index = VisualDebugComponentsLookup.MyEnum;
		var component = (ExampleContent.VisualDebugging.MyEnumComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyEnumComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myEnum = newMyEnum;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyEnumTo(ExampleContent.VisualDebugging.MyEnumComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.MyEnum;
		var component = (ExampleContent.VisualDebugging.MyEnumComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyEnumComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myEnum = copyComponent.myEnum;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyEnum()
	{
		RemoveComponent(VisualDebugComponentsLookup.MyEnum);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyEnum;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyEnum
	{
		get
		{
			if (_matcherMyEnum == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyEnum);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherMyEnum = matcher;
			}

			return _matcherMyEnum;
		}
	}
}
