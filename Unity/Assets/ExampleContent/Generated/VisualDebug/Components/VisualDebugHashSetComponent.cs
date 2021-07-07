public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.HashSetComponent HashSet { get { return (ExampleContent.VisualDebugging.HashSetComponent)GetComponent(VisualDebugComponentsLookup.HashSet); } }
	public bool HasHashSet { get { return HasComponent(VisualDebugComponentsLookup.HashSet); } }

	public void AddHashSet(System.Collections.Generic.HashSet<string> newHashset)
	{
		var index = VisualDebugComponentsLookup.HashSet;
		var component = (ExampleContent.VisualDebugging.HashSetComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.HashSetComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.hashset = newHashset;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceHashSet(System.Collections.Generic.HashSet<string> newHashset)
	{
		var index = VisualDebugComponentsLookup.HashSet;
		var component = (ExampleContent.VisualDebugging.HashSetComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.HashSetComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.hashset = newHashset;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyHashSetTo(ExampleContent.VisualDebugging.HashSetComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.HashSet;
		var component = (ExampleContent.VisualDebugging.HashSetComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.HashSetComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.hashset = copyComponent.hashset;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveHashSet()
	{
		RemoveComponent(VisualDebugComponentsLookup.HashSet);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherHashSet;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> HashSet
	{
		get
		{
			if (_matcherHashSet == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.HashSet);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherHashSet = matcher;
			}

			return _matcherHashSet;
		}
	}
}
