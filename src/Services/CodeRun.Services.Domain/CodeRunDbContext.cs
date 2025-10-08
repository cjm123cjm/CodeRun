using CodeRun.Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Domain
{
    public class CodeRunDbContext : DbContext
    {
        public CodeRunDbContext(DbContextOptions<CodeRunDbContext> dbContext) : base(dbContext)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(options =>
            {
                options.HasKey(t => t.UserId);
                options.HasIndex(t => t.Phone).IsUnique();
            });
        }
    }
}
