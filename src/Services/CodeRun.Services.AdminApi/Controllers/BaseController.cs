using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        protected async Task ReadFile(string filePath, string downloadFileName = null)
        {
            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                await Response.WriteAsync("Image not found");
                return;
            }

            // 获取文件信息
            var fileInfo = new FileInfo(filePath);
            string fileName = downloadFileName ?? fileInfo.Name;
            string fileExtension = fileInfo.Extension.ToLower();

            // 获取 Content-Type
            string contentType = GetContentType(fileExtension.TrimStart('.'));

            // 设置响应头
            Response.ContentType = contentType;

            // ⭐ 关键：正确设置 Content-Disposition
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = fileName, // 支持中文文件名
                FileName = Uri.EscapeDataString(fileName) // 兼容所有浏览器
            };

            Response.Headers.ContentDisposition = contentDisposition.ToString();

            // 添加其他有用的头信息
            Response.Headers.ContentLength = fileInfo.Length;
            Response.Headers.AcceptRanges = "bytes";
            Response.Headers.CacheControl = "no-cache";

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

                // Excel 文件类型
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "xls" => "application/vnd.ms-excel",
                "xlsm" => "application/vnd.ms-excel.sheet.macroEnabled.12",
                "xlsb" => "application/vnd.ms-excel.sheet.binary.macroEnabled.12",

                // Word 文件类型
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "doc" => "application/msword",

                // PowerPoint 文件类型
                "pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "ppt" => "application/vnd.ms-powerpoint",

                // PDF
                "pdf" => "application/pdf",

                // 压缩文件
                "zip" => "application/zip",
                "rar" => "application/x-rar-compressed",
                "7z" => "application/x-7z-compressed",

                // 文本文件
                "txt" => "text/plain",
                "csv" => "text/csv",
                "html" => "text/html",
                "htm" => "text/html",
                "xml" => "application/xml",
                "json" => "application/json",

                // 其他常见文件
                "mp4" => "video/mp4",
                "mp3" => "audio/mpeg",
                "wav" => "audio/wav",

                _ => "application/octet-stream"
            };
        }
    }
}
