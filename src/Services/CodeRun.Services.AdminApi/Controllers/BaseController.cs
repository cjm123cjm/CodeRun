using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [NonAction]
        protected async Task ReadFile(string filePath, string suffix)
        {
            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                await Response.WriteAsync("Image not found");
                return;
            }

            string contentType = GetContentType(suffix);

            Response.ContentType = contentType;
            Response.Headers.CacheControl = "max-age=259200";

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    await fileStream.CopyToAsync(Response.Body);
                }
            }
            catch (Exception ex)
            {
                if (!Response.HasStarted)
                {
                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await Response.WriteAsync("文件下载失败");
                }
            }
        }

        /// <summary>
        /// 根据文件后缀获取正确的 Content-Type
        /// </summary>
        [NonAction]
        protected string GetContentType(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "bmp" => "image/bmp",
                "webp" => "image/webp",
                "svg" => "image/svg+xml",
                "ico" => "image/x-icon",
                "tiff" => "image/tiff",
                _ => "application/octet-stream"
            };
        }
    }
}
