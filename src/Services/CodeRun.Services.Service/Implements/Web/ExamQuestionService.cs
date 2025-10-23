using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.Domain.Repository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace CodeRun.Services.Service.Implements.Web
{
    /// <summary>
    /// 题库接口实现
    /// </summary>
    public class ExamQuestionService : ServiceBase, IExamQuestionService
    {
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IExamQuestionItemRepository _examQuestionItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public ExamQuestionService(
            IExamQuestionRepository examQuestionRepository,
            IUnitOfWork unitOfWork,
            IExamQuestionItemRepository examQuestionItemRepository,
            ICategoryRepository categoryRepository)
        {
            _examQuestionRepository = examQuestionRepository;
            _unitOfWork = unitOfWork;
            _examQuestionItemRepository = examQuestionItemRepository;
            _categoryRepository = categoryRepository;
        }

        public IQueryable<ExamQuestion> SearchQuery(ExamQuestionQueryInput queryInput)
        {
            var query = _examQuestionRepository.Query().AsNoTracking();
            if (!string.IsNullOrEmpty(queryInput.Title))
            {
                query = query.Where(t => t.Title.Contains(queryInput.Title));
            }
            if (queryInput.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == queryInput.CategoryId.Value);
            }
            if (queryInput.DifficultyLevel.HasValue)
            {
                query = query.Where(t => t.DifficultyLevel == queryInput.DifficultyLevel.Value);
            }
            if (queryInput.QuestionType.HasValue)
            {
                query = query.Where(t => t.QuestionType == queryInput.QuestionType.Value);
            }
            if (queryInput.Status.HasValue)
            {
                query = query.Where(t => t.Status == queryInput.Status.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.CreatedUserName))
            {
                query = query.Where(t => t.CreatedUserName.Contains(queryInput.CreatedUserName));
            }

            return query;
        }

        /// <summary>
        /// 加载题库数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<PageDto<ExamQuestionDto>> LoadExamQuestionListAsync(ExamQuestionQueryInput queryInput)
        {
            var query = SearchQuery(queryInput);

            var total = await query.CountAsync();

            var examQuestion = await query.OrderByDescending(t => t.QuestionId)
                                          .Skip((queryInput.PageIndex - 1) * queryInput.PageSize)
                                          .Take(queryInput.PageSize)
                                          .ProjectTo<ExamQuestionDto>(ObjectMapper.ConfigurationProvider).ToListAsync();


            return new PageDto<ExamQuestionDto>
            {
                TotalCount = total,
                Data = examQuestion,
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize
            };
        }

        /// <summary>
        /// 添加/修改 考题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveExamQuestionAsync(ExamQuestionAddOrUpdateInput input)
        {
            if (input.QuestionId == 0)
            {
                var examQuestion = ObjectMapper.Map<ExamQuestion>(input);
                examQuestion.QuestionId = SnowIdWorker.NextId();
                examQuestion.CreatedTime = DateTime.Now;
                examQuestion.CreatedUserId = LoginUserId;
                examQuestion.CreatedUserName = LoginUserName;

                await _examQuestionRepository.AddAsync(examQuestion);

                var items = ObjectMapper.Map<List<ExamQuestionItem>>(input.Items);
                items.ForEach(t =>
                {
                    t.QuestionId = examQuestion.QuestionId;
                    t.ItemId = SnowIdWorker.NextId();
                });

                await _examQuestionItemRepository.AddAsync(items.ToArray());
            }
            else
            {
                var examQuestion = await _examQuestionRepository.GetByIdAsync(input.QuestionId);
                if (examQuestion == null)
                {
                    throw new BusinessException("数据不存在");
                }
                if (!IsAdmin && examQuestion.CreatedUserId != LoginUserId)
                {
                    throw new BusinessException("无权限修改");
                }
                ObjectMapper.Map(input, examQuestion);

                var items = await _examQuestionItemRepository.QueryWhere(t => t.QuestionId == examQuestion.QuestionId, true).ToListAsync();

                //添加
                var addItems = input.Items.Where(t => t.ItemId == 0).ToList();
                var addItemMaps = ObjectMapper.Map<List<ExamQuestionItem>>(addItems);
                addItemMaps.ForEach(t =>
                {
                    t.QuestionId = examQuestion.QuestionId;
                    t.ItemId = SnowIdWorker.NextId();
                });
                await _examQuestionItemRepository.AddAsync(addItemMaps.ToArray());

                //修改
                var editItems = input.Items.Where(t => t.ItemId != 0).ToList();
                var editItemIds = editItems.Select(t => t.ItemId).ToList();
                var editItemModels = items.Where(t => editItemIds.Contains(t.ItemId)).ToList();
                foreach (var item in editItemModels)
                {
                    var first = editItems.First(t => t.ItemId == item.ItemId);
                    item.Sort = first.Sort;
                    item.Title = first.Title;
                }

                //删除
                var deleteItemModels = items.Where(t => !editItemIds.Contains(t.ItemId)).ToList();
                if (deleteItemModels.Any())
                {
                    _examQuestionItemRepository.Delete(deleteItemModels.ToArray());
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExamQuestionAddOrUpdateInput> ExamQuestionByIdAsync(long id)
        {
            var examQuestion = await _examQuestionRepository.GetByIdAsync(id);
            if (examQuestion == null)
            {
                throw new BusinessException("数据不存在");
            }
            var detail = ObjectMapper.Map<ExamQuestionAddOrUpdateInput>(examQuestion);

            //查询选项
            var items = await _examQuestionItemRepository.QueryWhere(t => t.QuestionId == id).ToListAsync();

            detail.Items = ObjectMapper.Map<List<ExamQuestionItemDto>>(items);

            return detail;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">,号切割</param>
        /// <returns></returns>
        public async Task DeletedExamQuestionAsync(string ids)
        {
            var examIds = ids.Split(',').Select(t => Convert.ToInt64(t)).ToList();

            var query = _examQuestionRepository.QueryWhere(t => examIds.Contains(t.QuestionId), true);
            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }
            var examQuestion = await query.ToListAsync();

            _examQuestionRepository.Delete(examQuestion.ToArray());

            var itemQueryIds = examQuestion.Select(t => t.QuestionId).ToList();

            var items = await _examQuestionItemRepository.QueryWhere(t => itemQueryIds.Contains(t.QuestionId), true).ToListAsync();

            if (items.Any())
            {
                _examQuestionItemRepository.Delete(items.ToArray());
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 发布/取消
        /// </summary>
        /// <param name="questionIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateStatusExamQuestionAsync(string questionIds, int status)
        {
            if (status != 0 && status != 1)
            {
                throw new BusinessException("参数错误");
            }
            var examIds = questionIds.Split(',').Select(t => Convert.ToInt64(t)).ToList();

            var query = _examQuestionRepository.QueryWhere(t => examIds.Contains(t.QuestionId), true);
            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }

            var examQuestion = await query.ToListAsync();

            foreach (var item in examQuestion)
            {
                item.Status = status;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 上一页/下一页
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<ExamQuestionAddOrUpdateInput> ShowExamQuestionDetailNextAsync(ExamQuestionQueryInput queryInput)
        {
            if (queryInput.CurrentQuestionId == null)
            {
                throw new BusinessException("参数错误");
            }
            var query = SearchQuery(queryInput);

            //上一页
            if (queryInput.Type == 0)
            {
                query = query.Where(t => t.QuestionId < queryInput.CurrentQuestionId.Value).OrderByDescending(t => t.QuestionId).Take(1);
            }
            //下一页
            else
            {
                query = query.Where(t => t.QuestionId > queryInput.CurrentQuestionId.Value).OrderByDescending(t => t.QuestionId).Take(1);
            }

            var examQuestion = await query.FirstOrDefaultAsync();
            if (examQuestion != null)
            {
                var dto = ObjectMapper.Map<ExamQuestionAddOrUpdateInput>(examQuestion);

                var items = _examQuestionItemRepository.QueryWhere(t => t.QuestionId == dto.QuestionId).ToListAsync();

                dto.Items = ObjectMapper.Map<List<ExamQuestionItemDto>>(items);

                return dto;
            }
            else
            {
                if (queryInput.Type == 0)
                    throw new BusinessException("已经是第一页了");
                else
                    throw new BusinessException("已经是最后一页了");
            }
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="importDtos"></param>
        /// <returns></returns>
        public async Task BatchImportExamQuestionAsync(List<ExamQuestionImportDto> importDtos)
        {
            var categoryNames = importDtos.Select(t => t.CategoryName).Distinct().ToList();

            var categories = await _categoryRepository.QueryWhere(t => categoryNames.Contains(t.CategoryName), false).ToListAsync();

            Dictionary<int, List<string>> errorStr = new Dictionary<int, List<string>>();

            List<ExamQuestion> examQuestions = new List<ExamQuestion>();
            List<ExamQuestionItem> examQuestionItems = new List<ExamQuestionItem>();

            int index = 0;
            foreach (var item in importDtos)
            {
                index++;
                List<string> error = new List<string>();
                //判断分类是否存在
                var category = categories.FirstOrDefault(t => t.CategoryName == item.CategoryName);
                if (category == null)
                {
                    error.Add("分类不存在");
                }
                //判断难度等级
                if (item.DifficultyLevel <= 0 && item.DifficultyLevel > 5)
                {
                    error.Add("难度等级请填写1-5");
                }
                //判断问题类型
                if (item.QuestionType != "判断题" && item.QuestionType != "单选题" && item.QuestionType != "多选题")
                {
                    error.Add("问题类型只能是：判断题、单选题、多选题");
                }
                //判断问题选项
                if (string.IsNullOrEmpty(item.QuestionItems) && item.QuestionType != "判断题")
                {
                    error.Add("问题选项不能为空");
                }
                if (string.IsNullOrEmpty(item.QuestionAnswer))
                {
                    error.Add("答案不能为空");
                }
                if (error.Count > 0)
                {
                    errorStr.Add(index, error);
                    continue;
                }
                //添加数据
                var question = new ExamQuestion
                {
                    QuestionId = SnowIdWorker.NextId(),
                    Title = item.Title,
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    DifficultyLevel = item.DifficultyLevel,
                    QuestionType = item.QuestionType == "判断题" ? 0 : item.QuestionType == "单选题" ? 1 : 2,
                    Question = item.Question,
                    QuestionAnswer = item.QuestionAnswer,
                    AnswerAnalysis = item.AnswerAnalysis,
                    CreatedTime = DateTime.Now,
                    CreatedUserId = LoginUserId,
                    CreatedUserName = LoginUserName,
                };
                examQuestions.Add(question);
                var questionItems = item.QuestionItems.Split('\r').Where(t => !string.IsNullOrEmpty(t)).ToList();
                for (int i = 0; i < questionItems.Count; i++)
                {
                    examQuestionItems.Add(new ExamQuestionItem
                    {
                        QuestionId = question.QuestionId,
                        ItemId = SnowIdWorker.NextId(),
                        Title = questionItems[i].Substring(1),
                        Sort = i + 1
                    });
                }
            }

            if (errorStr.Count > 0)
            {
                throw new BusinessException(errorStr!.ToString()!);
            }

            await _examQuestionRepository.AddAsync(examQuestions.ToArray());

            await _examQuestionItemRepository.AddAsync(examQuestionItems.ToArray());

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
