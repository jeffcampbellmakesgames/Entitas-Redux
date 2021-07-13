using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class VisualDebugComponentsLookup
{
	public const int AnyMyEventAddedListener = 0;
	public const int AnyMyEventClassAddedListener = 1;
	public const int CoolName = 2;
	public const int AnArray = 3;
	public const int AnCleanupDestroyEntity = 4;
	public const int AnCleanupRemove = 5;
	public const int AnimationCurve = 6;
	public const int Array2D = 7;
	public const int Array3D = 8;
	public const int Bounds = 9;
	public const int Color = 10;
	public const int CustomFlag = 11;
	public const int CustomObject = 12;
	public const int CustomObjectDictionary = 13;
	public const int CustomObjectList = 14;
	public const int DateTime = 15;
	public const int DictArray = 16;
	public const int Dictionary = 17;
	public const int DictList = 18;
	public const int DontDrawSimpleObject = 19;
	public const int Flag = 20;
	public const int GameObject = 21;
	public const int HashSet = 22;
	public const int IndexedEntity = 23;
	public const int IndexedPrimary = 24;
	public const int JaggedArray = 25;
	public const int ListArray = 26;
	public const int List = 27;
	public const int ManyMembers = 28;
	public const int MonoBehaviourSubClass = 29;
	public const int MyBool = 30;
	public const int MyChar = 31;
	public const int MyDouble = 32;
	public const int MyEnum = 33;
	public const int MyEvent = 34;
	public const int MyFlags = 35;
	public const int MyFloat = 36;
	public const int MyHiddenInt = 37;
	public const int MyInt = 38;
	public const int MyString = 39;
	public const int Person = 40;
	public const int Property = 41;
	public const int Rect = 42;
	public const int SimpleObject = 43;
	public const int SystemObject = 44;
	public const int Texture2D = 45;
	public const int Texture = 46;
	public const int Unique = 47;
	public const int UnityObject = 48;
	public const int UnsupportedObject = 49;
	public const int Vector2 = 50;
	public const int Vector3 = 51;
	public const int Vector4 = 52;
	public const int ISomeInterface = 53;
	public const int MyEventClass = 54;
	public const int Position = 55;
	public const int SomeClass = 56;
	public const int SomeOtherClass = 57;
	public const int SomeStruct = 58;
	public const int Velocity = 59;

	public const int TotalComponents = 60;

	public static readonly string[] ComponentNames =
	{
		"AnyMyEventAddedListener",
		"AnyMyEventClassAddedListener",
		"CoolName",
		"AnArray",
		"AnCleanupDestroyEntity",
		"AnCleanupRemove",
		"AnimationCurve",
		"Array2D",
		"Array3D",
		"Bounds",
		"Color",
		"CustomFlag",
		"CustomObject",
		"CustomObjectDictionary",
		"CustomObjectList",
		"DateTime",
		"DictArray",
		"Dictionary",
		"DictList",
		"DontDrawSimpleObject",
		"Flag",
		"GameObject",
		"HashSet",
		"IndexedEntity",
		"IndexedPrimary",
		"JaggedArray",
		"ListArray",
		"List",
		"ManyMembers",
		"MonoBehaviourSubClass",
		"MyBool",
		"MyChar",
		"MyDouble",
		"MyEnum",
		"MyEvent",
		"MyFlags",
		"MyFloat",
		"MyHiddenInt",
		"MyInt",
		"MyString",
		"Person",
		"Property",
		"Rect",
		"SimpleObject",
		"SystemObject",
		"Texture2D",
		"Texture",
		"Unique",
		"UnityObject",
		"UnsupportedObject",
		"Vector2",
		"Vector3",
		"Vector4",
		"ISomeInterface",
		"MyEventClass",
		"Position",
		"SomeClass",
		"SomeOtherClass",
		"SomeStruct",
		"Velocity"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(AnyMyEventAddedListenerComponent),
		typeof(AnyMyEventClassAddedListenerComponent),
		typeof(CoolNameComponent),
		typeof(ExampleContent.VisualDebugging.AnArrayComponent),
		typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent),
		typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent),
		typeof(ExampleContent.VisualDebugging.AnimationCurveComponent),
		typeof(ExampleContent.VisualDebugging.Array2DComponent),
		typeof(ExampleContent.VisualDebugging.Array3DComponent),
		typeof(ExampleContent.VisualDebugging.BoundsComponent),
		typeof(ExampleContent.VisualDebugging.ColorComponent),
		typeof(ExampleContent.VisualDebugging.CustomFlagComponent),
		typeof(ExampleContent.VisualDebugging.CustomObjectComponent),
		typeof(ExampleContent.VisualDebugging.CustomObjectDictionaryComponent),
		typeof(ExampleContent.VisualDebugging.CustomObjectListComponent),
		typeof(ExampleContent.VisualDebugging.DateTimeComponent),
		typeof(ExampleContent.VisualDebugging.DictArrayComponent),
		typeof(ExampleContent.VisualDebugging.DictionaryComponent),
		typeof(ExampleContent.VisualDebugging.DictListComponent),
		typeof(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent),
		typeof(ExampleContent.VisualDebugging.FlagComponent),
		typeof(ExampleContent.VisualDebugging.GameObjectComponent),
		typeof(ExampleContent.VisualDebugging.HashSetComponent),
		typeof(ExampleContent.VisualDebugging.IndexedEntityComponent),
		typeof(ExampleContent.VisualDebugging.IndexedPrimaryComponent),
		typeof(ExampleContent.VisualDebugging.JaggedArrayComponent),
		typeof(ExampleContent.VisualDebugging.ListArrayComponent),
		typeof(ExampleContent.VisualDebugging.ListComponent),
		typeof(ExampleContent.VisualDebugging.ManyMembersComponent),
		typeof(ExampleContent.VisualDebugging.MonoBehaviourSubClassComponent),
		typeof(ExampleContent.VisualDebugging.MyBoolComponent),
		typeof(ExampleContent.VisualDebugging.MyCharComponent),
		typeof(ExampleContent.VisualDebugging.MyDoubleComponent),
		typeof(ExampleContent.VisualDebugging.MyEnumComponent),
		typeof(ExampleContent.VisualDebugging.MyEventComponent),
		typeof(ExampleContent.VisualDebugging.MyFlagsComponent),
		typeof(ExampleContent.VisualDebugging.MyFloatComponent),
		typeof(ExampleContent.VisualDebugging.MyHiddenIntComponent),
		typeof(ExampleContent.VisualDebugging.MyIntComponent),
		typeof(ExampleContent.VisualDebugging.MyStringComponent),
		typeof(ExampleContent.VisualDebugging.PersonComponent),
		typeof(ExampleContent.VisualDebugging.PropertyComponent),
		typeof(ExampleContent.VisualDebugging.RectComponent),
		typeof(ExampleContent.VisualDebugging.SimpleObjectComponent),
		typeof(ExampleContent.VisualDebugging.SystemObjectComponent),
		typeof(ExampleContent.VisualDebugging.Texture2DComponent),
		typeof(ExampleContent.VisualDebugging.TextureComponent),
		typeof(ExampleContent.VisualDebugging.UniqueComponent),
		typeof(ExampleContent.VisualDebugging.UnityObjectComponent),
		typeof(ExampleContent.VisualDebugging.UnsupportedObjectComponent),
		typeof(ExampleContent.VisualDebugging.Vector2Component),
		typeof(ExampleContent.VisualDebugging.Vector3Component),
		typeof(ExampleContent.VisualDebugging.Vector4Component),
		typeof(ISomeInterfaceComponent),
		typeof(MyEventClassComponent),
		typeof(PositionComponent),
		typeof(SomeClassComponent),
		typeof(SomeOtherClassComponent),
		typeof(SomeStructComponent),
		typeof(VelocityComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(AnyMyEventAddedListenerComponent), 0 },
		{ typeof(AnyMyEventClassAddedListenerComponent), 1 },
		{ typeof(CoolNameComponent), 2 },
		{ typeof(ExampleContent.VisualDebugging.AnArrayComponent), 3 },
		{ typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent), 4 },
		{ typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent), 5 },
		{ typeof(ExampleContent.VisualDebugging.AnimationCurveComponent), 6 },
		{ typeof(ExampleContent.VisualDebugging.Array2DComponent), 7 },
		{ typeof(ExampleContent.VisualDebugging.Array3DComponent), 8 },
		{ typeof(ExampleContent.VisualDebugging.BoundsComponent), 9 },
		{ typeof(ExampleContent.VisualDebugging.ColorComponent), 10 },
		{ typeof(ExampleContent.VisualDebugging.CustomFlagComponent), 11 },
		{ typeof(ExampleContent.VisualDebugging.CustomObjectComponent), 12 },
		{ typeof(ExampleContent.VisualDebugging.CustomObjectDictionaryComponent), 13 },
		{ typeof(ExampleContent.VisualDebugging.CustomObjectListComponent), 14 },
		{ typeof(ExampleContent.VisualDebugging.DateTimeComponent), 15 },
		{ typeof(ExampleContent.VisualDebugging.DictArrayComponent), 16 },
		{ typeof(ExampleContent.VisualDebugging.DictionaryComponent), 17 },
		{ typeof(ExampleContent.VisualDebugging.DictListComponent), 18 },
		{ typeof(ExampleContent.VisualDebugging.DontDrawSimpleObjectComponent), 19 },
		{ typeof(ExampleContent.VisualDebugging.FlagComponent), 20 },
		{ typeof(ExampleContent.VisualDebugging.GameObjectComponent), 21 },
		{ typeof(ExampleContent.VisualDebugging.HashSetComponent), 22 },
		{ typeof(ExampleContent.VisualDebugging.IndexedEntityComponent), 23 },
		{ typeof(ExampleContent.VisualDebugging.IndexedPrimaryComponent), 24 },
		{ typeof(ExampleContent.VisualDebugging.JaggedArrayComponent), 25 },
		{ typeof(ExampleContent.VisualDebugging.ListArrayComponent), 26 },
		{ typeof(ExampleContent.VisualDebugging.ListComponent), 27 },
		{ typeof(ExampleContent.VisualDebugging.ManyMembersComponent), 28 },
		{ typeof(ExampleContent.VisualDebugging.MonoBehaviourSubClassComponent), 29 },
		{ typeof(ExampleContent.VisualDebugging.MyBoolComponent), 30 },
		{ typeof(ExampleContent.VisualDebugging.MyCharComponent), 31 },
		{ typeof(ExampleContent.VisualDebugging.MyDoubleComponent), 32 },
		{ typeof(ExampleContent.VisualDebugging.MyEnumComponent), 33 },
		{ typeof(ExampleContent.VisualDebugging.MyEventComponent), 34 },
		{ typeof(ExampleContent.VisualDebugging.MyFlagsComponent), 35 },
		{ typeof(ExampleContent.VisualDebugging.MyFloatComponent), 36 },
		{ typeof(ExampleContent.VisualDebugging.MyHiddenIntComponent), 37 },
		{ typeof(ExampleContent.VisualDebugging.MyIntComponent), 38 },
		{ typeof(ExampleContent.VisualDebugging.MyStringComponent), 39 },
		{ typeof(ExampleContent.VisualDebugging.PersonComponent), 40 },
		{ typeof(ExampleContent.VisualDebugging.PropertyComponent), 41 },
		{ typeof(ExampleContent.VisualDebugging.RectComponent), 42 },
		{ typeof(ExampleContent.VisualDebugging.SimpleObjectComponent), 43 },
		{ typeof(ExampleContent.VisualDebugging.SystemObjectComponent), 44 },
		{ typeof(ExampleContent.VisualDebugging.Texture2DComponent), 45 },
		{ typeof(ExampleContent.VisualDebugging.TextureComponent), 46 },
		{ typeof(ExampleContent.VisualDebugging.UniqueComponent), 47 },
		{ typeof(ExampleContent.VisualDebugging.UnityObjectComponent), 48 },
		{ typeof(ExampleContent.VisualDebugging.UnsupportedObjectComponent), 49 },
		{ typeof(ExampleContent.VisualDebugging.Vector2Component), 50 },
		{ typeof(ExampleContent.VisualDebugging.Vector3Component), 51 },
		{ typeof(ExampleContent.VisualDebugging.Vector4Component), 52 },
		{ typeof(ISomeInterfaceComponent), 53 },
		{ typeof(MyEventClassComponent), 54 },
		{ typeof(PositionComponent), 55 },
		{ typeof(SomeClassComponent), 56 },
		{ typeof(SomeOtherClassComponent), 57 },
		{ typeof(SomeStructComponent), 58 },
		{ typeof(VelocityComponent), 59 }
	};

	/// <summary>
	/// Returns a component index based on the passed <paramref name="component"/> type; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="component"></param>
	public static int GetComponentIndex(IComponent component)
	{
		return GetComponentIndex(component.GetType());
	}

	/// <summary>
	/// Returns a component index based on the passed <paramref name="componentType"/>; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="componentType"></param>
	public static int GetComponentIndex(Type componentType)
	{
		return ComponentTypeToIndex.TryGetValue(componentType, out var index) ? index : -1;
	}
}
