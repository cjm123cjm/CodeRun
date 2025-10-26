using CodeRun.Services.IService.Dtos;

namespace CodeRun.Services.IService.Interfaces
{
    /// <summary>
    /// 报表/数据统计
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// 获取数据概括
        /// </summary>
        /// <returns></returns>
        Task<List<StatisticsDataDto>> GetAllDataAsync();

        /// <summary>
        /// 获取app注册统计数据
        /// </summary>
        /// <returns></returns>
        Task<StatisticsDataWeekDto> GetAppWeekDataAsync();

        /// <summary>
        /// 获取内容管理统计数据
        /// </summary>
        /// <returns></returns>
        Task<StatisticsDataWeekDto> GetContentWeekDataAsync();
    }
}
