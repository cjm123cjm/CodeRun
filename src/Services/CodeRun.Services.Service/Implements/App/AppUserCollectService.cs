using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppUserCollectService : ServiceBase, IAppUserCollectService
    {
        private readonly IAppUserCollectRepository _userCollectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppUserCollectService(
            IAppUserCollectRepository userCollectRepository,
            IUnitOfWork unitOfWork)
        {
            _userCollectRepository = userCollectRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取用户是否收藏文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="objectId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<AppUserCollect?> GetUserAppCollectByObjectIdAsync(long userId, long objectId, int type)
        {
            return await _userCollectRepository.QueryWhere(t => t.UserId == userId && t.ObjectId == objectId && t.CollectType == type).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 添加/取消收藏
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public async Task AddOrCancelCollect(AppUserCollectAddOrUpdate appUser)
        {
            var collect = await _userCollectRepository.QueryWhere(t => t.UserId == LoginUserId &&
                                                                        t.ObjectId == appUser.ObjectId &&
                                                                        t.CollectType == appUser.CollectType)
                                                      .FirstOrDefaultAsync();

            if (appUser.AddOrCancel == 0)
            {
                if (collect != null)
                {
                    throw new BusinessException("参数错误");
                }
                AppUserCollect userCollect = new AppUserCollect
                {
                    CollectId = SnowIdWorker.NextId(),
                    UserId = LoginUserId,
                    ObjectId = appUser.ObjectId,
                    CollectType = appUser.CollectType,
                    CollectTime = DateTime.Now
                };

                await _userCollectRepository.AddAsync(userCollect);
            }
            else
            {
                if (collect == null)
                {
                    throw new BusinessException("参数错误");
                }
                _userCollectRepository.Delete(collect);
            }

            await _unitOfWork.SaveChangesAsync();
        }

    }
}
