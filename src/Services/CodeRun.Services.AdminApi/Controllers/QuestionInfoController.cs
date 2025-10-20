using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Options;
using Microsoft.Extensions.Options;
using Rong.EasyExcel;
using CodeRun.Services.Domain.CustomerException;
using Rong.EasyExcel.Models;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionInfoController : BaseController
    {
        private readonly IQuestionInfoService _questionInfoService;
        private readonly IOptions<FolderPath> _folderPath;
        private readonly IExcelImportManager _excelImportManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<QuestionInfoController> _logger;

        public QuestionInfoController(
            IQuestionInfoService questionInfoService,
            IOptions<FolderPath> folderPath,
            IExcelImportManager excelImportManager,
            IWebHostEnvironment webHostEnvironment,
            ILogger<QuestionInfoController> logger)
        {
            _questionInfoService = questionInfoService;
            _folderPath = folderPath;
            _excelImportManager = excelImportManager;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// 加载八股文列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.question_list)]
        public async Task<ResponseDto> LoadQuestionInfoList(QuestionInfoQueryInput queryInput)
        {
            var data = await _questionInfoService.LoadQuestionInfoListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加或修改八股文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_edit)]
        public async Task<ResponseDto> QuestionInfoAddOrUpdate(QuestionInfoAddOrUpdateInput input)
        {
            await _questionInfoService.QuestionInfoAddOrUpdateAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [PermissionAuthorize(PermissionCodeEnum.question_list)]
        public async Task<ResponseDto> QuestionInfoById(long id)
        {
            var data = await _questionInfoService.QuestionInfoByIdAsync(id);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_del)]
        public async Task<ResponseDto> DeleteQuestionInfo(long questionInfoId)
        {
            await _questionInfoService.DeleteQuestionInfoAsync(questionInfoId.ToString());

            return new ResponseDto();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_del_batch)]
        public async Task<ResponseDto> DeleteBatchQuestionInfo(string questionInfoIds)
        {
            await _questionInfoService.DeleteQuestionInfoAsync(questionInfoIds);

            return new ResponseDto();
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_post)]
        public async Task<ResponseDto> PostQuestionInfo(string questionInfoIds)
        {
            await _questionInfoService.UpdateStatusQuestionInfoAsync(questionInfoIds, 1);

            return new ResponseDto();
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_post)]
        public async Task<ResponseDto> CancelQuestionInfo(string questionInfoIds)
        {
            await _questionInfoService.UpdateStatusQuestionInfoAsync(questionInfoIds, 0);

            return new ResponseDto();
        }

        /// <summary>
        /// 下载模板(文件地址/template/问题模板.xlsx)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.No_Permission)]
        public async Task DownloadTemplate()
        {
            string filePath = Path.Combine(
                         _folderPath.Value.PhysicalPath,
                         "/template/问题模板.xslx".TrimStart('/').Replace('/', '\\')
                     );

            string suffix = ".xslx";

            await ReadFile(filePath, suffix);
        }


        /// <summary>
        /// 导入问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.question_import)]
        public async Task<ResponseDto> ImportQuestionInfo(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentException("文件不能为空");
            }

            // 创建临时文件路径
            var tempFileName = $"{Guid.NewGuid()}_{formFile.FileName}";
            var tempFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "temp", tempFileName);

            try
            {
                // 确保目录存在
                var directory = Path.GetDirectoryName(tempFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory!);
                }

                // 保存上传的文件到临时路径
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                // 读取并处理Excel文件
                using (var stream = System.IO.File.OpenRead(tempFilePath))
                {
                    var importList = await _excelImportManager.ImportAsync<QuestionInfoImportDto>(stream, opt =>
                    {
                        opt.SheetIndex = 0;
                        opt.CheckError();
                    });

                    // 获取excel里的数据
                    var list = importList.GetAllData().ToList();

                    // 插入数据到数据库
                    await _questionInfoService.BatchImportQuestionInfoAsync(list);
                }

                return new ResponseDto();
            }
            catch (Exception ex)
            {
                // 记录日志而不是仅输出到控制台
                _logger.LogError(ex, "导入问题信息失败");
                throw new BusinessException("导入失败: " + ex.Message);
            }
            finally
            {
                // 清理临时文件
                if (System.IO.File.Exists(tempFilePath))
                {
                    try
                    {
                        System.IO.File.Delete(tempFilePath);
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogWarning(deleteEx, "删除临时文件失败: {FilePath}", tempFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// 上一页/下一页查看
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.question_list)]
        public async Task<ResponseDto> ShowQuestionInfoDetailNext(QuestionInfoQueryInput queryInput)
        {
            var data = await _questionInfoService.ShowQuestionInfoDetailNextAsync(queryInput);

            return new ResponseDto(data);
        }
    }
}
