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
        public DbSet<AppExam> AppExams { get; set; }
        public DbSet<AppExamQuestion> AppExamQuestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(options =>
            {
                options.HasKey(t => t.UserId);
                options.HasIndex(t => t.Phone).IsUnique();
            });

            modelBuilder.Entity<Menu>(options =>
            {
                options.HasKey(t => t.MenuId);
            });
            modelBuilder.Entity<Role>(options =>
            {
                options.HasKey(t => t.RoleId);
            });
            modelBuilder.Entity<RoleForMenu>(options =>
            {
                options.HasKey(t => new { t.RoleId, t.MenuId });
            });
            modelBuilder.Entity<Category>(options =>
            {
                options.HasKey(t => t.CategoryId);
            });
            modelBuilder.Entity<QuestionInfo>(options =>
            {
                options.HasKey(t => t.QuestionId);
            });
            modelBuilder.Entity<ExamQuestion>(options =>
            {
                options.HasKey(t => t.QuestionId);
            });
            modelBuilder.Entity<ExamQuestionItem>(options =>
            {
                options.HasKey(t => t.ItemId);
            });
            modelBuilder.Entity<ShareInfo>(options =>
            {
                options.HasKey(t => t.ShareId);
            });

            modelBuilder.Entity<AppCarousel>(options =>
            {
                options.HasKey(t => t.CarouselId);
            });
            modelBuilder.Entity<AppDevice>(options =>
            {
                options.HasKey(t => t.DeviceId);
            });
            modelBuilder.Entity<AppFeedback>(options =>
            {
                options.HasKey(t => t.FeedbackId);
            });
            modelBuilder.Entity<AppUpdate>(options =>
            {
                options.HasKey(t => t.Id);
            });
            modelBuilder.Entity<AppUserInfo>(options =>
            {
                options.HasKey(t => t.UserId);
            });
            modelBuilder.Entity<AppUserCollect>(options =>
            {
                options.HasKey(t => t.CollectId);
            });
            modelBuilder.Entity<AppExam>(options =>
            {
                options.HasKey(t => t.ExamId);
            });
            modelBuilder.Entity<AppExamQuestion>(options =>
            {
                options.HasKey(t => t.Id);
            });
        }
    }
}
