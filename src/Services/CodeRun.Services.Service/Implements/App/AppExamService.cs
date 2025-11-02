using CodeRun.Services.Domain;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Linq.Expressions;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.UnitOfWork;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppExamService : ServiceBase, IAppExamService
    {
        private readonly IAppExamRepository _examRepository;
        private readonly IAppExamQuestionRepository _questionRepository;
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppExamService(
            IAppExamRepository examRepository,
            IAppExamQuestionRepository questionRepository,
            IExamQuestionRepository examQuestionRepository,
            IUnitOfWork unitOfWork)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examQuestionRepository = examQuestionRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 查询用户是否有未完成的考试
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppExamDto>> CheckUserNoFinishedExamAsync()
        {
            var exam = await _examRepository.QueryWhere(t => t.UserId == LoginUserId && t.Status == 0)
                                            .OrderByDescending(t => t.ExamId)
                                            .ToListAsync();

            var examDto = ObjectMapper.Map<List<AppExamDto>>(exam);

            return examDto;
        }

        /// <summary>
        /// 创建考试
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task<AppExamDto> CreateExamAsync(string categoryIds)
        {
            //获取用户已经做过并且正确的题目
            var rightQuestionId = await _questionRepository.QueryWhere(t => t.UserId == LoginUserId && t.AnswerStatus == 1).Select(t => t.QuestionId).Distinct().ToListAsync();

            var categorySplitIds = categoryIds.Split(',').Select(t => Convert.ToInt64(t));
            Expression<Func<ExamQuestion, bool>> expression = t => categorySplitIds.Contains(t.CategoryId) &&
                                                                   !rightQuestionId.Contains(t.QuestionId) &&
                                                                   t.Status == 1;

            var questions = await _examQuestionRepository.QueryWhere(expression)
                                                         .OrderBy(t => EF.Functions.Random())
                                                         .Take(50)
                                                         .ToListAsync();
            if (!questions.Any())
            {
                throw new BusinessException("该分类下没有题目");
            }

            //记录考试
            AppExam appExam = new AppExam
            {
                ExamId = SnowIdWorker.NextId(),
                UserId = LoginUserId,
                NickName = LoginUserName,
                CreatedTime = DateTime.Now,
                Status = 0
            };

            await _examRepository.AddAsync(appExam);

            //记录考试题目
            List<AppExamQuestion> appExamQuestions = new List<AppExamQuestion>();
            foreach (var item in questions)
            {
                AppExamQuestion appExamQuestion = new AppExamQuestion
                {
                    ExamId = appExam.ExamId,
                    UserId = LoginUserId,
                    QuestionId = item.QuestionId,
                    AnswerStatus = 0
                };
                appExamQuestions.Add(appExamQuestion);
            }

            await _questionRepository.AddAsync(appExamQuestions.ToArray());

            await _unitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<AppExamDto>(appExam);
        }
    }
}
