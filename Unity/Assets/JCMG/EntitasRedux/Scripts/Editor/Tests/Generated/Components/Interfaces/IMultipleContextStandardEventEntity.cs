public partial interface IMultipleContextStandardEventEntity
{
	EntitasRedux.Tests.MultipleContextStandardEventComponent MultipleContextStandardEvent { get; }
	bool HasMultipleContextStandardEvent { get; }

	void AddMultipleContextStandardEvent(string newValue);
	void ReplaceMultipleContextStandardEvent(string newValue);
	void RemoveMultipleContextStandardEvent();
}
