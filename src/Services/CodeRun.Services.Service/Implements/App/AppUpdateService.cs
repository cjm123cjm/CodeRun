using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppUpdateService : ServiceBase, IAppUpdateService
    {
        private readonly IAppUpdateRepository _appUpdateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppUpdateService(IAppUpdateRepository appUpdateRepository, IUnitOfWork unitOfWork)
        {
            _appUpdateRepository = appUpdateRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载发布信息
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<PageDto<AppUpdateDto>> LoadAppUpdateListAsync(AppUpdateQueryInput queryInput)
        {
            var query = _appUpdateRepository.Query().AsNoTracking();
            if (queryInput.PulishStartTime != null)
            {
                query = query.Where(t => t.CreatedTime >= queryInput.PulishStartTime.Value);
            }
            if (queryInput.PulishEndTime != null)
            {
                query = query.Where(t => t.CreatedTime <= queryInput.PulishEndTime.Value);
            }

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.CreatedTime)
                                    .Skip((queryInput.PageIndex - 1) * queryInput.PageSize)
                                    .Take(queryInput.PageSize)
                                    .ProjectTo<AppUpdateDto>(ObjectMapper.ConfigurationProvider).ToListAsync();

            return new PageDto<AppUpdateDto>
            {
                TotalCount = totalCount,
                Data = data,
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize
            };
        }

        /// <summary>
        /// 发布/修改 版本
        /// </summary>
        /// <param name="addOrUpdateInput"></param>
        /// <returns></returns>
        public async Task SaveAppUpdateAsync(AppUpdateAddOrUpdateInput addOrUpdateInput)
        {
            var lastUpdate = await _appUpdateRepository.QueryWhere(t => t.Id != addOrUpdateInput.Id).AsNoTracking().OrderByDescending(t => t.CreatedTime).FirstOrDefaultAsync();
            //查询版本
            if (lastUpdate != null)
            {
                long dbVesrion = Convert.ToInt64(lastUpdate.Version.Replace(".", ""));
                long currentVesrion = Convert.ToInt64(lastUpdate.Version.Replace(".", ""));
                if (addOrUpdateInput.Id == 0 && currentVesrion <= dbVesrion)
                {
                    throw new BusinessException("当前版本必须大于历史版本");
                }
            }
            if (addOrUpdateInput.Id == 0)
            {
                var appUpdate = ObjectMapper.Map<AppUpdate>(addOrUpdateInput);
                appUpdate.Id = SnowIdWorker.NextId();
                appUpdate.CreatedTime = DateTime.Now;

                addOrUpdateInput.Id = appUpdate.Id;

                await _appUpdateRepository.AddAsync(appUpdate);
            }
            else
            {
                var appUpdate = await _appUpdateRepository.GetByIdAsync(addOrUpdateInput.Id);
                if (appUpdate == null)
                {
                    throw new BusinessException("数据不存在");
                }
                ObjectMapper.Map(addOrUpdateInput, appUpdate);

                _appUpdateRepository.Update(appUpdate);
            }
            if (addOrUpdateInput.File != null)
            {
                //E:\\coderun\\upload\\id+status
                var path = Path.Combine(FolderPath.PhysicalPath, Constants.APP_UPLOAD_FOLDER, addOrUpdateInput.Id + "_" + addOrUpdateInput.Status);
                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullPath = Path.Combine(path, addOrUpdateInput.File.FileName);

                using (var targetStream = System.IO.File.Create(fileFullPath))
                {
                    await addOrUpdateInput.File.CopyToAsync(targetStream);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="addUpdateId"></param>
        /// <returns></returns>
        public async Task DeletedAppUpdateAsync(long addUpdateId)
        {
            var appUpdate = await _appUpdateRepository.GetByIdAsync(addUpdateId);
            if (appUpdate == null)
            {
                throw new BusinessException("数据不存在");
            }

            _appUpdateRepository.Delete(appUpdate);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 发布app
        /// </summary>
        /// <param name="updateInput"></param>
        /// <returns></returns>
        public async Task PostUpdateAsync(PostAppUpdateInput updateInput)
        {
            var appUpdate = await _appUpdateRepository.GetByIdAsync(updateInput.AppUpdateId);
            if (appUpdate == null)
            {
                throw new BusinessException("数据不存在");
            }
            //0-未发布,1-灰度发布,2-全网发布
            if (updateInput.Status != 0 && updateInput.Status != 1 && updateInput.Status != 2)
            {
                throw new BusinessException("参数错误");
            }
            if (updateInput.Status == 1 && !string.IsNullOrWhiteSpace(updateInput.GrayscaleDevice))
            {
                throw new BusinessException("参数错误");
            }
            if (updateInput.Status != 1)
                updateInput.GrayscaleDevice = null;

            appUpdate.Status = updateInput.Status;
            appUpdate.GrayscaleDevice = updateInput.GrayscaleDevice;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
