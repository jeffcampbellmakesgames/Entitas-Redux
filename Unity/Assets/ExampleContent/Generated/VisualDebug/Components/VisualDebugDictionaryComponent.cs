public partial class VisualDebugEntity
{
	public ExampleContent.VisualDebugging.DictionaryComponent Dictionary { get { return (ExampleContent.VisualDebugging.DictionaryComponent)GetComponent(VisualDebugComponentsLookup.Dictionary); } }
	public bool HasDictionary { get { return HasComponent(VisualDebugComponentsLookup.Dictionary); } }

	public void AddDictionary(System.Collections.Generic.Dictionary<string, string> newDict)
	{
		var index = VisualDebugComponentsLookup.Dictionary;
		var component = (ExampleContent.VisualDebugging.DictionaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DictionaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.dict = newDict;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceDictionary(System.Collections.Generic.Dictionary<string, string> newDict)
	{
		var index = VisualDebugComponentsLookup.Dictionary;
		var component = (ExampleContent.VisualDebugging.DictionaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DictionaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.dict = newDict;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyDictionaryTo(ExampleContent.VisualDebugging.DictionaryComponent copyComponent)
	{
		var index = VisualDebugComponentsLookup.Dictionary;
		var component = (ExampleContent.VisualDebugging.DictionaryComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.DictionaryComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.dict = (System.Collections.Generic.Dictionary<string, string>)JCMG.EntitasRedux.DictionaryTools.DeepCopy(copyComponent.dict);
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveDictionary()
	{
		RemoveComponent(VisualDebugComponentsLookup.Dictionary);
	}
}

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherDictionary;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Dictionary
	{
		get
		{
			if (_matcherDictionary == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Dictionary);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherDictionary = matcher;
			}

			return _matcherDictionary;
		}
	}
}
