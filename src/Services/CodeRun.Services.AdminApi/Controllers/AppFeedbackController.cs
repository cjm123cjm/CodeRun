using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppFeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public AppFeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// 加载反馈信息
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_carousel_list)]
        public async Task<ResponseDto> LoadFeedbackList(FeedbackQueryInput queryInput)
        {
            var data = await _feedbackService.LoadFeedbackListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 加载详情反馈信息
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_feedback_list)]
        public async Task<ResponseDto> FeedbackDetailAsync(long feedback)
        {
            var data = await _feedbackService.FeedbackDetailAsync(feedback);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="replayFeedbackInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_feedback_replay)]
        public async Task<ResponseDto> ReplayFeedback(ReplayFeedbackInput replayFeedbackInput)
        {
            await _feedbackService.ReplayFeedbackAsync(replayFeedbackInput);

            return new ResponseDto();
        }
    }
}
