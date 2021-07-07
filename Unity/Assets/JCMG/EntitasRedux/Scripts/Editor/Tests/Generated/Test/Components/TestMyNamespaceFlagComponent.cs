public partial class TestEntity
{
	static readonly EntitasRedux.Tests.MyNamespaceFlagComponent MyNamespaceFlagComponent = new EntitasRedux.Tests.MyNamespaceFlagComponent();

	public bool IsMyNamespaceFlag
	{
		get { return HasComponent(TestComponentsLookup.MyNamespaceFlag); }
		set
		{
			if (value != IsMyNamespaceFlag)
			{
				var index = TestComponentsLookup.MyNamespaceFlag;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: MyNamespaceFlagComponent;

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
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherMyNamespaceFlag;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> MyNamespaceFlag
	{
		get
		{
			if (_matcherMyNamespaceFlag == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.MyNamespaceFlag);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherMyNamespaceFlag = matcher;
			}

			return _matcherMyNamespaceFlag;
		}
	}
}
