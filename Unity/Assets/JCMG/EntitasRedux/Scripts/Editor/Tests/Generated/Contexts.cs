public partial class Contexts : JCMG.EntitasRedux.IContexts
{
	#if UNITY_EDITOR && !ENTITAS_REDUX_NO_SHARED_CONTEXT

	static Contexts()
	{
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	/// <summary>
	/// Invoked when the Unity Editor has a <see cref="UnityEditor.PlayModeStateChange"/> change.
	/// </summary>
	private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
	{
		// When entering edit-mode, reset all static state so that it does not interfere with the
		// next play-mode session.
		if (playModeStateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
		{
			_sharedInstance = null;
		}
	}

	#endif

	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	/// <summary>
	/// A globally-accessible singleton instance of <see cref="Contexts"/>. Instantiated
	/// the first time its <see langword="get"/> property is used.
	/// </summary>
	/// <remarks>
	/// If your project forbids global singletons like this one, add a <c>#define</c> named <c>ENTITAS_REDUX_NO_SHARED_CONTEXT</c>
	/// to its build settings. Doing so will remove this property to prevent accidental use.
	/// </remarks>
	public static Contexts SharedInstance
	{
		get
		{
			if (_sharedInstance == null)
			{
				_sharedInstance = new Contexts();
			}

			return _sharedInstance;
		}
		set	{ _sharedInstance = value; }
	}

	static Contexts _sharedInstance;
	#endif

	public GameContext Game { get; set; }
	public MyTestContext MyTest { get; set; }
	public MyTest2Context MyTest2 { get; set; }
	public TestContext Test { get; set; }
	public Test2Context Test2 { get; set; }

	public JCMG.EntitasRedux.IContext[] AllContexts { get { return new JCMG.EntitasRedux.IContext [] { Game, MyTest, MyTest2, Test, Test2 }; } }

	public Contexts()
	{
		Game = new GameContext();
		MyTest = new MyTestContext();
		MyTest2 = new MyTest2Context();
		Test = new TestContext();
		Test2 = new Test2Context();

		var postConstructors = System.Linq.Enumerable.Where(
			GetType().GetMethods(),
			method => System.Attribute.IsDefined(method, typeof(JCMG.EntitasRedux.PostConstructorAttribute))
		);

		foreach (var postConstructor in postConstructors)
		{
			postConstructor.Invoke(this, null);
		}
	}

	public void Reset()
	{
		var contexts = AllContexts;
		for (int i = 0; i < contexts.Length; i++)
		{
			contexts[i].Reset();
		}
	}
}

public partial class Contexts
{
	public const string EntitasReduxTestsMyCustomEntityIndex = "EntitasReduxTestsMyCustomEntityIndex";
	public const string EntityIndex = "EntityIndex";
	public const string EntityIndexNoContext = "EntityIndexNoContext";
	public const string MultipleEntityIndicesValue = "MultipleEntityIndicesValue";
	public const string MultipleEntityIndicesValue2 = "MultipleEntityIndicesValue2";
	public const string MultiplePrimaryEntityIndicesValue = "MultiplePrimaryEntityIndicesValue";
	public const string MultiplePrimaryEntityIndicesValue2 = "MultiplePrimaryEntityIndicesValue2";
	public const string PrimaryEntityIndex = "PrimaryEntityIndex";

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeEntityIndices()
	{
		Test.AddEntityIndex(new EntitasRedux.Tests.MyCustomEntityIndex(Test));

		Test.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<TestEntity, string>(
			EntityIndex,
			Test.GetGroup(TestMatcher.EntityIndex),
			(e, c) => ((EntitasRedux.Tests.EntityIndexComponent)c).value));
		Test2.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<Test2Entity, string>(
			EntityIndex,
			Test2.GetGroup(Test2Matcher.EntityIndex),
			(e, c) => ((EntitasRedux.Tests.EntityIndexComponent)c).value));

		MyTest.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<MyTestEntity, string>(
			EntityIndexNoContext,
			MyTest.GetGroup(MyTestMatcher.EntityIndexNoContext),
			(e, c) => ((EntitasRedux.Tests.EntityIndexNoContextComponent)c).value));

		Test.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<TestEntity, string>(
			MultipleEntityIndicesValue,
			Test.GetGroup(TestMatcher.MultipleEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultipleEntityIndicesComponent)c).value));
		Test2.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<Test2Entity, string>(
			MultipleEntityIndicesValue,
			Test2.GetGroup(Test2Matcher.MultipleEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultipleEntityIndicesComponent)c).value));

		Test.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<TestEntity, string>(
			MultipleEntityIndicesValue2,
			Test.GetGroup(TestMatcher.MultipleEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultipleEntityIndicesComponent)c).value2));
		Test2.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<Test2Entity, string>(
			MultipleEntityIndicesValue2,
			Test2.GetGroup(Test2Matcher.MultipleEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultipleEntityIndicesComponent)c).value2));

		MyTest.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>(
			MultiplePrimaryEntityIndicesValue,
			MyTest.GetGroup(MyTestMatcher.MultiplePrimaryEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultiplePrimaryEntityIndicesComponent)c).value));

		MyTest.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>(
			MultiplePrimaryEntityIndicesValue2,
			MyTest.GetGroup(MyTestMatcher.MultiplePrimaryEntityIndices),
			(e, c) => ((EntitasRedux.Tests.MultiplePrimaryEntityIndicesComponent)c).value2));

		MyTest.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>(
			PrimaryEntityIndex,
			MyTest.GetGroup(MyTestMatcher.PrimaryEntityIndex),
			(e, c) => ((EntitasRedux.Tests.PrimaryEntityIndexComponent)c).value));
	}
}

