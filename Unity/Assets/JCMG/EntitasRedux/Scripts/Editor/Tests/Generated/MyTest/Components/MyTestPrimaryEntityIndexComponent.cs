public partial class MyTestEntity
{
	public EntitasRedux.Tests.PrimaryEntityIndexComponent PrimaryEntityIndex { get { return (EntitasRedux.Tests.PrimaryEntityIndexComponent)GetComponent(MyTestComponentsLookup.PrimaryEntityIndex); } }
	public bool HasPrimaryEntityIndex { get { return HasComponent(MyTestComponentsLookup.PrimaryEntityIndex); } }

	public void AddPrimaryEntityIndex(string newValue)
	{
		var index = MyTestComponentsLookup.PrimaryEntityIndex;
		var component = (EntitasRedux.Tests.PrimaryEntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PrimaryEntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplacePrimaryEntityIndex(string newValue)
	{
		var index = MyTestComponentsLookup.PrimaryEntityIndex;
		var component = (EntitasRedux.Tests.PrimaryEntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PrimaryEntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyPrimaryEntityIndexTo(EntitasRedux.Tests.PrimaryEntityIndexComponent copyComponent)
	{
		var index = MyTestComponentsLookup.PrimaryEntityIndex;
		var component = (EntitasRedux.Tests.PrimaryEntityIndexComponent)CreateComponent(index, typeof(EntitasRedux.Tests.PrimaryEntityIndexComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemovePrimaryEntityIndex()
	{
		RemoveComponent(MyTestComponentsLookup.PrimaryEntityIndex);
	}
}

public sealed partial class MyTestMatcher
{
	static JCMG.EntitasRedux.IMatcher<MyTestEntity> _matcherPrimaryEntityIndex;

	public static JCMG.EntitasRedux.IMatcher<MyTestEntity> PrimaryEntityIndex
	{
		get
		{
			if (_matcherPrimaryEntityIndex == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MyTestEntity>)JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.PrimaryEntityIndex);
				matcher.ComponentNames = MyTestComponentsLookup.ComponentNames;
				_matcherPrimaryEntityIndex = matcher;
			}

			return _matcherPrimaryEntityIndex;
		}
	}
}
