public partial class TestEntity
{
	public EntitasRedux.Tests.PositionComponent Position { get { return (EntitasRedux.Tests.PositionComponent)GetComponent(TestComponentsLookup.Position); } }
	public bool HasPosition { get { return HasComponent(TestComponentsLookup.Position); } }

	public void AddPosition(int newX, int newY)
	{
		var index = TestComponentsLookup.Position;
		var component = (EntitasRedux.Tests.PositionComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.x = newX;
		component.y = newY;
		#endif
		AddComponent(index, component);
	}

	public void ReplacePosition(int newX, int newY)
	{
		var index = TestComponentsLookup.Position;
		var component = (EntitasRedux.Tests.PositionComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.x = newX;
		component.y = newY;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyPositionTo(EntitasRedux.Tests.PositionComponent copyComponent)
	{
		var index = TestComponentsLookup.Position;
		var component = (EntitasRedux.Tests.PositionComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.x = copyComponent.x;
		component.y = copyComponent.y;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemovePosition()
	{
		RemoveComponent(TestComponentsLookup.Position);
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherPosition;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> Position
	{
		get
		{
			if (_matcherPosition == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.Position);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherPosition = matcher;
			}

			return _matcherPosition;
		}
	}
}
