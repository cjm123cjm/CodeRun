using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppDeviceService : ServiceBase, IAppDeviceService
    {
        private readonly IAppDeviceRepository _appDeviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppDeviceService(IAppDeviceRepository appDeviceRepository, IUnitOfWork unitOfWork)
        {
            _appDeviceRepository = appDeviceRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载设备列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<List<AppDeviceDto>> LoadAppDeviceListAsync(AppDeviceQueryInput queryInput)
        {
            var query = _appDeviceRepository.Query().AsNoTracking();
            if (queryInput.JoinTimeStart != null)
            {
                query = query.Where(t => t.CreatedTime >= queryInput.JoinTimeStart.Value);
            }
            if (queryInput.JoinTimeEnd != null)
            {
                query = query.Where(t => t.CreatedTime <= queryInput.JoinTimeEnd.Value);
            }
            if (queryInput.LastUseTimeStart != null)
            {
                query = query.Where(t => t.LastUseTime >= queryInput.LastUseTimeStart.Value);
            }
            if (queryInput.LastUseTimeEnd != null)
            {
                query = query.Where(t => t.LastUseTime >= queryInput.LastUseTimeEnd.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.DeviceBrand))
            {
                query = query.Where(t => t.DeviceBrand.Contains(queryInput.DeviceBrand));
            }
            if (!string.IsNullOrWhiteSpace(queryInput.DeviceId))
            {
                query = query.Where(t => t.DeviceId.Contains(queryInput.DeviceId));
            }

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.CreatedTime).ToListAsync();

            return ObjectMapper.Map<List<AppDeviceDto>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ReportAsync(ReportInput input)
        {
            AppDevice appDevice = new AppDevice
            {
                DeviceId = input.DeviceId,
                DeviceBrand = input.DeviceBrand,
                CreatedTime = DateTime.Now,
                LastUseTime = DateTime.Now,
                Ip = UserIp
            };

            await _appDeviceRepository.AddAsync(appDevice);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
