namespace Swords.LinqFilter
{
    using System;
    using System.Collections.Generic;

    public class ExactFilter<TEntity> : Filter<TEntity>
        where TEntity : class
    {
        public ExactFilter()
            : base()
        {
        }

        public ExactFilter(FilterValue filterValue)
            : base(filterValue)
        {
        }

        public ExactFilter(IEnumerable<FilterValue> filterValueCollection)
            : base(filterValueCollection)
        {
        }

        protected override bool CreateMergeCriteria(TEntity entity, Func<TEntity, bool> partialCriteria, Func<TEntity, bool> currentCriteria)
            => currentCriteria(entity) && partialCriteria(entity);
    }
}