public partial class TestContext {

	public TestEntity UniqueClassToGenerateEntity { get { return GetGroup(TestMatcher.UniqueClassToGenerate).GetSingleEntity(); } }
	public UniqueClassToGenerateComponent UniqueClassToGenerate { get { return UniqueClassToGenerateEntity.UniqueClassToGenerate; } }
	public bool HasUniqueClassToGenerate { get { return UniqueClassToGenerateEntity != null; } }

	public TestEntity SetUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue)
	{
		if (HasUniqueClassToGenerate)
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				"Could not set UniqueClassToGenerate!\n" +
				this +
				" already has an entity with UniqueClassToGenerateComponent!",
				"You should check if the context already has a UniqueClassToGenerateEntity before setting it or use context.ReplaceUniqueClassToGenerate().");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.AddUniqueClassToGenerate(newValue);
		#endif
		return entity;
	}

	public void ReplaceUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = UniqueClassToGenerateEntity;
		if (entity == null)
		{
			entity = SetUniqueClassToGenerate(newValue);
		}
		else
		{
			entity.ReplaceUniqueClassToGenerate(newValue);
		}
		#endif
	}

	public void RemoveUniqueClassToGenerate()
	{
		UniqueClassToGenerateEntity.Destroy();
	}
}

public partial class TestEntity
{
	public UniqueClassToGenerateComponent UniqueClassToGenerate { get { return (UniqueClassToGenerateComponent)GetComponent(TestComponentsLookup.UniqueClassToGenerate); } }
	public bool HasUniqueClassToGenerate { get { return HasComponent(TestComponentsLookup.UniqueClassToGenerate); } }

	public void AddUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue)
	{
		var index = TestComponentsLookup.UniqueClassToGenerate;
		var component = (UniqueClassToGenerateComponent)CreateComponent(index, typeof(UniqueClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue)
	{
		var index = TestComponentsLookup.UniqueClassToGenerate;
		var component = (UniqueClassToGenerateComponent)CreateComponent(index, typeof(UniqueClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyUniqueClassToGenerateTo(UniqueClassToGenerateComponent copyComponent)
	{
		var index = TestComponentsLookup.UniqueClassToGenerate;
		var component = (UniqueClassToGenerateComponent)CreateComponent(index, typeof(UniqueClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveUniqueClassToGenerate()
	{
		RemoveComponent(TestComponentsLookup.UniqueClassToGenerate);
	}
}

public partial class TestEntity : IUniqueClassToGenerateEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherUniqueClassToGenerate;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> UniqueClassToGenerate
	{
		get
		{
			if (_matcherUniqueClassToGenerate == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.UniqueClassToGenerate);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherUniqueClassToGenerate = matcher;
			}

			return _matcherUniqueClassToGenerate;
		}
	}
}
