namespace CodeRun.Services.IService.Dtos
{
    /// <summary>
    /// 数据概括
    /// </summary>
    public class StatisticsDataDto
    {
        /// <summary>
        /// app下载...
        /// </summary>
        public string StatisticsName { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 昨日新增
        /// </summary>
        public int PerCount { get; set; }
        public List<int> ListData { get; set; }
    }
}
