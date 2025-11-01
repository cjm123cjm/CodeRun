using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.Entities.Web;
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
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuestionInfo> QuestionInfos { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamQuestionItem> ExamQuestionItems { get; set; }
        public DbSet<ShareInfo> ShareInfos { get; set; }

        public DbSet<AppCarousel> AppCarousels { get; set; }
        public DbSet<AppDevice> AppDevices { get; set; }
        public DbSet<AppFeedback> AppFeedbacks { get; set; }
        public DbSet<AppUpdate> AppUpdates { get; set; }
        public DbSet<AppUserInfo> AppUserInfos { get; set; }
        public DbSet<AppUserCollect> AppUserCollects { get; set; }
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
