public partial interface IMultipleEntityIndicesEntity
{
	EntitasRedux.Tests.MultipleEntityIndicesComponent MultipleEntityIndices { get; }
	bool HasMultipleEntityIndices { get; }

	void AddMultipleEntityIndices(string newValue, string newValue2);
	void ReplaceMultipleEntityIndices(string newValue, string newValue2);
	void RemoveMultipleEntityIndices();
}
