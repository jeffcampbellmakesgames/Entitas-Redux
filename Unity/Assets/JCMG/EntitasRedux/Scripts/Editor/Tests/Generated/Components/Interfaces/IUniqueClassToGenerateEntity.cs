public partial interface IUniqueClassToGenerateEntity
{
	UniqueClassToGenerateComponent UniqueClassToGenerate { get; }
	bool HasUniqueClassToGenerate { get; }

	void AddUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue);
	void ReplaceUniqueClassToGenerate(EntitasRedux.Tests.UniqueClassToGenerate newValue);
	void RemoveUniqueClassToGenerate();
}
