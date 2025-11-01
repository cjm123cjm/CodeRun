using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserCollectController : ControllerBase
    {
        private readonly IAppUserCollectService _userCollectService;

        public UserCollectController(IAppUserCollectService userCollectService)
        {
            _userCollectService = userCollectService;
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> AddCollect(AppUserCollectAddOrUpdate appUser)
        {
            appUser.AddOrCancel = 0;
            await _userCollectService.AddOrCancelCollect(appUser);

            return new ResponseDto();
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> CancelCollect(AppUserCollectAddOrUpdate appUser)
        {
            appUser.AddOrCancel = 1;
            await _userCollectService.AddOrCancelCollect(appUser);

            return new ResponseDto();
        }
    }
}
