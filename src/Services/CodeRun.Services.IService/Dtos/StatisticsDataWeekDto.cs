namespace CodeRun.Services.IService.Dtos
{
    public class StatisticsDataWeekDto
    {
        /// <summary>
        /// 月份
        /// </summary>
        public List<string> DataList { get; set; } = new();
        /// <summary>
        /// 详情数据
        /// </summary>
        public List<StatisticsDataDto> ItemDataList { get; set; } = new();
    }
}
