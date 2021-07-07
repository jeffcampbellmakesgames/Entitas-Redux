public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.PropertyComponent Property { get { return (ExampleContent.VisualDebugging.PropertyComponent)GetComponent(VisualDebugComponentsLookup.Property); } }
	public bool HasProperty { get { return HasComponent(VisualDebugComponentsLookup.Property); } }

	public void AddProperty(string newValue)
	{
		var index = VisualDebugComponentsLookup.Property;
		var component = (ExampleContent.VisualDebugging.PropertyComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PropertyComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceProperty(string newValue)
	{
		var index = VisualDebugComponentsLookup.Property;
		var component = (ExampleContent.VisualDebugging.PropertyComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PropertyComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyPropertyTo(ExampleContent.VisualDebugging.PropertyComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Property;
		var component = (ExampleContent.VisualDebugging.PropertyComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.PropertyComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveProperty()
	{
		RemoveComponent(VisualDebugComponentsLookup.Property);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherProperty;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Property
	{
		get
		{
			if (_matcherProperty == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Property);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherProperty = matcher;
			}

			return _matcherProperty;
		}
	}
}
