public partial class MyTestEntity
{
	static readonly EntitasRedux.Tests.ComponentC ComponentCComponent = new EntitasRedux.Tests.ComponentC();

	public bool IsComponentC
	{
		get { return HasComponent(MyTestComponentsLookup.ComponentC); }
		set
		{
			if (value != IsComponentC)
			{
				var index = MyTestComponentsLookup.ComponentC;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: ComponentCComponent;

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

public sealed partial class MyTestMatcher
{
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherComponentC;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> ComponentC
	{
		get
		{
			if (_matcherComponentC == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentC);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherComponentC = matcher;
			}

			return _matcherComponentC;
		}
	}
}
