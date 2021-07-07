public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.MyStringComponent MyString { get { return (ExampleContent.VisualDebugging.MyStringComponent)GetComponent(VisualDebugComponentsLookup.MyString); } }
	public bool HasMyString { get { return HasComponent(VisualDebugComponentsLookup.MyString); } }

	public void AddMyString(string newMyString)
	{
		var index = VisualDebugComponentsLookup.MyString;
		var component = (ExampleContent.VisualDebugging.MyStringComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyStringComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myString = newMyString;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMyString(string newMyString)
	{
		var index = VisualDebugComponentsLookup.MyString;
		var component = (ExampleContent.VisualDebugging.MyStringComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyStringComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myString = newMyString;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMyStringTo(ExampleContent.VisualDebugging.MyStringComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.MyString;
		var component = (ExampleContent.VisualDebugging.MyStringComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyStringComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.myString = copyComponent.myString;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMyString()
	{
		RemoveComponent(VisualDebugComponentsLookup.MyString);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyString;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyString
	{
		get
		{
			if (_matcherMyString == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyString);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherMyString = matcher;
			}

			return _matcherMyString;
		}
	}
}
