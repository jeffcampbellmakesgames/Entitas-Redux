public partial class VisualDebugEntity
{
	public VelocityComponent Velocity { get { return (VelocityComponent)GetComponent(VisualDebugComponentsLookup.Velocity); } }
	public bool HasVelocity { get { return HasComponent(VisualDebugComponentsLookup.Velocity); } }

	public void AddVelocity(ExampleContent.VisualDebugging.IntVector2 newValue)
	{
		var index = VisualDebugComponentsLookup.Velocity;
		var component = (VelocityComponent)CreateComponent(index, typeof(VelocityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceVelocity(ExampleContent.VisualDebugging.IntVector2 newValue)
	{
		var index = VisualDebugComponentsLookup.Velocity;
		var component = (VelocityComponent)CreateComponent(index, typeof(VelocityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyVelocityTo(VelocityComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Velocity;
		var component = (VelocityComponent)CreateComponent(index, typeof(VelocityComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveVelocity()
	{
		RemoveComponent(VisualDebugComponentsLookup.Velocity);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherVelocity;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Velocity
	{
		get
		{
			if (_matcherVelocity == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Velocity);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherVelocity = matcher;
			}

			return _matcherVelocity;
		}
	}
}
