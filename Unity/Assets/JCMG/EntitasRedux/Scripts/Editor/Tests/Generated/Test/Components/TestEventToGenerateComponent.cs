public partial class TestEntity
{
	public EventToGenerateComponent EventToGenerate { get { return (EventToGenerateComponent)GetComponent(TestComponentsLookup.EventToGenerate); } }
	public bool HasEventToGenerate { get { return HasComponent(TestComponentsLookup.EventToGenerate); } }

	public void AddEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue)
	{
		var index = TestComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue)
	{
		var index = TestComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyEventToGenerateTo(EventToGenerateComponent copyComponent)
	{
		var index = TestComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveEventToGenerate()
	{
		RemoveComponent(TestComponentsLookup.EventToGenerate);
	}
}

public partial class TestEntity : IEventToGenerateEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherEventToGenerate;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> EventToGenerate
	{
		get
		{
			if (_matcherEventToGenerate == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.EventToGenerate);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherEventToGenerate = matcher;
			}

			return _matcherEventToGenerate;
		}
	}
}
