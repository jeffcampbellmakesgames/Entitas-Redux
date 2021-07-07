public partial interface IMultipleEventsStandardEventEntity
{
	EntitasRedux.Tests.MultipleEventsStandardEventComponent MultipleEventsStandardEvent { get; }
	bool HasMultipleEventsStandardEvent { get; }

	void AddMultipleEventsStandardEvent(string newValue);
	void ReplaceMultipleEventsStandardEvent(string newValue);
	void RemoveMultipleEventsStandardEvent();
}
