public partial interface IEntityIndexEntity
{
	EntitasRedux.Tests.EntityIndexComponent EntityIndex { get; }
	bool HasEntityIndex { get; }

	void AddEntityIndex(string newValue);
	void ReplaceEntityIndex(string newValue);
	void RemoveEntityIndex();
}
