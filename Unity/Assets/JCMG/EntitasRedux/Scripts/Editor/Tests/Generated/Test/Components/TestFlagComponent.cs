public partial class TestEntity
{
	static readonly EntitasRedux.Tests.FlagComponent FlagComponent = new EntitasRedux.Tests.FlagComponent();

	public bool IsFlag
	{
		get { return HasComponent(TestComponentsLookup.Flag); }
		set
		{
			if (value != IsFlag)
			{
				var index = TestComponentsLookup.Flag;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: FlagComponent;

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
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherFlag;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> Flag
	{
		get
		{
			if (_matcherFlag == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.Flag);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherFlag = matcher;
			}

			return _matcherFlag;
		}
	}
}
