namespace Swords.LinqFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Filter<TEntity>
        where TEntity : class
    {
        private Func<TEntity, bool> criteria;

        private readonly List<FilterValue> filterValueCollection;

        protected Filter()
            : this(new List<FilterValue>())
        {
        }

        protected Filter(FilterValue filterValue)
            : this(new List<FilterValue> { filterValue })
        {
        }

        protected Filter(IEnumerable<FilterValue> filterValueCollection)
        {
            this.filterValueCollection = (List<FilterValue>)filterValueCollection ?? throw new ArgumentNullException(nameof(filterValueCollection));
        }

        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> collection)
        {
            if (criteria != null)
            {
                return collection.Where(criteria).AsQueryable();
            }

            return collection;
        }

        public Filter<TEntity> Append(Func<TEntity, bool> partialCriteria)
        {
            if (partialCriteria != null)
            {
                if (this.criteria != null)
                {
                    var currentCriteria = this.criteria;
                    this.criteria = entity => this.CreateMergeCriteria(entity, partialCriteria, currentCriteria);
                }
                else
                {
                    this.criteria = entity => partialCriteria(entity);
                }
            }

            return this;
        }

        public Filter<TEntity> Append(string filterName, Func<TEntity, string, bool> partialCriteria)
        {
            if (!string.IsNullOrEmpty(filterName))
            {
                bool byEqualsName(FilterValue filter) => filter.Name.Equals(filterName, StringComparison.InvariantCultureIgnoreCase);
                if (this.filterValueCollection.Any(byEqualsName))
                {
                    var filterValue = filterValueCollection.First(byEqualsName);
                    string value = filterValue.Value;
                    return this.Append(entity => partialCriteria(entity, value));
                }
            }

            return this;
        }

        public Filter<TEntity> Append(string filterName, Func<TEntity, string[], bool> partialCriteria)
        {
            if (!string.IsNullOrEmpty(filterName))
            {
                bool byEqualsName(FilterValue filter) => filter.Name.Equals(filterName, StringComparison.InvariantCultureIgnoreCase);
                if (this.filterValueCollection.Any(byEqualsName))
                {
                    var filterValue = filterValueCollection.First(byEqualsName);
                    string[] valueCollection = filterValue.Value.Split(filterValue.Separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    return this.Append(entity =>
                    {
                        return partialCriteria(entity, valueCollection);
                    });
                }
            }

            return this;
        }

        protected abstract bool CreateMergeCriteria(TEntity entity, Func<TEntity, bool> partialCriteria, Func<TEntity, bool> currentCriteria);
    }
}