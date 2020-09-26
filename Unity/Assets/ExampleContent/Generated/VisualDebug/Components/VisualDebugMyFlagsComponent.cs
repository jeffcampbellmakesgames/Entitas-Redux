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

    public ExampleContent.VisualDebugging.MyFlagsComponent MyFlags { get { return (ExampleContent.VisualDebugging.MyFlagsComponent)GetComponent(VisualDebugComponentsLookup.MyFlags); } }
    public bool HasMyFlags { get { return HasComponent(VisualDebugComponentsLookup.MyFlags); } }

    public void AddMyFlags(ExampleContent.VisualDebugging.MyFlagsComponent.MyFlags newMyFlags) {
        var index = VisualDebugComponentsLookup.MyFlags;
        var component = (ExampleContent.VisualDebugging.MyFlagsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyFlagsComponent));
        component.myFlags = newMyFlags;
        AddComponent(index, component);
    }

    public void ReplaceMyFlags(ExampleContent.VisualDebugging.MyFlagsComponent.MyFlags newMyFlags) {
        var index = VisualDebugComponentsLookup.MyFlags;
        var component = (ExampleContent.VisualDebugging.MyFlagsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyFlagsComponent));
        component.myFlags = newMyFlags;
        ReplaceComponent(index, component);
    }

	public void CopyMyFlagsTo(ExampleContent.VisualDebugging.MyFlagsComponent copyComponent) {
        var index = VisualDebugComponentsLookup.MyFlags;
        var component = (ExampleContent.VisualDebugging.MyFlagsComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyFlagsComponent));
        component.myFlags = copyComponent.myFlags;
        ReplaceComponent(index, component);
    }

    public void RemoveMyFlags() {
        RemoveComponent(VisualDebugComponentsLookup.MyFlags);
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

    static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyFlags;

    public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyFlags {
        get {
            if (_matcherMyFlags == null) {
                var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyFlags);
                matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
                _matcherMyFlags = matcher;
            }

            return _matcherMyFlags;
        }
    }
}
