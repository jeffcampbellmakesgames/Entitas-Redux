public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.CustomObjectListComponent CustomObjectList { get { return (ExampleContent.VisualDebugging.CustomObjectListComponent)GetComponent(VisualDebugComponentsLookup.CustomObjectList); } }
	public bool HasCustomObjectList { get { return HasComponent(VisualDebugComponentsLookup.CustomObjectList); } }

	public void AddCustomObjectList(System.Collections.Generic.List<ExampleContent.VisualDebugging.CustomObject> newValue)
	{
		var index = VisualDebugComponentsLookup.CustomObjectList;
		var component = (ExampleContent.VisualDebugging.CustomObjectListComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.CustomObjectListComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceCustomObjectList(System.Collections.Generic.List<ExampleContent.VisualDebugging.CustomObject> newValue)
	{
		var index = VisualDebugComponentsLookup.CustomObjectList;
		var component = (ExampleContent.VisualDebugging.CustomObjectListComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.CustomObjectListComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyCustomObjectListTo(ExampleContent.VisualDebugging.CustomObjectListComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.CustomObjectList;
		var component = (ExampleContent.VisualDebugging.CustomObjectListComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.CustomObjectListComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = (System.Collections.Generic.List<ExampleContent.VisualDebugging.CustomObject>)JCMG.EntitasRedux.ListTools.DeepCopy(copyComponent.value);
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveCustomObjectList()
	{
		RemoveComponent(VisualDebugComponentsLookup.CustomObjectList);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherCustomObjectList;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> CustomObjectList
	{
		get
		{
			if (_matcherCustomObjectList == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.CustomObjectList);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherCustomObjectList = matcher;
			}

			return _matcherCustomObjectList;
		}
	}
}
