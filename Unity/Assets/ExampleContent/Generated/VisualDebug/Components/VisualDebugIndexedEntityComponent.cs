public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.IndexedEntityComponent IndexedEntity { get { return (ExampleContent.VisualDebugging.IndexedEntityComponent)GetComponent(VisualDebugComponentsLookup.IndexedEntity); } }
	public bool HasIndexedEntity { get { return HasComponent(VisualDebugComponentsLookup.IndexedEntity); } }

	public void AddIndexedEntity(int newId)
	{
		var index = VisualDebugComponentsLookup.IndexedEntity;
		var component = (ExampleContent.VisualDebugging.IndexedEntityComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedEntityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = newId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceIndexedEntity(int newId)
	{
		var index = VisualDebugComponentsLookup.IndexedEntity;
		var component = (ExampleContent.VisualDebugging.IndexedEntityComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedEntityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = newId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyIndexedEntityTo(ExampleContent.VisualDebugging.IndexedEntityComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.IndexedEntity;
		var component = (ExampleContent.VisualDebugging.IndexedEntityComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.IndexedEntityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.id = copyComponent.id;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveIndexedEntity()
	{
		RemoveComponent(VisualDebugComponentsLookup.IndexedEntity);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherIndexedEntity;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> IndexedEntity
	{
		get
		{
			if (_matcherIndexedEntity == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.IndexedEntity);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherIndexedEntity = matcher;
			}

			return _matcherIndexedEntity;
		}
	}
}
