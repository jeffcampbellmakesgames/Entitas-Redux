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

    public ExampleContent.VisualDebugging.MyBoolComponent MyBool { get { return (ExampleContent.VisualDebugging.MyBoolComponent)GetComponent(VisualDebugComponentsLookup.MyBool); } }
    public bool HasMyBool { get { return HasComponent(VisualDebugComponentsLookup.MyBool); } }

    public void AddMyBool(bool newMyBool) {
        var index = VisualDebugComponentsLookup.MyBool;
        var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
        component.myBool = newMyBool;
        AddComponent(index, component);
    }

    public void ReplaceMyBool(bool newMyBool) {
        var index = VisualDebugComponentsLookup.MyBool;
        var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
        component.myBool = newMyBool;
        ReplaceComponent(index, component);
    }

	public void CopyMyBoolTo(ExampleContent.VisualDebugging.MyBoolComponent copyComponent) {
        var index = VisualDebugComponentsLookup.MyBool;
        var component = (ExampleContent.VisualDebugging.MyBoolComponent)CreateComponent(index, typeof(ExampleContent.VisualDebugging.MyBoolComponent));
        component.myBool = copyComponent.myBool;
        ReplaceComponent(index, component);
    }

    public void RemoveMyBool() {
        RemoveComponent(VisualDebugComponentsLookup.MyBool);
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

    static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherMyBool;

    public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> MyBool {
        get {
            if (_matcherMyBool == null) {
                var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.MyBool);
                matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
                _matcherMyBool = matcher;
            }

            return _matcherMyBool;
        }
    }
}