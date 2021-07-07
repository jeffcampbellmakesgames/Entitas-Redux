public partial class GameEntity
{
	public CleanupEventAddedListenerComponent CleanupEventAddedListener { get { return (CleanupEventAddedListenerComponent)GetComponent(GameComponentsLookup.CleanupEventAddedListener); } }
	public bool HasCleanupEventAddedListener { get { return HasComponent(GameComponentsLookup.CleanupEventAddedListener); } }

	public void AddCleanupEventAddedListener(System.Collections.Generic.List<ICleanupEventAddedListener> newValue)
	{
		var index = GameComponentsLookup.CleanupEventAddedListener;
		var component = (CleanupEventAddedListenerComponent)CreateComponent(index, typeof(CleanupEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceCleanupEventAddedListener(System.Collections.Generic.List<ICleanupEventAddedListener> newValue)
	{
		var index = GameComponentsLookup.CleanupEventAddedListener;
		var component = (CleanupEventAddedListenerComponent)CreateComponent(index, typeof(CleanupEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyCleanupEventAddedListenerTo(CleanupEventAddedListenerComponent copyComponent)
	{
		var index = GameComponentsLookup.CleanupEventAddedListener;
		var component = (CleanupEventAddedListenerComponent)CreateComponent(index, typeof(CleanupEventAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveCleanupEventAddedListener()
	{
		RemoveComponent(GameComponentsLookup.CleanupEventAddedListener);
	}
}

public sealed partial class GameMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherCleanupEventAddedListener;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> CleanupEventAddedListener
	{
		get
		{
			if (_matcherCleanupEventAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.CleanupEventAddedListener);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherCleanupEventAddedListener = matcher;
			}

			return _matcherCleanupEventAddedListener;
		}
	}
}

public partial class GameEntity
{
	public void AddCleanupEventAddedListener(ICleanupEventAddedListener value)
	{
		var listeners = HasCleanupEventAddedListener
			? CleanupEventAddedListener.value
			: new System.Collections.Generic.List<ICleanupEventAddedListener>();
		listeners.Add(value);
		ReplaceCleanupEventAddedListener(listeners);
	}

	public void RemoveCleanupEventAddedListener(ICleanupEventAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = CleanupEventAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveCleanupEventAddedListener();
		}
		else
		{
			ReplaceCleanupEventAddedListener(listeners);
		}
	}
}
