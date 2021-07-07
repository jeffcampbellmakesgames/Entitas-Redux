public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.ListArrayComponent ListArray { get { return (ExampleContent.VisualDebugging.ListArrayComponent)GetComponent(VisualDebugComponentsLookup.ListArray); } }
	public bool HasListArray { get { return HasComponent(VisualDebugComponentsLookup.ListArray); } }

	public void AddListArray(System.Collections.Generic.List<string>[] newListArray)
	{
		var index = VisualDebugComponentsLookup.ListArray;
		var component = (ExampleContent.VisualDebugging.ListArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.ListArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.listArray = newListArray;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceListArray(System.Collections.Generic.List<string>[] newListArray)
	{
		var index = VisualDebugComponentsLookup.ListArray;
		var component = (ExampleContent.VisualDebugging.ListArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.ListArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.listArray = newListArray;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyListArrayTo(ExampleContent.VisualDebugging.ListArrayComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.ListArray;
		var component = (ExampleContent.VisualDebugging.ListArrayComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.ListArrayComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.listArray = (System.Collections.Generic.List<string>[])copyComponent.listArray.Clone();
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveListArray()
	{
		RemoveComponent(VisualDebugComponentsLookup.ListArray);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherListArray;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> ListArray
	{
		get
		{
			if (_matcherListArray == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.ListArray);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherListArray = matcher;
			}

			return _matcherListArray;
		}
	}
}
