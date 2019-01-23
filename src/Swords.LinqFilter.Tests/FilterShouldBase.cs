namespace Swords.LinqFilter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Swords.LinqFilter.Tests.Models;

    public abstract class FilterShouldBase
    {
        [TestMethod]
        public void Return_SameCollection_When_NoCriteriaIsAppended()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.ApplyTo(collection);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void Return_EmptyCollection_When_CriteriaIsNotMatched()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.Append(x => false)
                            .ApplyTo(collection);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Return_SameItems_When_CriteriaIsMatchedAlways()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.Append(x => true)
                            .ApplyTo(collection);
            Assert.AreEqual(collection.Count(), result.Count());
        }

        [TestMethod]
        public void Return_SameCollection_When_CriteriaIsNull()
        {
            var collection = GetStubCollection();
            var sut = CreateFilter<Stub>();
            var result = sut.Append(null).ApplyTo(collection);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void Return_SameCollection_When_FilterNameIsEmpty()
        {
            var collection = GetStubCollection();
            var sut = CreateFilter<Stub>();
            var result = sut.Append(string.Empty, (Stub entity, string value) => false).ApplyTo(collection);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void Return_SameCollection_When_FilterNameIsNotFound_And_UseFilterValueWithSingleValue()
        {
            var collection = GetStubCollection();
            var sut = CreateFilter<Stub>();
            var result = sut.Append("NonExistingFilter", (Stub entity, string value) => false).ApplyTo(collection);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void Return_SameCollection_When_FilterNameIsNotFound_And_UseFilterValueWithMultipleValues()
        {
            var collection = GetStubCollection();
            var sut = CreateFilter<Stub>();
            var result = sut.Append("NonExistingFilter", (Stub entity, string[] value) => false).ApplyTo(collection);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void Return_FilteredCollection_When_SearchByExactMatch()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.Append(x => x.Property2 == "Class")
                            .ApplyTo(collection);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Return_FilteredCollection_When_SearchByTwoMatchingConditions()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.Append(x => x.Property2 == "Class")
                            .Append(x => x.Property1 == 2)
                            .ApplyTo(collection);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Return_FilteredCollection_When_SearchByExistingFilterValue()
        {
            var filterValue = new FilterValue
            {
                Name = "FilterProperty2",
                Value = "Class"
            };

            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>(filterValue);
            var result = sut.Append("FilterProperty2", (entity, value) => entity.Property2 == value)
                            .ApplyTo(collection);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(collection.First(x => x.Property2 == filterValue.Value), result.First());
        }

        [TestMethod]
        public void Return_FilteredCollection_When_SearchByExistingMultipleFilterValues()
        {
            var filterValueCollection = new List<FilterValue>()
            {
                new FilterValue
                {
                    Name = "FilterProperty2",
                    Value = "Class"
                },
                new FilterValue
                {
                    Name = "FilterProperty1",
                    Value = "2"
                }
            };

            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>(filterValueCollection);
            var result = sut.Append("FilterProperty2", (entity, value) => entity.Property2 == value)
                            .ApplyTo(collection);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Return_FilteredCollection_When_SearchByExistingFilterValueWithMultipleValues()
        {
            var filterValueCollection = new List<FilterValue>()
            {
                new FilterValue
                {
                    Name = "FilterProperty2",
                    Value = "Class;State;Name"
                }
            };

            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>(filterValueCollection);
            var result = sut.Append("FilterProperty2",
                                    (Stub entity, string[] values) => values.Any(value => value.Equals(entity.Property2, StringComparison.InvariantCultureIgnoreCase)))
                            .ApplyTo(collection);
            Assert.AreEqual(2, result.Count());
        }

        protected abstract Filter<TEntity> CreateFilter<TEntity>() where TEntity : class;

        protected abstract Filter<TEntity> CreateFilter<TEntity>(FilterValue filterValue) where TEntity : class;

        protected abstract Filter<TEntity> CreateFilter<TEntity>(IEnumerable<FilterValue> filterValue) where TEntity : class;

        protected static IQueryable<Stub> GetStubCollection()
            => new List<Stub>
            {
                new Stub { Property1 = 1, Property2 = "Name", Property3 = -1 },
                new Stub { Property1 = 2, Property2 = "Class", Property3 = 50 },
                new Stub { Property1 = 3, Property2 = "Bug", Property3 = 150000 }
            }.AsQueryable();

        protected List<FilterValue> GetAutoExcludedFilterValueCollection()
            => new List<FilterValue>()
            {
                new FilterValue
                {
                    Name = "FilterProperty2",
                    Value = "Class"
                },

                    new FilterValue
                {
                    Name = "FilterProperty1",
                    Value = "1"
                }
            };
    }
}