using CodeRun.Services.Domain.Entities;

namespace CodeRun.Services.Domain.IRepository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task UpdateCategoryAsync(Category category);
    }
}
