//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class Test2EventSystems : Feature {

    public Test2EventSystems(Contexts contexts) {
        Add(new Test2AnyEventToGenerateAddedEventSystem(contexts)); // priority: 0
        Add(new Test2AnyMultipleContextStandardEventAddedEventSystem(contexts)); // priority: 0
        Add(new Test2AnyMultipleEventsStandardEventAddedEventSystem(contexts)); // priority: 1
        Add(new Test2MultipleEventsStandardEventRemovedEventSystem(contexts)); // priority: 2
    }
}