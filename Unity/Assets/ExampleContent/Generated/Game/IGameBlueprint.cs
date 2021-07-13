
/// <summary>
/// Represents a group of <see cref="JCMG.EntitasRedux.IComponent"/> instances that can be copied to one or more
/// <see cref="GameEntity"/>.
/// </summary>
public interface IGameBlueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name="entity"/>.
	/// </summary>
	/// <param name="entity"></param>
	void ApplyToEntity(GameEntity entity);
}
