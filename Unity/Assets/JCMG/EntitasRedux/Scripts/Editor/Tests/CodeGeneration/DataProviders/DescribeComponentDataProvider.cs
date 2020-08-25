/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using JCMG.EntitasRedux;
using JCMG.EntitasRedux.Editor.Plugins;
using JCMG.Genesis.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using EventType = JCMG.EntitasRedux.EventType;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeComponentDataProvider
	{
		private Type _type;
		private ComponentData _data;

		private GenesisSettings _settings;

		[SetUp]
		public void Setup()
		{
			_settings = ScriptableObject.CreateInstance<GenesisSettings>();
			var contextNamesConfig = _settings.CreateAndConfigure<ContextSettingsConfig>();
			contextNamesConfig.ContextNames = new[]
			{
				"Game",
				"GameState"
			};

			var assembliesConfig = _settings.CreateAndConfigure<AssembliesConfig>();
			assembliesConfig.WhiteListedAssemblies = new[]
			{
				"EntitasRedux.Tests"
			};
		}

		#region Component

		[NUnit.Framework.Test]
		public void GetsComponentData()
		{
			SetupMyNamespaceComponents();

			Assert.IsNotNull(_data);
		}

		[NUnit.Framework.Test]
		public void GetsComponentFullTypeName()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(String), _data.GetTypeName().GetType());
			Assert.AreEqual(_type.ToCompilableString(), _data.GetTypeName());
		}

		[NUnit.Framework.Test]
		public void GetsComponentContexts()
		{
			SetupMyNamespaceComponents();

			var contextNames = _data.GetContextNames();

			Assert.AreEqual(2, contextNames.Length);
			Assert.AreEqual("Test", contextNames[0]);
			Assert.AreEqual("Test2", contextNames[1]);
		}

		[NUnit.Framework.Test]
		public void SetsComponentFirstContextAsDefaultWhenComponentHasNoContext()
		{
			SetupMyNamespaceComponents();

			var contextNames = GetData<NoContextComponent>().GetContextNames();
			Assert.AreEqual(1, contextNames.Length);
			Assert.AreEqual("Game", contextNames[0]);
		}

		[NUnit.Framework.Test]
		public void GetsUniqueComponent()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(Boolean), _data.IsUnique().GetType());
			Assert.IsFalse(_data.IsUnique());
			Assert.IsTrue(GetData<UniqueStandardComponent>().IsUnique());
		}

		[NUnit.Framework.Test]
		public void GetsComponentMemberData()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(MemberData[]), _data.GetMemberData().GetType());
			Assert.AreEqual(1, _data.GetMemberData().Length);
			Assert.AreEqual("string", _data.GetMemberData()[0].type);
		}

		[NUnit.Framework.Test]
		public void GetsGenerateComponent()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(Boolean), _data.ShouldGenerateComponent().GetType());
			Assert.IsFalse(_data.ShouldGenerateComponent());
			Assert.IsFalse(_data.ContainsKey(ShouldGenerateComponentComponentDataExtension.COMPONENT_OBJECT_TYPE));
		}

		[NUnit.Framework.Test]
		public void GetsGenerateComponentIndex()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(Boolean), _data.ShouldGenerateIndex().GetType());
			Assert.IsTrue(_data.ShouldGenerateIndex());
			Assert.IsFalse(GetData<DontGenerateIndexComponent>().ShouldGenerateIndex());
		}

		[NUnit.Framework.Test]
		public void GetsGenerateComponentMethods()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(Boolean), _data.ShouldGenerateMethods().GetType());
			Assert.IsTrue(_data.ShouldGenerateMethods());
			Assert.IsFalse(GetData<DontGenerateMethodsComponent>().ShouldGenerateMethods());
		}

		[NUnit.Framework.Test]
		public void GetsFlagComponentPrefix()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(String), _data.GetFlagPrefix().GetType());
			Assert.AreEqual("is", _data.GetFlagPrefix());
			Assert.AreEqual("My", GetData<CustomPrefixFlagComponent>().GetFlagPrefix());
		}

		[NUnit.Framework.Test]
		public void GetsIsNotComponentEvent()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(Boolean), _data.IsEvent().GetType());
			Assert.IsFalse(_data.IsEvent());
		}


		[NUnit.Framework.Test]
		public void GetsIsComponentEvent()
		{
			SetupMyNamespaceComponents();

			Assert.IsTrue(GetData<StandardEventComponent>().IsEvent());
		}

		[NUnit.Framework.Test]
		public void GetsMultipleComponentEvents()
		{
			SetupMyNamespaceComponents();

			var d = GetData<MultipleEventsStandardEventComponent>();
			Assert.IsTrue(d.IsEvent());

			var eventData = d.GetEventData();
			Assert.AreEqual(2, eventData.Length);

			Assert.AreEqual(EventTarget.Any, eventData[0].eventTarget);
			Assert.AreEqual(EventType.Added, eventData[0].eventType);
			Assert.AreEqual(1, eventData[0].priority);
		}

		[NUnit.Framework.Test]
		public void GetsComponentEventTarget()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(EventTarget), GetData<StandardEventComponent>().GetEventData()[0].eventTarget.GetType());
			Assert.AreEqual(EventTarget.Any, GetData<StandardEventComponent>().GetEventData()[0].eventTarget);
			Assert.AreEqual(EventTarget.Self, GetData<StandardEntityEventComponent>().GetEventData()[0].eventTarget);
		}

		[NUnit.Framework.Test]
		public void GetsComponentEventType()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(EventTarget), GetData<StandardEntityEventComponent>().GetEventData()[0].eventTarget.GetType());
			Assert.AreEqual(EventTarget.Self, GetData<StandardEntityEventComponent>().GetEventData()[0].eventTarget);
			Assert.AreEqual(EventType.Removed, GetData<StandardEntityEventComponent>().GetEventData()[0].eventType);
		}

		[NUnit.Framework.Test]
		public void GetsComponentEventPriority()
		{
			SetupMyNamespaceComponents();

			Assert.AreEqual(typeof(int), GetData<StandardEventComponent>().GetEventData()[0].priority.GetType());
			Assert.AreEqual(1, GetData<StandardEntityEventComponent>().GetEventData()[0].priority);
		}

		[NUnit.Framework.Test]
		public void CreatesComponentDataForEventListeners()
		{
			SetupMyNamespaceComponents();

			var d = GetMultipleData<StandardEventComponent>();

			Assert.AreEqual(2, d.Length);
			Assert.IsFalse(d[1].IsEvent());
			Assert.AreEqual("AnyStandardEventAddedListenerComponent", d[1].GetTypeName());
			Assert.AreEqual(1, d[1].GetMemberData().Length);
			Assert.AreEqual("value", d[1].GetMemberData()[0].name);
			Assert.AreEqual("System.Collections.Generic.List<IAnyStandardEventAddedListener>", d[1].GetMemberData()[0].type);
		}

		[NUnit.Framework.Test]
		public void CreatesComponentDataForUniqueEventListeners()
		{
			SetupMyNamespaceComponents();

			var d = GetMultipleData<UniqueEventComponent>();

			Assert.AreEqual(2, d.Length);
			Assert.IsFalse(d[1].IsEvent());
			Assert.IsFalse(d[1].IsUnique());
		}

		[NUnit.Framework.Test]
		public void CreatesComponentDataForEventListenersWithMultipleContexts()
		{
			SetupMyNamespaceComponents();

			var d = GetMultipleData<MultipleContextStandardEventComponent>();
			Assert.AreEqual(3, d.Length);
			Assert.IsFalse(d[1].IsEvent());
			Assert.AreEqual("TestAnyMultipleContextStandardEventAddedListenerComponent", d[1].GetTypeName());
			Assert.AreEqual(1, d[1].GetMemberData().Length);
			Assert.AreEqual("value", d[1].GetMemberData()[0].name);
			Assert.AreEqual("System.Collections.Generic.List<ITestAnyMultipleContextStandardEventAddedListener>", d[1].GetMemberData()[0].type);

			Assert.IsFalse(d[2].IsEvent());
			Assert.AreEqual("Test2AnyMultipleContextStandardEventAddedListenerComponent", d[2].GetTypeName());
			Assert.AreEqual(1, d[2].GetMemberData().Length);
			Assert.AreEqual("value", d[2].GetMemberData()[0].name);
			Assert.AreEqual("System.Collections.Generic.List<ITest2AnyMultipleContextStandardEventAddedListener>", d[2].GetMemberData()[0].type);
		}

		[NUnit.Framework.Test]
		public static void DuplicateComponentNamesAreDetectedAndThrows()
		{
			// Ignore Debug.Log's
			LogAssert.ignoreFailingMessages = true;

			// Setup duplicate components, any component in this case will do.
			var contextNames = new[]
			{
				"Game"
			};
			var componentDataOne = new ComponentData();
			componentDataOne.SetTypeName(typeof(ClassToGenerate).FullName);
			componentDataOne.SetContextNames(contextNames);

			var componentDataTwo = new ComponentData();
			componentDataTwo.SetTypeName(typeof(ClassToGenerate).FullName);
			componentDataTwo.SetContextNames(contextNames);

			var componentData = new[]
			{
				componentDataOne,
				componentDataTwo
			};

			// This should throw a exception since we don't allow duplicate components with the same name
			// as that would cause compile errors at code generation.
			Assert.Throws<DuplicateComponentNameException>(() =>
				ComponentValidationTools.ValidateComponentData(componentData));
		}

		#endregion

		#region Non Component

		[NUnit.Framework.Test]
		public void GetsNonComponentData()
		{
			SetupNonComponent();

			Assert.IsNotNull(_data);
		}

		[NUnit.Framework.Test]
		public void GetsNonComponentFullTypeName()
		{
			SetupNonComponent();

			// Not the type, but the component that should be generated
			// See: no namespace
			Assert.AreEqual("ClassToGenerateComponent", _data.GetTypeName());
		}

		[NUnit.Framework.Test]
		public void GetsNonComponentContexts()
		{
			SetupNonComponent();

			var contextNames = _data.GetContextNames();
			Assert.AreEqual(2, contextNames.Length);
			Assert.AreEqual("Test", contextNames[0]);
			Assert.AreEqual("Test2", contextNames[1]);
		}

		[NUnit.Framework.Test]
		public void GetsUniqueNonComponent()
		{
			SetupNonComponent();

			Assert.IsFalse(_data.IsUnique());
		}

		[NUnit.Framework.Test]
		public void GetsNonComponentMemberData()
		{
			SetupNonComponent();

			Assert.AreEqual(1, _data.GetMemberData().Length);
			Assert.AreEqual(_type.ToCompilableString(), _data.GetMemberData()[0].type);
		}

		[NUnit.Framework.Test]
		public void GetsGenerateNonComponent()
		{
			SetupNonComponent();

			Assert.AreEqual(typeof(Boolean), _data.ShouldGenerateComponent().GetType());
			Assert.IsTrue(_data.ShouldGenerateComponent());
			Assert.AreEqual(typeof(ClassToGenerate).ToCompilableString(), _data.GetObjectTypeName());
		}

		[NUnit.Framework.Test]
		public void GeneratesNonComponentIndex()
		{
			SetupNonComponent();

			Assert.IsTrue(_data.ShouldGenerateIndex());
		}

		[NUnit.Framework.Test]
		public void GetsGenerateNonComponentMethods()
		{
			SetupNonComponent();

			Assert.IsTrue(_data.ShouldGenerateMethods());
		}

		[NUnit.Framework.Test]
		public void GetsNonComponentFlagPrefix()
		{
			SetupNonComponent();

			Assert.AreEqual("is", _data.GetFlagPrefix());
		}

		[NUnit.Framework.Test]
		public void GetsIsNotNonComponentEvent()
		{
			SetupNonComponent();

			Assert.IsFalse(_data.IsEvent());
		}

		[NUnit.Framework.Test]
		public void GetsNonComponentEvent()
		{
			SetupNonComponent();

			Assert.AreEqual(1, GetData<EventToGenerate>().GetEventData().Length);

			var eventData = GetData<EventToGenerate>().GetEventData()[0];
			Assert.AreEqual(EventTarget.Any, eventData.eventTarget);
			Assert.AreEqual(EventType.Added, eventData.eventType);
			Assert.AreEqual(0, eventData.priority);
		}

		[NUnit.Framework.Test]
		public void CreatesNonComponentDataForEventListeners()
		{
			SetupNonComponent();

			var d = GetMultipleData<EventToGenerate>();
			Assert.AreEqual(3, d.Length);
			Assert.IsFalse(d[1].IsEvent());
			Assert.IsFalse(d[1].ShouldGenerateComponent());
			Assert.AreEqual("TestAnyEventToGenerateAddedListenerComponent", d[1].GetTypeName());
			Assert.AreEqual(1, d[1].GetMemberData().Length);
			Assert.AreEqual("value", d[1].GetMemberData()[0].name);
			Assert.AreEqual("System.Collections.Generic.List<ITestAnyEventToGenerateAddedListener>", d[1].GetMemberData()[0].type);

			Assert.IsFalse(d[2].IsEvent());
			Assert.IsFalse(d[2].ShouldGenerateComponent());
			Assert.AreEqual("Test2AnyEventToGenerateAddedListenerComponent", d[2].GetTypeName());
			Assert.AreEqual(1, d[2].GetMemberData().Length);
			Assert.AreEqual("value", d[2].GetMemberData()[0].name);
			Assert.AreEqual("System.Collections.Generic.List<ITest2AnyEventToGenerateAddedListener>", d[2].GetMemberData()[0].type);
		}

		#endregion

		#region Multiple Types

		[NUnit.Framework.Test]
		public void CreatesDataForMultipleTypes()
		{
			var types = new[] { typeof(NameAgeComponent), typeof(Test2ContextComponent) };
			var provider = new ComponentDataProvider(types);
			provider.Configure(_settings);
			var data = provider.GetData();

			Assert.AreEqual(types.Length, data.Length);
		}

		[NUnit.Framework.Test]
		public void IgnoresDuplicatesFromNonComponents()
		{
			var types = new[] { typeof(ClassToGenerate), typeof(ClassToGenerateComponent) };
			var provider = new ComponentDataProvider(types);
			provider.Configure(_settings);
			var data = provider.GetData();

			Assert.AreEqual(1, data.Length);
		}

		#endregion

		#region Multiple Custom Component Names

		[NUnit.Framework.Test]
		public void CreatesDataForEachCustomComponentName()
		{
			var type = typeof(CustomName);
			var data = GetMultipleData<CustomName>();
			var data1 = data[0];
			var data2 = data[1];

			Assert.AreEqual(type.ToCompilableString(), data1.GetObjectTypeName());
			Assert.AreEqual(type.ToCompilableString(), data2.GetObjectTypeName());

			Assert.AreEqual("NewCustomNameComponent1Component", data1.GetTypeName());
			Assert.AreEqual("NewCustomNameComponent2Component", data2.GetTypeName());
		}

		#endregion

		#region Configuration

		[NUnit.Framework.Test]
		public void GetsDefaultContext()
		{
			var settings = ScriptableObject.CreateInstance<GenesisSettings>();
			var contextConfig = settings.CreateAndConfigure<ContextSettingsConfig>();
			contextConfig.ContextNames = new[]
			{
				"ConfiguredContext"
			};

			var type = typeof(NoContextComponent);
			var data = GetData<NoContextComponent>(settings);

			var contextNames = data.GetContextNames();
			Assert.AreEqual(1, contextNames.Length);
			Assert.AreEqual("ConfiguredContext", contextNames[0]);

		}

		#endregion

		#region Helpers

		private void SetupMyNamespaceComponents()
		{
			_type = typeof(MyNamespaceComponent);
			_data = GetData<MyNamespaceComponent>();
		}

		private void SetupNonComponent()
		{
			_type = typeof(ClassToGenerate);
			_data = GetData<ClassToGenerate>();
		}

		private static ComponentData GetData<T>(GenesisSettings settings = null)
		{
			return GetMultipleData<T>(settings)[0];
		}

		private static ComponentData[] GetMultipleData<T>(GenesisSettings settings = null)
		{
			var provider = new ComponentDataProvider(
				new Type[]
				{
					typeof(T)
				});
			if (settings == null)
			{
				settings = ScriptableObject.CreateInstance<GenesisSettings>();
				var contextConfig = settings.CreateAndConfigure<ContextSettingsConfig>();
				contextConfig.ContextNames = new[]
				{
					"Game",
					"GameState"
				};

				var assembliesConfig = settings.CreateAndConfigure<AssembliesConfig>();
				assembliesConfig.DoUseWhitelistOfAssemblies = true;
				assembliesConfig.WhiteListedAssemblies = new[]
				{
					"EntitasRedux.Tests"
				};
			}

			provider.Configure(settings);

			return (ComponentData[])provider.GetData();
		}

		#endregion
	}
}
