using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IFeedbackService
    {
        /// <summary>
        /// 加载反馈信息
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<FeedbackDto>> LoadFeedbackListAsync(FeedbackQueryInput queryInput);

        /// <summary>
        /// 反馈详情
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        Task<List<FeedbackDto>> FeedbackDetailAsync(long feedbackId);

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="replayFeedbackInput"></param>
        /// <returns></returns>
        Task ReplayFeedbackAsync(ReplayFeedbackInput replayFeedbackInput);
    }
}
