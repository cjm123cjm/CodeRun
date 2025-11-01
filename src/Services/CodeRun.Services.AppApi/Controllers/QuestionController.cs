using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 八股文
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionInfoService _questionInfoService;
        private readonly IAppUserCollectService _userCollectService;

        public QuestionController(
            IQuestionInfoService questionInfoService,
            IAppUserCollectService userCollectService)
        {
            _questionInfoService = questionInfoService;
            _userCollectService = userCollectService;
        }

        /// <summary>
        /// 八股文列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadQuestion(PageInput pageInput)
        {
            var data = await _questionInfoService.LoadQuestionInfoListAsync(new QuestionInfoQueryInput
            {
                PageIndex = pageInput.PageIndex,
                PageSize = pageInput.PageSize,
                Status = 1
            });

            return new ResponseDto(data);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> QuestionDetailNext(ShowNextDetailInput showNextDetail)
        {
            var data = await _questionInfoService.ShowQuestionInfoDetailNextAsync(new QuestionInfoQueryInput
            {
                PageIndex = showNextDetail.PageIndex,
                PageSize = showNextDetail.PageSize,
                CurrentQuestionInfoId = showNextDetail.CurrentId,
                NextType = showNextDetail.Type,
                ReadCountAdd = true
            });

            //判断用户是否登录
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                var userId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(p => p.Type.Equals("UserId"))!.Value);

                var collect = await _userCollectService.GetUserAppCollectByObjectIdAsync(userId, data.QuestionId, 1);

                if (collect != null)
                {
                    data.IsCollect = true;
                }
            }

            return new ResponseDto(data);
        }
    }
}
