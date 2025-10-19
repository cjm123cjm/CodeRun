namespace CodeRun.Services.IService.Options
{
    public class FolderPath
    {
        /// <summary>
        /// 物理路径
        /// </summary>
        public string PhysicalPath { get; set; } = null!;
        /// <summary>
        /// 虚拟目录
        /// </summary>
        public string virtualPath { get; set; } = null!;
    }
}
