public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.UnityObjectComponent UnityObject { get { return (ExampleContent.VisualDebugging.UnityObjectComponent)GetComponent(VisualDebugComponentsLookup.UnityObject); } }
	public bool HasUnityObject { get { return HasComponent(VisualDebugComponentsLookup.UnityObject); } }

	public void AddUnityObject(UnityEngine.Object newUnityObject)
	{
		var index = VisualDebugComponentsLookup.UnityObject;
		var component = (ExampleContent.VisualDebugging.UnityObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.UnityObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.unityObject = newUnityObject;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceUnityObject(UnityEngine.Object newUnityObject)
	{
		var index = VisualDebugComponentsLookup.UnityObject;
		var component = (ExampleContent.VisualDebugging.UnityObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.UnityObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.unityObject = newUnityObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyUnityObjectTo(ExampleContent.VisualDebugging.UnityObjectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.UnityObject;
		var component = (ExampleContent.VisualDebugging.UnityObjectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.UnityObjectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.unityObject = copyComponent.unityObject;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveUnityObject()
	{
		RemoveComponent(VisualDebugComponentsLookup.UnityObject);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherUnityObject;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> UnityObject
	{
		get
		{
			if (_matcherUnityObject == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.UnityObject);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherUnityObject = matcher;
			}

			return _matcherUnityObject;
		}
	}
}
