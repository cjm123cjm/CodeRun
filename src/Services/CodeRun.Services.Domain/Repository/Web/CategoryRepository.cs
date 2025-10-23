using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        protected CategoryRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            Update(category);

            //修改ExamQuestion、QuestionInfo
            var examQuestion = await _context.ExamQuestions.Where(t => t.CategoryId == category.CategoryId).ToListAsync();
            examQuestion.ForEach(question =>
            {
                question.CategoryName = category.CategoryName;
            });

            var questions = await _context.QuestionInfos.Where(t => t.CategoryId == category.CategoryId).ToListAsync();
            questions.ForEach(question =>
            {
                question.CategoryName = category.CategoryName;
            });
        }
    }
}
