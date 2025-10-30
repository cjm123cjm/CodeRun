using CodeRun.Services.AppApi.Filters;
using CodeRun.Services.IService;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 应用更新
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IAppUpdateService _appUpdateService;
        private readonly FolderPath _folderPath;

        public UpdateController(IAppUpdateService appUpdateService, IOptions<FolderPath> folderPath)
        {
            _appUpdateService = appUpdateService;
            _folderPath = folderPath.Value;
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="updateVersionInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> CheckVerion(UpdateVersionInput updateVersionInput)
        {
            if (string.IsNullOrWhiteSpace(updateVersionInput.AppVersion))
            {
                return new ResponseDto();
            }
            var app = await _appUpdateService.SelectLastAppVersionAsync(updateVersionInput);
            if (app == null)
            {
                return new ResponseDto();
            }

            //查询更新文件大小
            var path = Path.Combine(_folderPath.PhysicalPath, Constants.APP_UPLOAD_FOLDER, app.Id + "_" + app.Status);
            if (!System.IO.File.Exists(path))
            {
                return new ResponseDto();
            }

            app.Size = path.Length;

            return new ResponseDto(app);
        }

        [HttpPost]
        [RateLimit(limit: 5, seconds: 86400)]
        public async Task<ResponseDto> DownLoad(long appId)
        {
            var app = await _appUpdateService.GetOneByIdAsync(appId);
            if (app == null)
            {
                return new ResponseDto("应用不存在");
            }

            string fileName = "CodeRun." + app.Version + app.Status;
            var path = Path.Combine(_folderPath.PhysicalPath, Constants.APP_UPLOAD_FOLDER, app.Id + "_" + app.Status);
            if (!System.IO.File.Exists(path))
            {
                return new ResponseDto("文件不存在");
            }

            // 获取文件信息
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return new ResponseDto("文件不存在");
            }

            Response.ContentType = "application/x-msdownload;charset=UTF-8";
            Response.ContentLength = fileInfo.Length;

            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + Uri.EscapeDataString(fileName) + "\"");
            Response.Headers.Add("Content-Type", "application/octet-stream");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

            // 使用文件流输出
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            await fileStream.CopyToAsync(Response.Body);
            await fileStream.DisposeAsync();

            return new ResponseDto();
        }
    }
}
