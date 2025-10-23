using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements.App
{
    public class FeedbackService : ServiceBase, IFeedbackService
    {
        private readonly IAppFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(
            IAppFeedbackRepository feedbackRepository, 
            IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载反馈信息
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<PageDto<FeedbackDto>> LoadFeedbackListAsync(FeedbackQueryInput queryInput)
        {
            var query = _feedbackRepository.Query().AsNoTracking().Where(t => t.FeedbackParentId == 0);

            if (queryInput.FeedbackStartTime != null)
            {
                query = query.Where(t => t.CreatedTime >= queryInput.FeedbackStartTime.Value);
            }
            if (queryInput.FeedbackEndTime != null)
            {
                query = query.Where(t => t.CreatedTime <= queryInput.FeedbackEndTime.Value);
            }
            if (queryInput.Status.HasValue)
            {
                query = query.Where(t => t.Status == queryInput.Status.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.UserName))
            {
                query = query.Where(t => t.NickName.Contains(queryInput.UserName));
            }

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.FeedbackParentId).Skip((queryInput.PageIndex - 1) * queryInput.PageSize).Take(queryInput.PageSize).ToListAsync();

            var dtos = ObjectMapper.Map<List<FeedbackDto>>(data);

            return new PageDto<FeedbackDto>
            {
                TotalCount = totalCount,
                Data = dtos,
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize,
            };
        }

        /// <summary>
        /// 反馈详情
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        public async Task<List<FeedbackDto>> FeedbackDetailAsync(long feedbackId)
        {
            return await _feedbackRepository.QueryWhere(t => t.FeedbackId == feedbackId || t.FeedbackParentId == feedbackId)
                  .ProjectTo<FeedbackDto>(ObjectMapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="replayFeedbackInput"></param>
        /// <returns></returns>
        public async Task ReplayFeedbackAsync(ReplayFeedbackInput replayFeedbackInput)
        {
            //修改父级状态
            var parentFeedback = await _feedbackRepository.GetByIdAsync(replayFeedbackInput.FeedbackId);
            if(parentFeedback == null)
            {
                throw new BusinessException("参数错误");
            }
            AppFeedback appFeedback = new AppFeedback
            {
                FeedbackId = SnowIdWorker.NextId(),
                UserId = LoginUserId,
                NickName = LoginUserName,
                Content = replayFeedbackInput.Content,
                FeedbackParentId = replayFeedbackInput.FeedbackId,
                SendType = 1
            };

            await _feedbackRepository.AddAsync(appFeedback);

            parentFeedback.Status = 1;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
