using CodeRun.Services.Domain.Entities.Web;

namespace CodeRun.Services.Domain.IRepository.Web
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task UpdateCategoryAsync(Category category);
    }
}
