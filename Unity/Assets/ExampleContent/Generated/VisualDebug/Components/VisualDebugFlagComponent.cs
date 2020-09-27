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

    static readonly ExampleContent.VisualDebugging.FlagComponent FlagComponent = new ExampleContent.VisualDebugging.FlagComponent();

    public bool IsFlag {
        get { return HasComponent(VisualDebugComponentsLookup.Flag); }
        set {
            if (value != IsFlag) {
                var index = VisualDebugComponentsLookup.Flag;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : FlagComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
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
public sealed partial class VisualDebugMatcher {

    static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherFlag;

    public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> Flag {
        get {
            if (_matcherFlag == null) {
                var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.Flag);
                matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
                _matcherFlag = matcher;
            }

            return _matcherFlag;
        }
    }
}