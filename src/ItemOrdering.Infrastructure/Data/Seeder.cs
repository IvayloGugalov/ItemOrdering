using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemOrdering.Domain.CustomerAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public static class Seeder
    {
        public static void Initialize(ItemOrderingDbContext context)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
