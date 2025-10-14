using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class QuestionInfoRepository : BaseRepository<QuestionInfo>, IQuestionInfoRepository
    {
        protected QuestionInfoRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
