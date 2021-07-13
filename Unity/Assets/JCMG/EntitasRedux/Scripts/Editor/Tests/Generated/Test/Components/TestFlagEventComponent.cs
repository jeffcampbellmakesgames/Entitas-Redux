public partial class TestEntity
{
	static readonly EntitasRedux.Tests.FlagEventComponent FlagEventComponent = new EntitasRedux.Tests.FlagEventComponent();

	public bool IsFlagEvent
	{
		get { return HasComponent(TestComponentsLookup.FlagEvent); }
		set
		{
			if (value != IsFlagEvent)
			{
				var index = TestComponentsLookup.FlagEvent;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: FlagEventComponent;

					AddComponent(index, component);
				}
				else
				{
					RemoveComponent(index);
				}
			}
		}
	}
}

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherFlagEvent;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> FlagEvent
	{
		get
		{
			if (_matcherFlagEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.FlagEvent);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherFlagEvent = matcher;
			}

			return _matcherFlagEvent;
		}
	}
}
