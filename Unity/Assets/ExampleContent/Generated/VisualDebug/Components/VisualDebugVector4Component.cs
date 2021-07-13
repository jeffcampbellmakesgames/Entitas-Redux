public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.Vector4Component Vector4 { get { return (ExampleContent.VisualDebugging.Vector4Component)GetComponent(VisualDebugComponentsLookup.Vector4); } }
	public bool HasVector4 { get { return HasComponent(VisualDebugComponentsLookup.Vector4); } }

	public void AddVector4(UnityEngine.Vector4 newVector4)
	{
		var index = VisualDebugComponentsLookup.Vector4;
		var component = (ExampleContent.VisualDebugging.Vector4Component)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Vector4Component));
		#if !ENTITAS_REDUX_NO_IMPL
		component.vector4 = newVector4;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceVector4(UnityEngine.Vector4 newVector4)
	{
		var index = VisualDebugComponentsLookup.Vector4;
		var component = (ExampleContent.VisualDebugging.Vector4Component)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Vector4Component));
		#if !ENTITAS_REDUX_NO_IMPL
		component.vector4 = newVector4;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyVector4To(ExampleContent.VisualDebugging.Vector4Component copyComponent)
	{
		var index = VisualDebugComponentsLookup.Vector4;
		var component = (ExampleContent.VisualDebugging.Vector4Component)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Vector4Component));
		#if !ENTITAS_REDUX_NO_IMPL
		component.vector4 = copyComponent.vector4;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveVector4()
	{
		RemoveComponent(VisualDebugComponentsLookup.Vector4);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherVector4;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Vector4
	{
		get
		{
			if (_matcherVector4 == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Vector4);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherVector4 = matcher;
			}

			return _matcherVector4;
		}
	}
}
