using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 分享
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly IShareInfoService _shareInfoService;
        private readonly IAppUserCollectService _userCollectService;

        public ShareController(
            IShareInfoService shareInfoService, 
            IAppUserCollectService userCollectService)
        {
            _shareInfoService = shareInfoService;
            _userCollectService = userCollectService;
        }

        /// <summary>
        /// 分享列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadShare(PageInput pageInput)
        {
            var data = await _shareInfoService.LoadShareInfoListAsync(new ShareInfoQueryInput
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
        public async Task<ResponseDto> ShareDetailNext(ShowNextDetailInput showNextDetail)
        {
            var data = await _shareInfoService.ShowShareInfoDetailNextAsync(new ShareInfoQueryInput
            {
                PageIndex = showNextDetail.PageIndex,
                PageSize = showNextDetail.PageSize,
                CurrentShareInfoId = showNextDetail.CurrentId,
                Type = showNextDetail.Type,
                ReadCount = true
            });

            //判断用户是否登录
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                var userId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(p => p.Type.Equals("UserId"))!.Value);

                var collect = await _userCollectService.GetUserAppCollectByObjectIdAsync(userId, data.ShareId, 0);

                if (collect != null)
                {
                    data.IsCollect = true;
                }
            }

            return new ResponseDto(data);
        }
    }
}
