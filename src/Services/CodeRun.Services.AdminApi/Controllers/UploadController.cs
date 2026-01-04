using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IOptions<FolderPath> _folderPath;

        public UploadController(IOptions<FolderPath> folderPath)
        {
            _folderPath = folderPath;
        }

        /// <summary>
        /// 上传文件（支持多文件/大文件500M）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 609715200)]
        [RequestSizeLimit(609715200)]
        public async Task<ResponseDto> UploadFile([FromForm] IFormFile formFile)
        {
            // 获取文件后缀名
            var extension = Path.GetExtension(formFile.FileName);
            string month = DateTime.Now.ToString("yyyyMM");
            var uploadFolder = Path.Combine(_folderPath.Value.PhysicalPath, month);
            //202510/
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // 为文件重命名，防止文件重名
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

            // 文件保存的文件夹路径
            var fileFullPath = Path.Combine(uploadFolder, fileName);

            using var targetStream = System.IO.File.Create(fileFullPath);

            await formFile.CopyToAsync(targetStream);

            //todo:生成缩略图

            targetStream.Dispose();

            return new ResponseDto
            {
                IsSuccess = true,
                Result = month + "/" + fileName
            };
        }

        /// <summary>
        /// 上传文件（支持多文件/大文件500M）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 609715200)]
        [RequestSizeLimit(609715200)]
        public async Task<ResponseDto> UploadFileByFrom([FromForm] UploadFileInput formFile)
        {
            // 获取文件后缀名
            var extension = Path.GetExtension(formFile.File.FileName);
            string month = DateTime.Now.ToString("yyyyMM");

            var uploadFolder = "";

            switch (formFile.Type)
            {
                case 0:
                    uploadFolder = Path.Combine(_folderPath.Value.PhysicalPath, "分类管理");
                    break;
                case 1:
                    uploadFolder = Path.Combine(_folderPath.Value.PhysicalPath, "经验分享");
                    break;
                default:
                    uploadFolder = Path.Combine(_folderPath.Value.PhysicalPath, month);
                    break;
            }
            //202510/
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // 为文件重命名，防止文件重名
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

            // 文件保存的文件夹路径
            var fileFullPath = Path.Combine(uploadFolder, fileName);

            using var targetStream = System.IO.File.Create(fileFullPath);

            await formFile.File.CopyToAsync(targetStream);

            targetStream.Dispose();

            return new ResponseDto
            {
                IsSuccess = true,
                Result = formFile.Type == 0 ? "分类管理/" + fileName : formFile.Type == 1 ? "经验分享/" + fileName : month + "/" + fileName
            };
        }
    }
}
