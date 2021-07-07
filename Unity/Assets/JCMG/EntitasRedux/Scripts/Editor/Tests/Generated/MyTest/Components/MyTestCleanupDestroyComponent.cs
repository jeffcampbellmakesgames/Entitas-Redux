public partial class MyTestEntity
{
	static readonly EntitasRedux.Tests.CleanupDestroyComponent CleanupDestroyComponent = new EntitasRedux.Tests.CleanupDestroyComponent();

	public bool IsCleanupDestroy
	{
		get { return HasComponent(MyTestComponentsLookup.CleanupDestroy); }
		set
		{
			if (value != IsCleanupDestroy)
			{
				var index = MyTestComponentsLookup.CleanupDestroy;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: CleanupDestroyComponent;

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
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherCleanupDestroy;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> CleanupDestroy
	{
		get
		{
			if (_matcherCleanupDestroy == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.CleanupDestroy);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherCleanupDestroy = matcher;
			}

			return _matcherCleanupDestroy;
		}
	}
}