public static class ContextsExtensions
{
	public static System.Collections.Generic.HashSet<TestEntity> GetEntitiesWithPosition(this TestContext context, EntitasRedux.Tests.IntVector2 position)
	{
		return ((EntitasRedux.Tests.MyCustomEntityIndex)(context.GetEntityIndex(Contexts.EntitasReduxTestsMyCustomEntityIndex))).GetEntitiesWithPosition(position);
	}
	public static System.Collections.Generic.HashSet<TestEntity> GetEntitiesWithPosition2(this TestContext context, EntitasRedux.Tests.IntVector2 position, EntitasRedux.Tests.IntVector2 size)
	{
		return ((EntitasRedux.Tests.MyCustomEntityIndex)(context.GetEntityIndex(Contexts.EntitasReduxTestsMyCustomEntityIndex))).GetEntitiesWithPosition2(position, size);
	}

	public static System.Collections.Generic.HashSet<TestEntity> GetEntitiesWithEntityIndex(this TestContext context, string value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<TestEntity, string>)context.GetEntityIndex(Contexts.EntityIndex)).GetEntities(value);
	}

	public static System.Collections.Generic.HashSet<Test2Entity> GetEntitiesWithEntityIndex(this Test2Context context, string value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<Test2Entity, string>)context.GetEntityIndex(Contexts.EntityIndex)).GetEntities(value);
	}

	public static System.Collections.Generic.HashSet<MyTestEntity> GetEntitiesWithEntityIndexNoContext(this MyTestContext context, string value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<MyTestEntity, string>)context.GetEntityIndex(Contexts.EntityIndexNoContext)).GetEntities(value);
	}

	public static System.Collections.Generic.HashSet<TestEntity> GetEntitiesWithMultipleEntityIndicesValue(this TestContext context, string value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<TestEntity, string>)context.GetEntityIndex(Contexts.MultipleEntityIndicesValue)).GetEntities(value);
	}

	public static System.Collections.Generic.HashSet<Test2Entity> GetEntitiesWithMultipleEntityIndicesValue(this Test2Context context, string value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<Test2Entity, string>)context.GetEntityIndex(Contexts.MultipleEntityIndicesValue)).GetEntities(value);
	}

	public static System.Collections.Generic.HashSet<TestEntity> GetEntitiesWithMultipleEntityIndicesValue2(this TestContext context, string value2)
	{
		return ((JCMG.EntitasRedux.EntityIndex<TestEntity, string>)context.GetEntityIndex(Contexts.MultipleEntityIndicesValue2)).GetEntities(value2);
	}

	public static System.Collections.Generic.HashSet<Test2Entity> GetEntitiesWithMultipleEntityIndicesValue2(this Test2Context context, string value2)
	{
		return ((JCMG.EntitasRedux.EntityIndex<Test2Entity, string>)context.GetEntityIndex(Contexts.MultipleEntityIndicesValue2)).GetEntities(value2);
	}

	public static MyTestEntity GetEntityWithMultiplePrimaryEntityIndicesValue(this MyTestContext context, string value)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>)context.GetEntityIndex(Contexts.MultiplePrimaryEntityIndicesValue)).GetEntity(value);
	}

	public static MyTestEntity GetEntityWithMultiplePrimaryEntityIndicesValue2(this MyTestContext context, string value2)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>)context.GetEntityIndex(Contexts.MultiplePrimaryEntityIndicesValue2)).GetEntity(value2);
	}

	public static MyTestEntity GetEntityWithPrimaryEntityIndex(this MyTestContext context, string value)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<MyTestEntity, string>)context.GetEntityIndex(Contexts.PrimaryEntityIndex)).GetEntity(value);
	}
}
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeContextObservers() {
		try {
			CreateContextObserver(Game);
			CreateContextObserver(MyTest);
			CreateContextObserver(MyTest2);
			CreateContextObserver(Test);
			CreateContextObserver(Test2);
		} catch(System.Exception) {
		}
	}

	public void CreateContextObserver(JCMG.EntitasRedux.IContext context) {
		if (UnityEngine.Application.isPlaying) {
			var observer = new JCMG.EntitasRedux.VisualDebugging.ContextObserver(context);
			UnityEngine.Object.DontDestroyOnLoad(observer.GameObject);
		}
	}

#endif
}
