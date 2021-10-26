using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Ordering.Domain.Shared
{
    public static class SpecificationExtensions
    {
        public static IQueryable<TSource> Specify<TSource>(this IQueryable<TSource> query, ISpecification<TSource> spec)
            where TSource : class
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(query,
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult
                .Where(spec.Criteria);
        }
    }
}
