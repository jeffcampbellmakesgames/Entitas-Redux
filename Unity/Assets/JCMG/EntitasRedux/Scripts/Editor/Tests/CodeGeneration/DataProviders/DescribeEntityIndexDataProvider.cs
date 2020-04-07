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
using JCMG.EntitasRedux.Editor.Plugins;
using JCMG.Genesis.Editor;
using NUnit.Framework;
using UnityEngine;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeEntityIndexDataProvider
	{
		private GenesisSettings _settings;

		[SetUp]
		public void Setup()
		{
			_settings = ScriptableObject.CreateInstance<GenesisSettings>();

			var contextConfig = _settings.CreateAndConfigure<ContextSettingsConfig>();
			contextConfig.ContextNames = new[]
			{
				"ConfiguredContext"
			};
		}

		[NUnit.Framework.Test]
		public void CreatesDataForSingleEntityIndex()
		{
			var data = GetData<EntityIndexComponent, StandardComponent>();
			Assert.AreEqual(1, data.Length);

			var d = data[0];

			Assert.AreEqual(typeof(String), d.GetEntityIndexType().GetType());
			Assert.AreEqual("JCMG.EntitasRedux.EntityIndex", d.GetEntityIndexType());

			Assert.AreEqual(typeof(Boolean), d.IsCustom().GetType());
			Assert.IsFalse(d.IsCustom());

			Assert.AreEqual(typeof(String), d.GetEntityIndexName().GetType());
			Assert.AreEqual("EntityIndex", d.GetEntityIndexName());

			Assert.AreEqual(typeof(string[]), d.GetContextNames().GetType());
			Assert.AreEqual(2, d.GetContextNames().Length);
			Assert.AreEqual("Test", d.GetContextNames()[0]);
			Assert.AreEqual("Test2", d.GetContextNames()[1]);

			Assert.AreEqual(typeof(String), d.GetKeyType().GetType());
			Assert.AreEqual("string", d.GetKeyType());

			Assert.AreEqual(typeof(String), d.GetComponentType().GetType());
			Assert.AreEqual("EntitasRedux.Tests.EntityIndexComponent", d.GetComponentType());

			Assert.AreEqual(typeof(String), d.GetMemberName().GetType());
			Assert.AreEqual("value", d.GetMemberName());

			Assert.AreEqual(typeof(Boolean), d.GetHasMultiple().GetType());
			Assert.IsFalse(d.GetHasMultiple());
		}

		[NUnit.Framework.Test]
		public void CreatesDataForMultipleEntityIndex()
		{
			var data = GetData<MultipleEntityIndicesComponent, StandardComponent>();
			Assert.AreEqual(2, data.Length);

			Assert.AreEqual("MultipleEntityIndices", data[0].GetEntityIndexName());
			Assert.IsTrue(data[0].GetHasMultiple());

			Assert.AreEqual("MultipleEntityIndices", data[1].GetEntityIndexName());
			Assert.IsTrue(data[1].GetHasMultiple());
		}

		[NUnit.Framework.Test]
		public void CreatesDataForSinglePrimaryEntityIndex()
		{
			var data = GetData<PrimaryEntityIndexComponent, StandardComponent>();

			Assert.AreEqual(1, data.Length);
			var d = data[0];

			Assert.AreEqual("JCMG.EntitasRedux.PrimaryEntityIndex", d.GetEntityIndexType());
			Assert.IsFalse(d.IsCustom());
			Assert.AreEqual("PrimaryEntityIndex", d.GetEntityIndexName());
			Assert.AreEqual(1, d.GetContextNames().Length);
			Assert.AreEqual("Game", d.GetContextNames()[0]);
			Assert.AreEqual("string", d.GetKeyType());
			Assert.AreEqual("EntitasRedux.Tests.PrimaryEntityIndexComponent", d.GetComponentType());
			Assert.AreEqual("value", d.GetMemberName());
			Assert.IsFalse(d.GetHasMultiple());
		}

		[NUnit.Framework.Test]
		public void CreatesDataForMultiplePrimaryEntityIndex()
		{
			var data = GetData<MultiplePrimaryEntityIndicesComponent, StandardComponent>();

			Assert.AreEqual(2, data.Length);

			Assert.AreEqual("MultiplePrimaryEntityIndices", data[0].GetEntityIndexName());
			Assert.IsTrue(data[0].GetHasMultiple());

			Assert.AreEqual("MultiplePrimaryEntityIndices", data[1].GetEntityIndexName());
			Assert.IsTrue(data[1].GetHasMultiple());
		}

		[NUnit.Framework.Test]
		public void IgnoresAbstractComponents()
		{
			var data = GetData<AbstractEntityIndexComponent, StandardComponent>();
			Assert.AreEqual(0, data.Length);
		}

		[NUnit.Framework.Test]
		public void CreatesDataForCustomEntityIndexClass()
		{
			var data = GetData<CustomEntityIndex, StandardComponent>();

			Assert.AreEqual(1, data.Length);
			var d = data[0];

			Assert.AreEqual("EntitasRedux.Tests.CustomEntityIndex", d.GetEntityIndexType());
			Assert.IsTrue(d.IsCustom());
			Assert.AreEqual("EntitasReduxTestsCustomEntityIndex", d.GetEntityIndexName());
			Assert.AreEqual(1, d.GetContextNames().Length);
			Assert.AreEqual("Test", d.GetContextNames()[0]);

			var methods = d.GetCustomMethods();
			Assert.AreEqual(typeof(MethodData[]), methods.GetType());
			Assert.AreEqual(2, methods.Length);
		}

		[NUnit.Framework.Test]
		public void IgnoresNonIComponent()
		{
			var data = GetData<ClassWithEntitIndexAttribute, StandardComponent>();
			Assert.AreEqual(0, data.Length);
		}

		#region Configure

		[NUnit.Framework.Test]
		public void IgnoresNamespaces()
		{
			var data = GetData<EntityIndexComponent, StandardComponent>(_settings);
			Assert.AreEqual(1, data.Length);
			var d = data[0];

			Assert.AreEqual("EntityIndex", d.GetEntityIndexName());
		}

		[NUnit.Framework.Test]
		public void GetsDefaultContext()
		{
			var data = GetData<EntityIndexNoContextComponent, StandardComponent>(_settings);

			Assert.AreEqual(1, data.Length);
			var d = data[0];

			Assert.AreEqual(1, d.GetContextNames().Length);
			Assert.AreEqual("ConfiguredContext", d.GetContextNames()[0]);
		}

		#endregion

		#region Helpers

		private static EntityIndexData[] GetData<T1, T2>(GenesisSettings _settings = null)
		{
			var provider = new EntityIndexDataProvider(
				new Type[]
				{
					typeof(T1),
					typeof(T2)
				});
			if (_settings == null)
			{
				_settings = ScriptableObject.CreateInstance<GenesisSettings>();
				var contextConfig = _settings.CreateAndConfigure<ContextSettingsConfig>();
				contextConfig.ContextNames = new[]
				{
					"Game",
					"GameState"
				};
			}

			provider.Configure(_settings);

			return (EntityIndexData[])provider.GetData();
		}

		#endregion
	}
}
