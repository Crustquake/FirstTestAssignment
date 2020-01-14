using System;

using Microsoft.EntityFrameworkCore;

using Xpandion.Data.DatabaseEntities;

namespace Xpandion.Data
{
    public class XpandionContext : DbContext
    {
        public DbSet<CsvFileEntity> Files { get; set; }
        public DbSet<CsvFileColumnEntity> Columns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DataStructure> DataStructures { get; set; }

        public XpandionContext(DbContextOptions<XpandionContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
