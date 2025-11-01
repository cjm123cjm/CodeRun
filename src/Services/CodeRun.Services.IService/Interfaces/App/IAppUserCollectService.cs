using CodeRun.Services.Domain.Entities.App;
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
    }
}
