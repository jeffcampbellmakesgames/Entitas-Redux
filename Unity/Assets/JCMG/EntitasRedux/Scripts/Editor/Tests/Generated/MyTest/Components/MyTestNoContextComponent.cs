public partial class MyTestEntity
{
	static readonly EntitasRedux.Tests.NoContextComponent NoContextComponent = new EntitasRedux.Tests.NoContextComponent();

	public bool IsNoContext
	{
		get { return HasComponent(MyTestComponentsLookup.NoContext); }
		set
		{
			if (value != IsNoContext)
			{
				var index = MyTestComponentsLookup.NoContext;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: NoContextComponent;

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
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherNoContext;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> NoContext
	{
		get
		{
			if (_matcherNoContext == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.NoContext);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherNoContext = matcher;
			}

			return _matcherNoContext;
		}
	}
}
