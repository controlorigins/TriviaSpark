using TriviaSpark.Core.Utility;

namespace TriviaSpark.Core.Tests.Utility
{

    [TestClass]
    public class ListProviderTests
    {

        [TestMethod]
        public void Add_AddsAllItemsInIEnumerableToList()
        {
            // Arrange
            var provider = new ListProvider<string>();

            // Act
            provider.Add(new[] { "hello", "world" });

            // Assert
            Assert.AreEqual(2, provider.Count);
        }


        [TestMethod]
        public void Add_AddsItemToList()
        {
            // Arrange
            var provider = new ListProvider<string>();
            var item = "test";

            // Act
            provider.Add(item);

            // Assert
            Assert.AreEqual(1, provider.Count);
            Assert.AreEqual(item, provider.First);
        }


        [TestMethod]
        public void Add_AddsNewItemToList()
        {
            // Arrange
            var provider = new ListProvider<string>();

            // Act
            provider.Add("hello");

            // Assert
            Assert.AreEqual(1, provider.Count);
        }

        [TestMethod]
        public void Add_DoesNotAddDuplicateItemToList()
        {
            // Arrange
            var provider = new ListProvider<string>();

            // Act
            provider.Add("hello");
            provider.Add("hello");

            // Assert
            Assert.AreEqual(1, provider.Count);
        }

        [TestMethod]
        public void Add_DoesNotAddNullItemsInIEnumerableToList()
        {
            // Arrange
            var provider = new ListProvider<string>();

            // Act
            provider.Add(new[] { "hello", null });

            // Assert
            Assert.AreEqual(1, provider.Count);
        }




        [TestMethod]
        public void Add_InitializeList()
        {
            // Arrange
            var provider = new ListProvider<string>(new[] { "hello", "world" });

            // Act

            // Assert
            Assert.AreEqual(2, provider.Count);
        }

        [TestMethod]
        public void Add_InitializeListDuplicate()
        {
            // Arrange
            var provider = new ListProvider<string>(new[] { "hello", "world", "world" });

            // Act

            // Assert
            Assert.AreEqual(2, provider.Count);
        }


