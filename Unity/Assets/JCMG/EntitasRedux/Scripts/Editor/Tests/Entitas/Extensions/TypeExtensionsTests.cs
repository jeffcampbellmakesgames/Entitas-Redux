using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JCMG.EntitasRedux.Editor.Plugins;
using NUnit.Framework;

namespace EntitasRedux.Tests.Extensions
{
	[TestFixture]
	internal sealed class TypeExtensionsTests
	{
		#region Fixtures

		private class FooList : List<int> { };

		private class BadFooList : IList<int>
		{
			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			public IEnumerator<int> GetEnumerator()
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
			public void Add(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only. </exception>
			public void Clear()
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
			/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			/// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
			public bool Contains(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// <paramref name="arrayIndex" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(int[] array, int arrayIndex)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			/// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
			public bool Remove(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			public int Count { get; }

			/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
			/// <returns>
			/// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.</returns>
			public bool IsReadOnly { get; }

			/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.</summary>
			/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
			/// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
			public int IndexOf(int item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.</summary>
			/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
			/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
			public void Insert(int index, int item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
			public void RemoveAt(int index)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Gets or sets the element at the specified index.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The element at the specified index.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
			/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
			public int this[int index]
			{
				get => throw new System.NotImplementedException();
				set => throw new System.NotImplementedException();
			}
		}

		private class FooDictionary : Dictionary<int, string> { }

		private class BadFooDictionary : IDictionary<int, string>
		{
			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
			public void Add(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only. </exception>
			public void Clear()
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
			/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			/// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
			public bool Contains(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// <paramref name="arrayIndex" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(KeyValuePair<int, string>[] array, int arrayIndex)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			/// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
			public bool Remove(KeyValuePair<int, string> item)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			public int Count { get; }

			/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
			/// <returns>
			/// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.</returns>
			public bool IsReadOnly { get; }

			/// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
			/// <param name="key">The object to use as the key of the element to add.</param>
			/// <param name="value">The object to use as the value of the element to add.</param>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="key" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</exception>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.</exception>
			public void Add(int key, string value)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.</summary>
			/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
			/// <returns>
			/// <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="key" /> is <see langword="null" />.</exception>
			public bool ContainsKey(int key)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
			/// <param name="key">The key of the element to remove.</param>
			/// <returns>
			/// <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="key" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.</exception>
			public bool Remove(int key)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Gets the value associated with the specified key.</summary>
			/// <param name="key">The key whose value to get.</param>
			/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
			/// <returns>
			/// <see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="key" /> is <see langword="null" />.</exception>
			public bool TryGetValue(int key, out string value)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>Gets or sets the element with the specified key.</summary>
			/// <param name="key">The key of the element to get or set.</param>
			/// <returns>The element with the specified key.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="key" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> is not found.</exception>
			/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.</exception>
			public string this[int key]
			{
				get => throw new System.NotImplementedException();
				set => throw new System.NotImplementedException();
			}

			/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
			/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
			public ICollection<int> Keys { get; }

			/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
			/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
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
