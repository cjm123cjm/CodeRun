using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppUpdateService : ServiceBase, IAppUpdateService
    {
        private readonly IAppUpdateRepository _appUpdateRepository;

        public AppUpdateService(IAppUpdateRepository appUpdateRepository)
        {
            _appUpdateRepository = appUpdateRepository;
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
        public Task SaveAppUpdateAsync(AppUpdateAddOrUpdateInput addOrUpdateInput)
        {
            throw new NotImplementedException();
        }
    }
}
