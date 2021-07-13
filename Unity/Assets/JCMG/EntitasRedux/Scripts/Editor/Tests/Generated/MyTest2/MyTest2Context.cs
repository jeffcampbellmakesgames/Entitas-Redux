public sealed partial class MyTest2Context : JCMG.EntitasRedux.Context<MyTest2Entity>
{
	public MyTest2Context()
		: base(
			MyTest2ComponentsLookup.TotalComponents,
			0,
			new JCMG.EntitasRedux.ContextInfo(
				"MyTest2",
				MyTest2ComponentsLookup.ComponentNames,
				MyTest2ComponentsLookup.ComponentTypes
			),
			(entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
				new JCMG.EntitasRedux.UnsafeAERC(),
#else
				new JCMG.EntitasRedux.SafeAERC(entity),
#endif
			() => new MyTest2Entity()
		)
	{
	}

	/// <summary>
	/// Creates a new entity and adds copies of all specified components to it. If replaceExisting is true, it will
	/// replace existing components.
	/// </summary>
	public MyTest2Entity CloneEntity(MyTest2Entity entity, bool replaceExisting = false, params int[] indices)
	{
		var target = CreateEntity();
		entity.CopyTo(target, replaceExisting, indices);
		return target;
	}
}
