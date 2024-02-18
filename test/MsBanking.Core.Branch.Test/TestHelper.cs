using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsBanking.Core.Branch.Test
{
    public static class TestHelper
    {
        public static BranchDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BranchDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var mockDb = new BranchDbContext(options);
            return mockDb;
        }
    }
}
