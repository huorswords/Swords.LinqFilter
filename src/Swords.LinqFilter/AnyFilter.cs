namespace Swords.LinqFilter
{
    using System;
    using System.Collections.Generic;

    public class AnyFilter<TEntity> : Filter<TEntity>
        where TEntity : class
    {
        public AnyFilter()
            : base()
        {
        }

        public AnyFilter(FilterValue filterValue)
            : base(filterValue)
        {
        }

        public AnyFilter(IEnumerable<FilterValue> filterValueCollection)
            : base(filterValueCollection)
        {
        }

        protected override bool CreateMergeCriteria(TEntity entity, Func<TEntity, bool> partialCriteria, Func<TEntity, bool> currentCriteria)
            => currentCriteria(entity) || partialCriteria(entity);
    }
}