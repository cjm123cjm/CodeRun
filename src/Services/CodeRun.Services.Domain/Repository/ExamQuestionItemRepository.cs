using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class ExamQuestionItemRepository : BaseRepository<ExamQuestionItem>, IExamQuestionItemRepository
    {
        protected ExamQuestionItemRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
