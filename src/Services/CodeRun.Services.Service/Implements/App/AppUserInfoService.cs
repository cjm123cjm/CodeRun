using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
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
    public class AppUserInfoService : ServiceBase, IAppUserInfoService
    {
        private readonly IAppUserInfoRepository _userInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppUserInfoService(
            IAppUserInfoRepository userInfoRepository,
            IUnitOfWork unitOfWork)
        {
            _userInfoRepository = userInfoRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageDto<AppUserInfoDto>> LoadAppUserInfoListAsync(AppUserInfoQueryInput queryInput)
        {
            var query = _userInfoRepository.Query().AsNoTracking();
            if (queryInput.JoinTimeStart != null)
            {
                query = query.Where(t => t.JoinTime >= queryInput.JoinTimeStart.Value);
            }
            if (queryInput.JoinTimeEnd != null)
            {
                query = query.Where(t => t.JoinTime <= queryInput.JoinTimeEnd.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Email))
            {
                query = query.Where(t => t.Email.Contains(queryInput.Email));
            }
            if (!string.IsNullOrWhiteSpace(queryInput.DeviceId))
            {
                query = query.Where(t => t.LastUseDeviceId.Contains(queryInput.DeviceId));
            }

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.JoinTime).Skip((queryInput.PageIndex - 1) * queryInput.PageSize).Take(queryInput.PageSize)
                .ToListAsync();

            return new PageDto<AppUserInfoDto>
            {
                TotalCount = totalCount,
                Data = ObjectMapper.Map<List<AppUserInfoDto>>(data),
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize
            };
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task UpdateStatusAppUserInfoAsync(UpdateStatusAppUserInput update)
        {
            var appUser = await _userInfoRepository.GetByIdAsync(update.AppUserId);
            if (appUser == null)
            {
                throw new BusinessException("数据不存在");
            }

            appUser.Status = update.Status;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
