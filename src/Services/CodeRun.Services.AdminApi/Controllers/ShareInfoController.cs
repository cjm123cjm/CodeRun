using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareInfoController : ControllerBase
    {
        private readonly IShareInfoService _shareInfoService;

        public ShareInfoController(IShareInfoService shareInfoService)
        {
            _shareInfoService = shareInfoService;
        }

        /// <summary>
        /// 分享列表查询
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.share_list)]
        public async Task<ResponseDto> LoadShareInfoList(ShareInfoQueryInput queryInput)
        {
            var data = await _shareInfoService.LoadShareInfoListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.share_edit)]
        public async Task<ResponseDto> SaveShareInfo(ShareInfoAddOrUpdateInput input)
        {
            await _shareInfoService.SaveShareInfoAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.share_del)]
        public async Task<ResponseDto> DeletedShareInfo(long shareIds)
        {
            await _shareInfoService.DeletedShareInfoAsync(shareIds.ToString());

            return new ResponseDto();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.share_del_batch)]
        public async Task<ResponseDto> BatchDeletedShareInfo(long shareIds)
        {
            await _shareInfoService.DeletedShareInfoAsync(shareIds.ToString());

            return new ResponseDto();
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <param name="status">0-取消发布 1-发布</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.share_post)]
        public async Task<ResponseDto> PostShareInfo(string shareIds)
        {
            await _shareInfoService.UpdateStatusShareInfoAsync(shareIds, 1);

            return new ResponseDto();
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <param name="status">0-取消发布 1-发布</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.share_post)]
        public async Task<ResponseDto> CancelShareInfo(string shareIds)
        {
            await _shareInfoService.UpdateStatusShareInfoAsync(shareIds, 0);

            return new ResponseDto();
        }

        /// <summary>
        /// 上一页/下一页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.share_list)]
        public async Task<ResponseDto> ShowShareInfoDetailNextAsync(ShareInfoQueryInput input)
        {
            var data = await _shareInfoService.ShowShareInfoDetailNextAsync(input);

            return new ResponseDto(data);
        }
    }
}
