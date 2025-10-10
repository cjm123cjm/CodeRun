using CodeRun.Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Domain
{
    public class CodeRunDbContext : DbContext
    {
        public CodeRunDbContext(DbContextOptions<CodeRunDbContext> dbContext) : base(dbContext)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleForMenu> RoleForMenus { get; set; }
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
