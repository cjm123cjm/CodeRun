namespace CodeRun.Services.IService.Dtos
{
    /// <summary>
    /// 导入错误信息
    /// </summary>
    public class ImportDataErrorDto
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> ErrorItemList { get; set; } = new List<string>();
    }
}
