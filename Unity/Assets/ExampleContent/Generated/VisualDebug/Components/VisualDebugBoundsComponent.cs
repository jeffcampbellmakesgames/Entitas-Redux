public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.BoundsComponent Bounds { get { return (ExampleContent.VisualDebugging.BoundsComponent)GetComponent(VisualDebugComponentsLookup.Bounds); } }
	public bool HasBounds { get { return HasComponent(VisualDebugComponentsLookup.Bounds); } }

	public void AddBounds(UnityEngine.Bounds newBounds)
	{
		var index = VisualDebugComponentsLookup.Bounds;
		var component = (ExampleContent.VisualDebugging.BoundsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.BoundsComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.bounds = newBounds;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceBounds(UnityEngine.Bounds newBounds)
	{
		var index = VisualDebugComponentsLookup.Bounds;
		var component = (ExampleContent.VisualDebugging.BoundsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.BoundsComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.bounds = newBounds;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyBoundsTo(ExampleContent.VisualDebugging.BoundsComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Bounds;
		var component = (ExampleContent.VisualDebugging.BoundsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.BoundsComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.bounds = copyComponent.bounds;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveBounds()
	{
		RemoveComponent(VisualDebugComponentsLookup.Bounds);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherBounds;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Bounds
	{
		get
		{
			if (_matcherBounds == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Bounds);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherBounds = matcher;
			}

			return _matcherBounds;
		}
	}
}
