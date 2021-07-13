public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.IndexedPrimaryComponent IndexedPrimary { get { return (ExampleContent.VisualDebugging.IndexedPrimaryComponent)GetComponent(VisualDebugComponentsLookup.IndexedPrimary); } }
	public bool HasIndexedPrimary { get { return HasComponent(VisualDebugComponentsLookup.IndexedPrimary); } }

	public void AddIndexedPrimary(int newId)
	{
		var index = VisualDebugComponentsLookup.IndexedPrimary;
		var component = (ExampleContent.VisualDebugging.IndexedPrimaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedPrimaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = newId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceIndexedPrimary(int newId)
	{
		var index = VisualDebugComponentsLookup.IndexedPrimary;
		var component = (ExampleContent.VisualDebugging.IndexedPrimaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedPrimaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = newId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyIndexedPrimaryTo(ExampleContent.VisualDebugging.IndexedPrimaryComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.IndexedPrimary;
		var component = (ExampleContent.VisualDebugging.IndexedPrimaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedPrimaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = copyComponent.id;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveIndexedPrimary()
	{
		RemoveComponent(VisualDebugComponentsLookup.IndexedPrimary);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherIndexedPrimary;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> IndexedPrimary
	{
		get
		{
			if (_matcherIndexedPrimary == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.IndexedPrimary);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherIndexedPrimary = matcher;
			}

			return _matcherIndexedPrimary;
		}
	}
}
