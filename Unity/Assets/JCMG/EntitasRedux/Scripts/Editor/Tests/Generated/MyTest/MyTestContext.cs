public sealed partial class MyTestContext : JCMG.EntitasRedux.Context<MyTestEntity>
{
	public MyTestContext()
		: base(
			MyTestComponentsLookup.TotalComponents,
			0,
			new JCMG.EntitasRedux.ContextInfo(
				"MyTest",
				MyTestComponentsLookup.ComponentNames,
				MyTestComponentsLookup.ComponentTypes
			),
			(entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
				new JCMG.EntitasRedux.UnsafeAERC(),
#else
				new JCMG.EntitasRedux.SafeAERC(entity),
#endif
			() => new MyTestEntity()
		)
	{
	}

	/// <summary>
	/// Creates a new entity and adds copies of all specified components to it. If replaceExisting is true, it will
	/// replace existing components.
	/// </summary>
	public MyTestEntity CloneEntity(MyTestEntity entity, bool replaceExisting = false, params int[] indices)
	{
		var target = CreateEntity();
		entity.CopyTo(target, replaceExisting, indices);
		return target;
	}
}
