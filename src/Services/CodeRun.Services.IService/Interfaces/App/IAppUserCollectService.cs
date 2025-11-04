using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppUserCollectService
    {
        /// <summary>
        /// 获取用户是否收藏文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="objectId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<AppUserCollect?> GetUserAppCollectByObjectIdAsync(long userId, long objectId, int type);

        /// <summary>
        /// 添加/取消收藏
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task AddOrCancelCollect(AppUserCollectAddOrUpdate appUser);

        /// <summary>
        /// 获取用户收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PageDto<AppUserCollect>> GetUserAppCollectByUserIdIdAsync(AppUserCollectQueryInput queryInput);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="showNextDetailInput"></param>
        /// <returns></returns>
        Task<AppUserCollect> ShowDetailNextAsync(ShowNextDetailInput showNextDetailInput);
    }
}
