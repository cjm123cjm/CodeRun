using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class ExamQuestionItemRepository : BaseRepository<ExamQuestionItem>, IExamQuestionItemRepository
    {
        protected ExamQuestionItemRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
