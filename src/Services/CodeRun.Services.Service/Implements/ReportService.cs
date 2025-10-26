using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;

namespace CodeRun.Services.Service.Implements
{
    public class ReportService : ServiceBase, IReportService
    {
        private readonly IDapperRepository _dapperRepository;

        public ReportService(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        /// <summary>
        /// 获取数据概括
        /// </summary>
        /// <returns></returns>
        public Task<List<StatisticsDataDto>> GetAllDataAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取app注册统计数据
        /// </summary>
        /// <returns></returns>
        public Task<StatisticsDataWeekDto> GetAppWeekDataAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取内容管理统计数据
        /// </summary>
        /// <returns></returns>
        public Task<StatisticsDataWeekDto> GetContentWeekDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
