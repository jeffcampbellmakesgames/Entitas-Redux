public partial class TestEntity
{
	public EntitasRedux.Tests.SizeComponent Size { get { return (EntitasRedux.Tests.SizeComponent)GetComponent(TestComponentsLookup.Size); } }
	public bool HasSize { get { return HasComponent(TestComponentsLookup.Size); } }

	public void AddSize(int newWidth, int newHeight)
	{
		var index = TestComponentsLookup.Size;
		var component = (EntitasRedux.Tests.SizeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.SizeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.width = newWidth;
		component.height = newHeight;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSize(int newWidth, int newHeight)
	{
		var index = TestComponentsLookup.Size;
		var component = (EntitasRedux.Tests.SizeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.SizeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.width = newWidth;
		component.height = newHeight;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySizeTo(EntitasRedux.Tests.SizeComponent copyComponent)
	{
		var index = TestComponentsLookup.Size;
		var component = (EntitasRedux.Tests.SizeComponent)CreateComponent(index, typeof(EntitasRedux.Tests.SizeComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.width = copyComponent.width;
		component.height = copyComponent.height;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSize()
	{
		RemoveComponent(TestComponentsLookup.Size);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherSize;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> Size
	{
		get
		{
			if (_matcherSize == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.Size);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherSize = matcher;
			}

			return _matcherSize;
		}
	}
}
