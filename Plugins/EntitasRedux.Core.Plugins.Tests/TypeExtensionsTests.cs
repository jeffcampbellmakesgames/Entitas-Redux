using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace EntitasRedux.Core.Plugins.Tests
{
	[TestFixture]
	internal sealed class TypeExtensionsTests
	{
		#region Fixtures

		private class FooList : List<int> { };

		private class BadFooList : IList<int>
		{
			/// <inheritdoc />
			public IEnumerator<int> GetEnumerator()
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			/// <inheritdoc />
			public void Add(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void Clear()
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool Contains(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void CopyTo(int[] array, int arrayIndex)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool Remove(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public int Count { get; }

			/// <inheritdoc />
			public bool IsReadOnly { get; }

			/// <inheritdoc />
			public int IndexOf(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void Insert(int index, int item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void RemoveAt(int index)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public int this[int index]
			{
				get => throw new System.NotImplementedException();
				set => throw new System.NotImplementedException();
			}
		}

		private class FooDictionary : Dictionary<int, string> { }

		private class BadFooDictionary : IDictionary<int, string>
		{
			/// <inheritdoc />
			public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			/// <inheritdoc />
			public void Add(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void Clear()
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool Contains(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public void CopyTo(KeyValuePair<int, string>[] array, int arrayIndex)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool Remove(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public int Count { get; }

			/// <inheritdoc />
			public bool IsReadOnly { get; }

			/// <inheritdoc />
			public void Add(int key, string value)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool ContainsKey(int key)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool Remove(int key)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public bool TryGetValue(int key, out string value)
			{
				throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public string this[int key]
			{
				get => throw new System.NotImplementedException();
				set => throw new System.NotImplementedException();
			}

			/// <inheritdoc />
			public ICollection<int> Keys { get; }

			/// <inheritdoc />
			public ICollection<string> Values { get; }
		}

		private class HasEmptyConstructor { }

		private class DoesNotHaveEmptyConstructor
		{
			public DoesNotHaveEmptyConstructor(int foo)
			{

			}
		}

		private struct ValueTypeObject { }

		#endregion

		[NUnit.Framework.Test]
		public void IsListReturnsTrueForList()
		{
			var list = new List<int>();

			Assert.IsTrue(list.GetType().IsList(out var genericType));
			Assert.AreEqual(typeof(int), genericType);
		}

		[NUnit.Framework.Test]
		public void IsListReturnsTrueForTypeDerivedFromList()
		{
			var list = new FooList();

			Assert.IsTrue(list.GetType().IsList(out var genericType));
			Assert.AreEqual(typeof(int), genericType);
		}

		[NUnit.Framework.Test]
		public void IsListReturnsFalseForTypeDerivedFromIList()
		{
			var list = new BadFooList();

			Assert.IsFalse(list.GetType().IsList(out var genericType));
		}

		[NUnit.Framework.Test]
		public void IsDictionaryReturnsTrueForDictionary()
		{
			var list = new Dictionary<int, string>();

			Assert.IsTrue(list.GetType().IsDictionary(out var genericKeyType, out var genericValueType));
			Assert.AreEqual(typeof(int), genericKeyType);
			Assert.AreEqual(typeof(string), genericValueType);
		}

		[NUnit.Framework.Test]
		public void IsDictionaryReturnsTrueForTypeDerivedFromDictionary()
		{
			var list = new FooDictionary();

			Assert.IsTrue(list.GetType().IsDictionary(out var genericKeyType, out var genericValueType));
			Assert.AreEqual(typeof(int), genericKeyType);
			Assert.AreEqual(typeof(string), genericValueType);
		}

		[NUnit.Framework.Test]
		public void IsDictionaryReturnsFalseForTypeDerivedFromIDictionary()
		{
			var list = new BadFooDictionary();

			Assert.IsFalse(list.GetType().IsDictionary(out var genericKeyType, out var genericValueType));
		}

		[NUnit.Framework.Test]
		public void HasDefaultConstructorWorksAsExpected()
		{
			Assert.IsTrue(typeof(HasEmptyConstructor).HasDefaultConstructor());
			Assert.IsFalse(typeof(DoesNotHaveEmptyConstructor).HasDefaultConstructor());
		}

		[NUnit.Framework.Test]
		public void IsMutableReferenceTypeCanDetermineRefTypes()
		{
			Assert.IsTrue(typeof(FooList).IsMutableReferenceType());
			Assert.IsTrue(typeof(List<int>).IsMutableReferenceType());
		}

		[NUnit.Framework.Test]
		public void IsMutableReferenceTypeCanDetermineValueTypes()
		{
			Assert.IsFalse(typeof(int).IsMutableReferenceType());
			Assert.IsFalse(typeof(ValueTypeObject).IsMutableReferenceType());
			Assert.IsFalse(typeof(string).IsMutableReferenceType());
		}
	}
}
