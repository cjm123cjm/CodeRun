using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ResponseDto> UploadFile(IFormFile formFile)
        {
            // 获取文件后缀名
            var extension = Path.GetExtension(formFile.FileName);
            string month = DateTime.Now.ToString("yyyyMM");
            var uploadFolder = Path.Combine(_folderPath.Value.PhysicalPath, month);
            //202510/
            if (Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // 为文件重命名，防止文件重名
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

            // 文件保存的文件夹路径
            var fileFullPath = Path.Combine(uploadFolder, fileName);

            using var targetStream = System.IO.File.Create(fileFullPath);

            await formFile.CopyToAsync(targetStream);

            targetStream.Dispose();

            return new ResponseDto
            {
                IsSuccess = true,
                Result = month + "/" + fileName
            };
        }
    }
}
