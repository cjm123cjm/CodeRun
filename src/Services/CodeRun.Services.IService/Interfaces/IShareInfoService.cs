using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    public interface IShareInfoService
    {
        /// <summary>
        /// 分享列表查询
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<ShareInfoDto>> LoadShareInfoListAsync(ShareInfoQueryInput queryInput);

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveShareInfoAsync(ShareInfoAddOrUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <returns></returns>
        Task DeletedShareInfoAsync(string shareIds);

        /// <summary>
        /// 发布/取消发布
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <param name="status">0-取消发布 1-发布</param>
        /// <returns></returns>
        Task UpdateStatusShareInfoAsync(string shareIds, int status);

        /// <summary>
        /// 上一页/下一页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ShareInfoAddOrUpdateInput> ShowShareInfoDetailNextAsync(ShareInfoQueryInput input);
    }
}
