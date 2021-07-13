public partial interface IEventToGenerateEntity
{
	EventToGenerateComponent EventToGenerate { get; }
	bool HasEventToGenerate { get; }

	void AddEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue);
	void ReplaceEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue);
	void RemoveEventToGenerate();
}
