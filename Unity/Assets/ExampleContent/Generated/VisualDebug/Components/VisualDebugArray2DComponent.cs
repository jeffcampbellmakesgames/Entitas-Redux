public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.Array2DComponent Array2D { get { return (ExampleContent.VisualDebugging.Array2DComponent)GetComponent(VisualDebugComponentsLookup.Array2D); } }
	public bool HasArray2D { get { return HasComponent(VisualDebugComponentsLookup.Array2D); } }

	public void AddArray2D(string[,] newArray2d)
	{
		var index = VisualDebugComponentsLookup.Array2D;
		var component = (ExampleContent.VisualDebugging.Array2DComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Array2DComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array2d = newArray2d;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceArray2D(string[,] newArray2d)
	{
		var index = VisualDebugComponentsLookup.Array2D;
		var component = (ExampleContent.VisualDebugging.Array2DComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Array2DComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array2d = newArray2d;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyArray2DTo(ExampleContent.VisualDebugging.Array2DComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Array2D;
		var component = (ExampleContent.VisualDebugging.Array2DComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.Array2DComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.array2d = (string[,])copyComponent.array2d.Clone();
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveArray2D()
	{
		RemoveComponent(VisualDebugComponentsLookup.Array2D);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherArray2D;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Array2D
	{
		get
		{
			if (_matcherArray2D == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Array2D);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherArray2D = matcher;
			}

			return _matcherArray2D;
		}
	}
}