        [TestMethod]
        public void AddItem_AddNullItem()
        {
            // Arrange
            var provider = new ListProvider<string>();
            string? nullItem = null;
            provider.Add(nullItem);


            // Act
            var result = provider.Get(1);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [DataTestMethod]
        [DataRow(null, "hello", false)]
        [DataRow(new[] { "hello" }, "hello", true)]
        [DataRow(new[] { "hello", "world", "world", "hello", "world", "world" }, "world", true)]
        [DataRow(new[] { "hello", "world", "hello", "bob", null }, "bob", true)]
        [DataRow(new[] { "hello", "world", "bob", "john" }, "sam", false)]
        public void Clear_List(IEnumerable<string> strings, string test, bool result)
        {
            // Arrange
            var provider = new ListProvider<string>(strings);

            // Act
            provider.Clear();

            // Assert
            Assert.AreEqual(0, provider.Count);
        }

        [TestMethod]
        public void Clear_RemovesAllItemsFromList()
        {
            // Arrange
            var provider = new ListProvider<string>();
            provider.Add("hello");
            provider.Add("world");

            // Act
            provider.Clear();

            // Assert
            Assert.AreEqual(0, provider.Count);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(new[] { "hello" }, "hello")]
        [DataRow(new[] { "hello", "world", "world", "hello", "world", "world" }, "hello")]
        [DataRow(new[] { "hello", "world", "hello", "bob", null }, "hello")]
        [DataRow(new[] { null, "hello", "world", "bob", "john" }, "hello")]
        public void First_List(IEnumerable<string> strings, string test)
        {
            // Arrange
            var provider = new ListProvider<string>(strings);

            // Act
            var first = provider.First;

            // Assert
            Assert.AreEqual(first, test);
        }

        [TestMethod]
        public void Get_ReturnsEmptyListIfCountIsZero()
        {
            // Arrange
            var provider = new ListProvider<string>();
            provider.Add("hello");

            // Act
            var result = provider.Get(0);

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Get_ReturnsEmptyListIfListIsNull()
        {
            // Arrange
            var provider = new ListProvider<string>();

            // Act
            var result = provider.Get(10);

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Get_ReturnsListWithCountItemsOrLess()
        {
            // Arrange
            var provider = new ListProvider<string>();
            provider.Add("hello");
            provider.Add("world");

            // Act
            var result = provider.Get(1);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Get_ReturnsListWithoutExcludedItems()
        {
            // Arrange
            var provider = new ListProvider<string>();
            provider.Add("hello");
            provider.Add("world");
            var excluded = new[] { "hello" };

            // Act
            var result = provider.Get(excluded, 2);

            // Assert
            Assert.AreEqual(1, result?.Count() ?? 0);
            Assert.AreEqual("world", result?.First() ?? null);
        }


        private class Person : IComparable<Person>
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public int CompareTo(Person other)
            {
                return this.Name.CompareTo(other.Name);
            }
        }

        [TestMethod]
        public void TestGetRandom()
        {
            // Arrange
            ListProvider<Person> listProvider = new();
            listProvider.Add(new Person { Name = "Alice", Age = 30 });
            listProvider.Add(new Person { Name = "Bob", Age = 25 });
            listProvider.Add(new Person { Name = "Charlie", Age = 40 });

            // Act
            Person randomPerson = listProvider.GetRandom();

            // Assert
            Assert.IsNotNull(randomPerson);
        }

        [TestMethod]
        public void TestGetRandom_NullList()
        {
            // Arrange
            ListProvider<Person> listProvider = new();

            // Act
            Person? randomPerson = listProvider.GetRandom();

            // Assert
            Assert.IsNull(randomPerson);
        }

        [TestMethod]
        public void TestAdd()
        {
            var listProvider = new ListProvider<Person>();
            var person1 = new Person { Name = "Alice", Age = 30 };
            var person2 = new Person { Name = "Bob", Age = 40 };
            var person3 = new Person { Name = "Charlie", Age = 50 };

            Assert.AreEqual(1, listProvider.Add(person1));
            Assert.AreEqual(1, listProvider.Add(person2));
            Assert.AreEqual(1, listProvider.Add(person3));
            Assert.AreEqual(0, listProvider.Add(person1));
            Assert.AreEqual(0, listProvider.Add(person2));
            Assert.AreEqual(0, listProvider.Add(person3));
        }

        [TestMethod]
        public void TestAddRange()
        {
            var listProvider = new ListProvider<Person>();
            var people = new[]
            {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 40 },
            new Person { Name = "Charlie", Age = 50 }
        };

            Assert.AreEqual(3, listProvider.Add(people));
            Assert.AreEqual(0, listProvider.Add(people));
        }


        [DataTestMethod]
        [DataRow(null, null, 0)]
        [DataRow(null, new[] { "hello" }, 0)]
        [DataRow(new[] { "hello" }, null, 1)]
        [DataRow(new[] { "hello" }, new[] { "hello" }, 0)]
        [DataRow(new[] { "hello" }, new[] { "world" }, 1)]
        [DataRow(new[] { "hello", "world" }, new[] { "hello" }, 1)]
        public void GetExcluded_List(IEnumerable<string> source, IEnumerable<string> exclude, int result)
        {
            // Arrange
            var provider = new ListProvider<string>(source);

            // Act
            var getResult = provider.Get(exclude, 1);


            // Assert
            Assert.AreEqual(result, getResult.Count());
        }

        [TestMethod]
        public void Items_ReturnsAddedItems_WhenItemsAdded()
        {
            // Arrange
            var listProvider = new ListProvider<string>(new[] { "item1", "item2", "item3" });

            // Act
            var items = listProvider.Items;

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("item1", items.ElementAt(0));
            Assert.AreEqual("item2", items.ElementAt(1));
            Assert.AreEqual("item3", items.ElementAt(2));
        }
        [TestMethod]
        public void Items_ReturnsEmpty_WhenNoItemsAdded()
        {
            // Arrange
            var listProvider = new ListProvider<string>();

            // Act
            var items = listProvider.Items;

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count());
        }

        [DataTestMethod]
        [DataRow(null, 0)]
        [DataRow(new[] { "hello" }, 1)]
        [DataRow(new[] { "hello", "world", "world", "hello", "world", "world" }, 2)]
        [DataRow(new[] { "hello", "world", "hello", "bob", null }, 3)]
        [DataRow(new[] { "hello", "world", "bob", "john" }, 4)]
        public void ListProvider_InitializeList(IEnumerable<string> strings, int count)
        {
            // Arrange
            var provider = new ListProvider<string>(strings);

            // Act

            // Assert
            Assert.AreEqual(count, provider.Count);
        }

        [DataTestMethod]
        [DataRow(null, "hello", false)]
        [DataRow(new[] { "hello" }, "hello", true)]
        [DataRow(new[] { "hello", "world", "world", "hello", "world", "world" }, "world", true)]
        [DataRow(new[] { "hello", "world", "hello", "bob", null }, "bob", true)]
        [DataRow(new[] { "hello", "world", "bob", "john" }, "sam", false)]
        public void Remove_List(IEnumerable<string> strings, string test, bool result)
        {
            // Arrange
            var provider = new ListProvider<string>(strings);

            // Act
            var removed = provider.Remove(test);

            // Assert
            Assert.AreEqual(removed, result);
        }

        [TestMethod]
        public void Remove_RemovesItemFromList()
        {
            // Arrange
            var provider = new ListProvider<string>();
            provider.Add("hello");
            provider.Add("world");

            // Act
            var result = provider.Remove("hello");

            // Assert
            Assert.AreEqual(1, provider.Count);
            Assert.IsTrue(result);
        }
    }
}
