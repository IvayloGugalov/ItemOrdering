using System.Collections.Generic;

using Ordering.Domain.Shared;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Test
{
    public static class SeedDatabaseExtension
    {
        /// <summary>
        /// Adds an entity to the In-Memory Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        public static void SeedDataBaseWith<T>(this ItemOrderingDbContext dbContext, T entity)
            where T : Entity
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Adds a range of entities to the In-Memory Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entities"></param>
        public static void SeedDataBaseWith<T>(this ItemOrderingDbContext dbContext, IEnumerable<T> entities)
            where T : Entity
        {
            dbContext.Set<T>().AddRange(entities);
            dbContext.SaveChanges();
        }
    }
}
