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
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppExamService : ServiceBase, IAppExamService
    {
        private readonly IAppExamRepository _examRepository;
        private readonly IAppExamQuestionRepository _questionRepository;
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IExamQuestionItemRepository _questionItemRepository;
        private readonly IAppUserCollectRepository _userCollectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppExamService(
            IAppExamRepository examRepository,
            IAppExamQuestionRepository questionRepository,
            IExamQuestionRepository examQuestionRepository,
            IUnitOfWork unitOfWork,
            IExamQuestionItemRepository questionItemRepository,
            IAppUserCollectRepository userCollectRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examQuestionRepository = examQuestionRepository;
            _unitOfWork = unitOfWork;
            _questionItemRepository = questionItemRepository;
            _userCollectRepository = userCollectRepository;
        }

        /// <summary>
        /// 获取用户已考试列表
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public async Task<PageDto<AppExamDto>> LoadUserExamAsync(PageInput pageInput)
        {
            var query = _examRepository.QueryWhere(t => t.UserId == LoginUserId);

            var totalCount = await query.CountAsync();

            var exams = await query.OrderByDescending(t => t.CreatedTime)
                                   .Skip((pageInput.PageIndex - 1) * pageInput.PageSize)
                                   .Take(pageInput.PageSize)
                                   .ToListAsync();

            var examDtos = ObjectMapper.Map<List<AppExamDto>>(exams);

            return new PageDto<AppExamDto>
            {
                Data = examDtos,
                PageIndex = pageInput.PageIndex,
                PageSize = pageInput.PageSize,
                TotalCount = totalCount
            };
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

        /// <summary>
        /// 获取考试和考题信息
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<AppExamDto> GetExamQuestionAsync(long examId)
        {
            //判断这个考试Id是不是登录人创建的,不是就抛异常
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null || exam.UserId != LoginUserId)
            {
                throw new BusinessException("参数错误");
            }

            var examDto = ObjectMapper.Map<AppExamDto>(exam);

            var showAnswer = exam.Status == 1 ? true : false;

            //查询考题
            var questions = await _questionRepository.QueryWhere(t => t.ExamId == examId).ToListAsync();

            var questionIds = questions.Select(t => t.QuestionId).ToList();

            var userExamQuestions = await _examQuestionRepository.QueryWhere(t => questionIds.Contains(t.QuestionId))
                                                                 .Select(t => new UserExamQuestionListDto
                                                                 {
                                                                     QuestionId = t.QuestionId,
                                                                     Title = t.Title,
                                                                     DifficultyLevel = t.DifficultyLevel,
                                                                     Question = t.Question,
                                                                     QuestionAnswer = showAnswer ? t.QuestionAnswer : "",
                                                                     AnswerAnalysis = showAnswer ? t.AnswerAnalysis : "",
                                                                     QuestionType = t.QuestionType,
                                                                     ExamId = examId,
                                                                 }).ToListAsync();
            //查询选项
            var itemList = await _questionItemRepository.QueryWhere(t => questionIds.Contains(t.QuestionId)).ToListAsync();
            var itemListDto = ObjectMapper.Map<List<ExamQuestionItemDto>>(itemList);

            //查询收藏
            var collects = await _userCollectRepository.QueryWhere(t => questionIds.Contains(t.ObjectId) && t.CollectType == 2 && t.UserId == LoginUserId).ToListAsync();

            foreach (var item in userExamQuestions)
            {
                //选项
                item.QuestionItemList = itemListDto.Where(t => t.QuestionId == item.QuestionId).ToList();
                //收藏
                item.HaveCollect = collects.Any(t => t.ObjectId == item.QuestionId);

                //查询用户答案和是否正确
                var question = questions.FirstOrDefault(t => t.QuestionId == item.QuestionId);
                if (question != null)
                {
                    item.UserAnswer = question?.UserAnswer;
                    item.AnswerStatus = question == null ? 0 : question.AnswerStatus;
                    item.AppExamQuestionId = question.Id;
                }

            }

            examDto.ExamQuestionLists = userExamQuestions;

            return examDto;
        }

        /// <summary>
        /// 开始考试
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task StartExamAsync(long examId)
        {
            //判断这个考试Id是不是登录人创建的,不是就抛异常
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null || exam.UserId != LoginUserId)
            {
                throw new BusinessException("参数错误");
            }

            exam.StartTime = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 提交考试
        /// </summary>
        /// <param name="postExamInput"></param>
        /// <returns></returns>
        public async Task<AppExamDto> PostExamAsync(PostExamInput postExamInput)
        {
            //判断这个考试Id是不是登录人创建的,不是就抛异常
            var exam = await _examRepository.GetByIdAsync(postExamInput.ExamId);
            if (exam == null || exam.UserId != LoginUserId)
            {
                throw new BusinessException("参数错误");
            }
            if (exam.Status == 1)
            {
                throw new BusinessException(message: "考试已提交");
            }
            if (!postExamInput.appExamQuestionDtos.Any())
            {
                throw new BusinessException("参数错误");
            }

            var questions = await _questionRepository.QueryWhere(t => t.ExamId == postExamInput.ExamId, true).ToListAsync();

            var questionIds = questions.Select(t => t.QuestionId);

            //查询问题
            var examQuestions = await _examQuestionRepository.QueryWhere(t => questionIds.Contains(t.QuestionId)).ToListAsync();

            foreach (var item in postExamInput.appExamQuestionDtos)
            {
                var questionFirst = questions.FirstOrDefault(t => t.QuestionId == item.QuestionId);
                if (questionFirst == null)
                {
                    throw new BusinessException("参数错误");
                }
                //问题
                var examAnswer = examQuestions.FirstOrDefault(t => t.QuestionId == item.QuestionId);
                if (examAnswer != null)
                {
                    questionFirst.UserAnswer = item.UserAnswer;
                    questionFirst.AnswerStatus = examAnswer.QuestionAnswer == item.UserAnswer ? 1 : 2;
                }
            }

            exam.EndTime = DateTime.Now;
            exam.Status = 1;
            exam.Remark = postExamInput.Remark;

            int count = await _unitOfWork.SaveChangesAsync();
            if (count == 0)
            {
                throw new BusinessException("考试提交失败");
            }

            return ObjectMapper.Map<AppExamDto>(exam);
        }

        /// <summary>
        /// 删除考试
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task DeleteExamAsync(long examId)
        {
            //判断这个考试Id是不是登录人创建的,不是就抛异常
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null || exam.UserId != LoginUserId)
            {
                throw new BusinessException("参数错误");
            }

            var examQuestions = await _questionRepository.QueryWhere(t => t.ExamId == examId && t.UserId == LoginUserId, true).ToListAsync();

            _questionRepository.Delete(examQuestions.ToArray());

            _examRepository.Delete(exam);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 加载用户错题集133333
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public async Task<PageDto<UserExamQuestionListDto>> LoadUserWroingExamAsync(PageInput pageInput)
        {
            var query = _questionRepository.QueryWhere(t => t.UserId == LoginUserId && t.AnswerStatus == 2).OrderByDescending(t => t.ExamId);

            var total = await query.CountAsync();

            //查询考题
            var questions = await query.OrderByDescending(t => t.ExamId)
                                       .Skip((pageInput.PageIndex - 1) * pageInput.PageSize)
                                       .ToListAsync();

            var questionIds = questions.Select(t => t.QuestionId).ToList();

            var userExamQuestions = await _examQuestionRepository.QueryWhere(t => questionIds.Contains(t.QuestionId))
                                                                 .Select(t => new UserExamQuestionListDto
                                                                 {
                                                                     QuestionId = t.QuestionId,
                                                                     Title = t.Title,
                                                                     DifficultyLevel = t.DifficultyLevel,
                                                                     Question = t.Question,
                                                                     QuestionAnswer = t.QuestionAnswer,
                                                                     AnswerAnalysis = t.AnswerAnalysis,
                                                                     QuestionType = t.QuestionType
                                                                 }).ToListAsync();
            foreach (var item in userExamQuestions)
            {
                var first = questions.FirstOrDefault(t => t.QuestionId == item.QuestionId);
                if (first != null)
                {
                    item.UserAnswer = first.UserAnswer;
                    item.AnswerStatus = first.AnswerStatus;
                    item.ExamId = first.ExamId;
                    item.AppExamQuestionId = first.Id;
                }
            }


            return new PageDto<UserExamQuestionListDto>
            {
                Data = userExamQuestions,
                TotalCount = total,
                PageIndex = pageInput.PageIndex,
                PageSize = pageInput.PageSize
            };
        }
    }
}
