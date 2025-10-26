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
    public class AppUpdateController : ControllerBase
    {
        private readonly IAppUpdateService _updateService;

        public AppUpdateController(IAppUpdateService updateService)
        {
            _updateService = updateService;
        }

        /// <summary>
        /// 加载发布列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_update_list)]
        public async Task<ResponseDto> LoadAppUpdateList(AppUpdateQueryInput queryInput)
        {
            var data = await _updateService.LoadAppUpdateListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 发布/修改 版本
        /// </summary>
        /// <param name="addOrUpdateInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_update_edit)]
        public async Task<ResponseDto> SaveAppUpdate(AppUpdateAddOrUpdateInput addOrUpdateInput)
        {
            await _updateService.SaveAppUpdateAsync(addOrUpdateInput);

            return new ResponseDto();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="addUpdateId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_update_edit)]
        public async Task<ResponseDto> DeletedAppUpdate(long addUpdateId)
        {
            await _updateService.DeletedAppUpdateAsync(addUpdateId);

            return new ResponseDto();
        }

        /// <summary>
        /// 发布app
        /// </summary>
        /// <param name="updateInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_update_post)]
        public async Task<ResponseDto> PostUpdate(PostAppUpdateInput updateInput)
        {
            await _updateService.PostUpdateAsync(updateInput);

            return new ResponseDto();
        }
    }
}
