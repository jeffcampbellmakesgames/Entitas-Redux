public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.TextureComponent Texture { get { return (ExampleContent.VisualDebugging.TextureComponent)GetComponent(VisualDebugComponentsLookup.Texture); } }
	public bool HasTexture { get { return HasComponent(VisualDebugComponentsLookup.Texture); } }

	public void AddTexture(UnityEngine.Texture newTexture)
	{
		var index = VisualDebugComponentsLookup.Texture;
		var component = (ExampleContent.VisualDebugging.TextureComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.TextureComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.texture = newTexture;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTexture(UnityEngine.Texture newTexture)
	{
		var index = VisualDebugComponentsLookup.Texture;
		var component = (ExampleContent.VisualDebugging.TextureComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.TextureComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.texture = newTexture;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTextureTo(ExampleContent.VisualDebugging.TextureComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Texture;
		var component = (ExampleContent.VisualDebugging.TextureComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.TextureComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.texture = copyComponent.texture;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTexture()
	{
		RemoveComponent(VisualDebugComponentsLookup.Texture);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherTexture;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Texture
	{
		get
		{
			if (_matcherTexture == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Texture);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherTexture = matcher;
			}

			return _matcherTexture;
		}
	}
}
