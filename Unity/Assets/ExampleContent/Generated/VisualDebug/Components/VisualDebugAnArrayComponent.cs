public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.AnArrayComponent AnArray { get { return (ExampleContent.VisualDebugging.AnArrayComponent)GetComponent(VisualDebugComponentsLookup.AnArray); } }
	public bool HasAnArray { get { return HasComponent(VisualDebugComponentsLookup.AnArray); } }

	public void AddAnArray(string[] newArray)
	{
		var index = VisualDebugComponentsLookup.AnArray;
		var component = (ExampleContent.VisualDebugging.AnArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array = newArray;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceAnArray(string[] newArray)
	{
		var index = VisualDebugComponentsLookup.AnArray;
		var component = (ExampleContent.VisualDebugging.AnArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array = newArray;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyAnArrayTo(ExampleContent.VisualDebugging.AnArrayComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.AnArray;
		var component = (ExampleContent.VisualDebugging.AnArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array = (string[])JCMG.EntitasRedux.ArrayTools.DeepCopy(copyComponent.array);
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveAnArray()
	{
		RemoveComponent(VisualDebugComponentsLookup.AnArray);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherAnArray;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> AnArray
	{
		get
		{
			if (_matcherAnArray == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.AnArray);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherAnArray = matcher;
			}

			return _matcherAnArray;
		}
	}
}
