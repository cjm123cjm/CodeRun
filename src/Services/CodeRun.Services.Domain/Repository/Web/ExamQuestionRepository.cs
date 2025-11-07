using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class ExamQuestionRepository : BaseRepository<ExamQuestion>, IExamQuestionRepository
    {
        public ExamQuestionRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
