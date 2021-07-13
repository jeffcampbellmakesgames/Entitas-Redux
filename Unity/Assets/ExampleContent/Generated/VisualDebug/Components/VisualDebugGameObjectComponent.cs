public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.GameObjectComponent GameObject { get { return (ExampleContent.VisualDebugging.GameObjectComponent)GetComponent(VisualDebugComponentsLookup.GameObject); } }
	public bool HasGameObject { get { return HasComponent(VisualDebugComponentsLookup.GameObject); } }

	public void AddGameObject(UnityEngine.GameObject newGameObject)
	{
		var index = VisualDebugComponentsLookup.GameObject;
		var component = (ExampleContent.VisualDebugging.GameObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.GameObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gameObject = newGameObject;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceGameObject(UnityEngine.GameObject newGameObject)
	{
		var index = VisualDebugComponentsLookup.GameObject;
		var component = (ExampleContent.VisualDebugging.GameObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.GameObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gameObject = newGameObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyGameObjectTo(ExampleContent.VisualDebugging.GameObjectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.GameObject;
		var component = (ExampleContent.VisualDebugging.GameObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.GameObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.gameObject = copyComponent.gameObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveGameObject()
	{
		RemoveComponent(VisualDebugComponentsLookup.GameObject);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherGameObject;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> GameObject
	{
		get
		{
			if (_matcherGameObject == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.GameObject);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherGameObject = matcher;
			}

			return _matcherGameObject;
		}
	}
}
