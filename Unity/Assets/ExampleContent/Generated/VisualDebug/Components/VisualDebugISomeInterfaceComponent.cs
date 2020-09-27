//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class VisualDebugContext {

    public VisualDebugEntity ISomeInterfaceEntity { get { return GetGroup(VisualDebugMatcher.ISomeInterface).GetSingleEntity(); } }
    public ISomeInterfaceComponent ISomeInterface { get { return ISomeInterfaceEntity.ISomeInterface; } }
    public bool HasISomeInterface { get { return ISomeInterfaceEntity != null; } }

    public VisualDebugEntity SetISomeInterface(ExampleContent.VisualDebugging.ISomeInterface newValue) {
        if (HasISomeInterface) {
            throw new JCMG.EntitasRedux.EntitasReduxException("Could not set ISomeInterface!\n" + this + " already has an entity with ISomeInterfaceComponent!",
                "You should check if the context already has a ISomeInterfaceEntity before setting it or use context.ReplaceISomeInterface().");
        }
        var entity = CreateEntity();
        entity.AddISomeInterface(newValue);
        return entity;
    }

    public void ReplaceISomeInterface(ExampleContent.VisualDebugging.ISomeInterface newValue) {
        var entity = ISomeInterfaceEntity;
        if (entity == null) {
            entity = SetISomeInterface(newValue);
        } else {
            entity.ReplaceISomeInterface(newValue);
        }
    }

    public void RemoveISomeInterface() {
        ISomeInterfaceEntity.Destroy();
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
public partial class VisualDebugEntity {

    public ISomeInterfaceComponent ISomeInterface { get { return (ISomeInterfaceComponent)GetComponent(VisualDebugComponentsLookup.ISomeInterface); } }
    public bool HasISomeInterface { get { return HasComponent(VisualDebugComponentsLookup.ISomeInterface); } }

    public void AddISomeInterface(ExampleContent.VisualDebugging.ISomeInterface newValue) {
        var index = VisualDebugComponentsLookup.ISomeInterface;
        var component = (ISomeInterfaceComponent)CreateComponent(index, typeof(ISomeInterfaceComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceISomeInterface(ExampleContent.VisualDebugging.ISomeInterface newValue) {
        var index = VisualDebugComponentsLookup.ISomeInterface;
        var component = (ISomeInterfaceComponent)CreateComponent(index, typeof(ISomeInterfaceComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

	public void CopyISomeInterfaceTo(ISomeInterfaceComponent copyComponent) {
        var index = VisualDebugComponentsLookup.ISomeInterface;
        var component = (ISomeInterfaceComponent)CreateComponent(index, typeof(ISomeInterfaceComponent));
        component.value = copyComponent.value;
        ReplaceComponent(index, component);
    }

    public void RemoveISomeInterface() {
        RemoveComponent(VisualDebugComponentsLookup.ISomeInterface);
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
public sealed partial class VisualDebugMatcher {

    static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherISomeInterface;

    public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> ISomeInterface {
        get {
            if (_matcherISomeInterface == null) {
                var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.ISomeInterface);
                matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
                _matcherISomeInterface = matcher;
            }

            return _matcherISomeInterface;
        }
    }
}