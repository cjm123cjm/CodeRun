using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppUpdateService
    {
        /// <summary>
        /// 加载发布列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<AppUpdateDto>> LoadAppUpdateListAsync(AppUpdateQueryInput queryInput);

        /// <summary>
        /// 发布/修改 版本
        /// </summary>
        /// <param name="addOrUpdateInput"></param>
        /// <returns></returns>
        Task SaveAppUpdateAsync(AppUpdateAddOrUpdateInput addOrUpdateInput);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="addUpdateId"></param>
        /// <returns></returns>
        Task DeletedAppUpdateAsync(long addUpdateId);

        /// <summary>
        /// 发布app
        /// </summary>
        /// <param name="updateInput"></param>
        /// <returns></returns>
        Task PostUpdateAsync(PostAppUpdateInput updateInput);
    }
}
