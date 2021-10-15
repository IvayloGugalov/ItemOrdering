using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ItemOrdering.Infrastructure.Data;

namespace ItemOrdering.Infrastructure.Test
{
    public static class SeedDatabaseExtension
    {

        public static async Task SeedDataBaseWith(this ItemOrderingDbContext dbContext, int range)
        {

            //await dbContext.Orders.AddRangeAsync(orders);
            //await dbContext.SaveChangesAsync();
        }
    }
}
