public partial class VisualDebugEntity
{
	public CoolNameComponent CoolName { get { return (CoolNameComponent)GetComponent(VisualDebugComponentsLookup.CoolName); } }
	public bool HasCoolName { get { return HasComponent(VisualDebugComponentsLookup.CoolName); } }

	public void AddCoolName(ExampleContent.VisualDebugging.BadName newValue)
	{
		var index = VisualDebugComponentsLookup.CoolName;
		var component = (CoolNameComponent)CreateComponent(index, typeof(CoolNameComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceCoolName(ExampleContent.VisualDebugging.BadName newValue)
	{
		var index = VisualDebugComponentsLookup.CoolName;
		var component = (CoolNameComponent)CreateComponent(index, typeof(CoolNameComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyCoolNameTo(CoolNameComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.CoolName;
		var component = (CoolNameComponent)CreateComponent(index, typeof(CoolNameComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveCoolName()
	{
		RemoveComponent(VisualDebugComponentsLookup.CoolName);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherCoolName;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> CoolName
	{
		get
		{
			if (_matcherCoolName == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.CoolName);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherCoolName = matcher;
			}

			return _matcherCoolName;
		}
	}
}
