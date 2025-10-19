using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeRun.Services.Service.Implements
{
    public class QuestionInfoService : ServiceBase, IQuestionInfoService
    {
        private readonly IQuestionInfoRepository _questionInfoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionInfoService(
            IQuestionInfoRepository questionInfoRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _questionInfoRepository = questionInfoRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载八股文列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<PageDto<QuestionInfoDto>> LoadQuestionInfoListAsync(QuestionInfoQueryInput queryInput)
        {
            var query = _questionInfoRepository.Query();
            if (!string.IsNullOrWhiteSpace(queryInput.Title))
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
            if (queryInput.Status.HasValue)
            {
                query = query.Where(t => t.Status == queryInput.Status.Value);
            }
            if (!string.IsNullOrEmpty(queryInput.CreatedUserName))
            {
                query = query.Where(t => t.CreatedUserName.Contains(queryInput.CreatedUserName));
            }

            var totalCount = await query.CountAsync();

            var questionDtos = await query
                                    .OrderByDescending(t => t.CreatedTime)
                                    .Skip((queryInput.PageIndex - 1) * queryInput.PageSize)
                                    .Take(queryInput.PageSize)
                                    .ProjectTo<QuestionInfoDto>(ObjectMapper.ConfigurationProvider).ToListAsync();

            return new PageDto<QuestionInfoDto>
            {
                TotalCount = totalCount,
                Data = questionDtos,
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize,
            };
        }

        /// <summary>
        /// 添加或修改八股文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task QuestionInfoAddOrUpdateAsync(QuestionInfoAddOrUpdateInput input)
        {
            var category = await _categoryRepository.GetByIdAsync(input.CategoryId);
            if (category == null)
            {
                throw new BusinessException("分类不存在");
            }
            input.CategoryName = category.CategoryName;

            if (input.QuestionId == 0)
            {
                var questionInfo = ObjectMapper.Map<QuestionInfo>(input);
                questionInfo.QuestionId = SnowIdWorker.NextId();
                questionInfo.CreatedTime = DateTime.Now;
                questionInfo.CreatedUserId = LoginUserId;
                questionInfo.CreatedUserName = LoginUserName;

                await _questionInfoRepository.AddAsync(questionInfo);
            }
            else
            {
                var questionInfo = await _questionInfoRepository.GetByIdAsync(input.QuestionId);
                if (questionInfo == null)
                {
                    throw new BusinessException("数据不存在");
                }
                if (questionInfo.CreatedUserId != LoginUserId && !IsAdmin)
                {
                    throw new BusinessException(message: "无权限");
                }
                ObjectMapper.Map(input, questionInfo);

                _questionInfoRepository.Update(questionInfo);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QuestionInfoAddOrUpdateInput> QuestionInfoByIdAsync(long id)
        {
            var questionInfo = await _questionInfoRepository.GetByIdAsync(id);
            if (questionInfo == null)
            {
                throw new BusinessException("数据不存在");
            }
            var questionInput = ObjectMapper.Map<QuestionInfoAddOrUpdateInput>(questionInfo);
            return questionInput;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        public async Task DeleteQuestionInfoAsync(string questionInfoIds)
        {
            var ids = questionInfoIds.Split(',').Select(t => Convert.ToInt64(t)).Distinct().ToList();

            var query = _questionInfoRepository.QueryWhere(t => ids.Contains(t.QuestionId), true);

            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }

            var question = await query.ToListAsync();

            _questionInfoRepository.Delete(question.ToArray());

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 发布/取消发布
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <param name="status">0-未发布,1-已发布</param>
        /// <returns></returns>
        public async Task UpdateStatusQuestionInfoAsync(string questionInfoIds, int status)
        {
            var ids = questionInfoIds.Split(',').Select(t => Convert.ToInt64(t)).Distinct().ToList();

            var query = _questionInfoRepository.QueryWhere(t => ids.Contains(t.QuestionId), true);

            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }

            var question = await query.ToListAsync();

            foreach (var item in question)
            {
                item.Status = status;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
