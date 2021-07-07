public partial class MyTestEntity
{
	static readonly EntitasRedux.Tests.ComponentD ComponentDComponent = new EntitasRedux.Tests.ComponentD();

	public bool IsComponentD
	{
		get { return HasComponent(MyTestComponentsLookup.ComponentD); }
		set
		{
			if (value != IsComponentD)
			{
				var index = MyTestComponentsLookup.ComponentD;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: ComponentDComponent;

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
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherComponentD;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> ComponentD
	{
		get
		{
			if (_matcherComponentD == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentD);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherComponentD = matcher;
			}

			return _matcherComponentD;
		}
	}
}
