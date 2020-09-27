//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TestEntity {

    public AnyUniqueEventAddedListenerComponent AnyUniqueEventAddedListener { get { return (AnyUniqueEventAddedListenerComponent)GetComponent(TestComponentsLookup.AnyUniqueEventAddedListener); } }
    public bool HasAnyUniqueEventAddedListener { get { return HasComponent(TestComponentsLookup.AnyUniqueEventAddedListener); } }

    public void AddAnyUniqueEventAddedListener(System.Collections.Generic.List<IAnyUniqueEventAddedListener> newValue) {
        var index = TestComponentsLookup.AnyUniqueEventAddedListener;
        var component = (AnyUniqueEventAddedListenerComponent)CreateComponent(index, typeof(AnyUniqueEventAddedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyUniqueEventAddedListener(System.Collections.Generic.List<IAnyUniqueEventAddedListener> newValue) {
        var index = TestComponentsLookup.AnyUniqueEventAddedListener;
        var component = (AnyUniqueEventAddedListenerComponent)CreateComponent(index, typeof(AnyUniqueEventAddedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

	public void CopyAnyUniqueEventAddedListenerTo(AnyUniqueEventAddedListenerComponent copyComponent) {
        var index = TestComponentsLookup.AnyUniqueEventAddedListener;
        var component = (AnyUniqueEventAddedListenerComponent)CreateComponent(index, typeof(AnyUniqueEventAddedListenerComponent));
        component.value = copyComponent.value;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyUniqueEventAddedListener() {
        RemoveComponent(TestComponentsLookup.AnyUniqueEventAddedListener);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class TestMatcher {

    static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherAnyUniqueEventAddedListener;

    public static JCMG.EntitasRedux.IMatcher<TestEntity> AnyUniqueEventAddedListener {
        get {
            if (_matcherAnyUniqueEventAddedListener == null) {
                var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.AnyUniqueEventAddedListener);
                matcher.ComponentNames = TestComponentsLookup.ComponentNames;
                _matcherAnyUniqueEventAddedListener = matcher;
            }

            return _matcherAnyUniqueEventAddedListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TestEntity {

    public void AddAnyUniqueEventAddedListener(IAnyUniqueEventAddedListener value) {
        var listeners = HasAnyUniqueEventAddedListener
            ? AnyUniqueEventAddedListener.value
            : new System.Collections.Generic.List<IAnyUniqueEventAddedListener>();
        listeners.Add(value);
        ReplaceAnyUniqueEventAddedListener(listeners);
    }

    public void RemoveAnyUniqueEventAddedListener(IAnyUniqueEventAddedListener value, bool removeComponentWhenEmpty = true) {
        var listeners = AnyUniqueEventAddedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyUniqueEventAddedListener();
        } else {
            ReplaceAnyUniqueEventAddedListener(listeners);
        }
    }
}