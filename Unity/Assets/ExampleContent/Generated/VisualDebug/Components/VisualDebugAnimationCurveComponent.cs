public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.AnimationCurveComponent AnimationCurve { get { return (ExampleContent.VisualDebugging.AnimationCurveComponent)GetComponent(VisualDebugComponentsLookup.AnimationCurve); } }
	public bool HasAnimationCurve { get { return HasComponent(VisualDebugComponentsLookup.AnimationCurve); } }

	public void AddAnimationCurve(UnityEngine.AnimationCurve newAnimationCurve)
	{
		var index = VisualDebugComponentsLookup.AnimationCurve;
		var component = (ExampleContent.VisualDebugging.AnimationCurveComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnimationCurveComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.animationCurve = newAnimationCurve;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceAnimationCurve(UnityEngine.AnimationCurve newAnimationCurve)
	{
		var index = VisualDebugComponentsLookup.AnimationCurve;
		var component = (ExampleContent.VisualDebugging.AnimationCurveComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnimationCurveComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.animationCurve = newAnimationCurve;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyAnimationCurveTo(ExampleContent.VisualDebugging.AnimationCurveComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.AnimationCurve;
		var component = (ExampleContent.VisualDebugging.AnimationCurveComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.AnimationCurveComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.animationCurve = copyComponent.animationCurve;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveAnimationCurve()
	{
		RemoveComponent(VisualDebugComponentsLookup.AnimationCurve);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherAnimationCurve;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> AnimationCurve
	{
		get
		{
			if (_matcherAnimationCurve == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.AnimationCurve);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherAnimationCurve = matcher;
			}

			return _matcherAnimationCurve;
		}
	}
}
