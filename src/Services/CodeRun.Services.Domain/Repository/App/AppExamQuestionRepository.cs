using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppExamQuestionRepository : BaseRepository<AppExamQuestion>, IAppExamQuestionRepository
    {
        public AppExamQuestionRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
