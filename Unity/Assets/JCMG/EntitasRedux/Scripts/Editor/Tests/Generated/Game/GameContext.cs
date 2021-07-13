public sealed partial class GameContext : JCMG.EntitasRedux.Context<GameEntity>
{
	public GameContext()
		: base(
			GameComponentsLookup.TotalComponents,
			0,
			new JCMG.EntitasRedux.ContextInfo(
				"Game",
				GameComponentsLookup.ComponentNames,
				GameComponentsLookup.ComponentTypes
			),
			(entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
				new JCMG.EntitasRedux.UnsafeAERC(),
#else
				new JCMG.EntitasRedux.SafeAERC(entity),
#endif
			() => new GameEntity()
		)
	{
	}

	/// <summary>
	/// Creates a new entity and adds copies of all specified components to it. If replaceExisting is true, it will
	/// replace existing components.
	/// </summary>
	public GameEntity CloneEntity(GameEntity entity, bool replaceExisting = false, params int[] indices)
	{
		var target = CreateEntity();
		entity.CopyTo(target, replaceExisting, indices);
		return target;
	}
}
