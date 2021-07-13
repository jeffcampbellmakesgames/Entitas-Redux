public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.RectComponent Rect { get { return (ExampleContent.VisualDebugging.RectComponent)GetComponent(VisualDebugComponentsLookup.Rect); } }
	public bool HasRect { get { return HasComponent(VisualDebugComponentsLookup.Rect); } }

	public void AddRect(UnityEngine.Rect newRect)
	{
		var index = VisualDebugComponentsLookup.Rect;
		var component = (ExampleContent.VisualDebugging.RectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.RectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.rect = newRect;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceRect(UnityEngine.Rect newRect)
	{
		var index = VisualDebugComponentsLookup.Rect;
		var component = (ExampleContent.VisualDebugging.RectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.RectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.rect = newRect;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyRectTo(ExampleContent.VisualDebugging.RectComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Rect;
		var component = (ExampleContent.VisualDebugging.RectComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.RectComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.rect = copyComponent.rect;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveRect()
	{
		RemoveComponent(VisualDebugComponentsLookup.Rect);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherRect;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Rect
	{
		get
		{
			if (_matcherRect == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Rect);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherRect = matcher;
			}

			return _matcherRect;
		}
	}
}
