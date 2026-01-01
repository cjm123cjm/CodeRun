using Microsoft.AspNetCore.Http;

namespace CodeRun.Services.IService.Dtos
{
    /// <summary>
    /// 上传文件参数
    /// </summary>
    public class UploadFileInput
    {
        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile File { get; set; } = null!;
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
    }
}
