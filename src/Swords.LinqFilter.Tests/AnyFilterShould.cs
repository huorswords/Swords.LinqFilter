namespace Swords.LinqFilter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Swords.LinqFilter.Tests.Models;

    [TestClass]
    public class AnyFilterShould : FilterShouldBase
    {
        [TestMethod]
        public void Return_AnyMatchingElement_When_SearchByTwoMatchingAutoExcludedConditions()
        {
            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>();
            var result = sut.Append(x => x.Property2 == "Class")
                            .Append(x => x.Property1 == 1)
                            .ApplyTo(collection);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Return_AnyMatchingElement_When_SearchByTwoMatchingAutoExcludedFilterValues()
        {
            var filterValueCollection = this.GetAutoExcludedFilterValueCollection();

            var collection = GetStubCollection();

            var sut = CreateFilter<Stub>(filterValueCollection);
            var result = sut.Append("FilterProperty2", (entity, value) => entity.Property2.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                            .Append("FilterProperty1", (entity, value) => entity.Property1 == int.Parse(value))
                            .ApplyTo(collection);
            Assert.AreEqual(2, result.Count());
        }
               
        protected override Filter<TEntity> CreateFilter<TEntity>()
            => new AnyFilter<TEntity>();

        protected override Filter<TEntity> CreateFilter<TEntity>(FilterValue filterValue)
            => new AnyFilter<TEntity>(filterValue);

        protected override Filter<TEntity> CreateFilter<TEntity>(IEnumerable<FilterValue> filterValueCollection)
            => new AnyFilter<TEntity>(filterValueCollection);
    }
}