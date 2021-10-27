using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ordering.Domain.Interfaces
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; }
        public List<string> IncludeStrings { get; }
    }
}
