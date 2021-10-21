using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ItemOrdering.Domain.Shared
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; }
        public List<string> IncludeStrings { get; }
    }
}
