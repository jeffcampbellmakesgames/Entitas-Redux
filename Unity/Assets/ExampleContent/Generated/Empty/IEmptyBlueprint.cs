
/// <summary>
/// Represents a group of <see cref="JCMG.EntitasRedux.IComponent"/> instances that can be copied to one or more
/// <see cref="EmptyEntity"/>.
/// </summary>
public interface IEmptyBlueprint
{
	/// <summary>
	/// Applies all components in the blueprint to <paramref name="entity"/>.
	/// </summary>
	/// <param name="entity"></param>
	void ApplyToEntity(EmptyEntity entity);
}
