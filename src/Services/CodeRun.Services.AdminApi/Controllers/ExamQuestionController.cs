using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces;
using CodeRun.Services.IService.Options;
using CodeRun.Services.Service.Implements;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rong.EasyExcel;
using Rong.EasyExcel.Models;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamQuestionController : BaseController
    {
        private readonly IExamQuestionService _questionService;
        private readonly IOptions<FolderPath> _folderPath;
        private readonly IExcelImportManager _excelImportManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ExamQuestionController> _logger;

        public ExamQuestionController(
            IExamQuestionService questionService,
            IOptions<FolderPath> folderPath,
            IExcelImportManager excelImportManager,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ExamQuestionController> logger)
        {
            _questionService = questionService;
            _folderPath = folderPath;
            _excelImportManager = excelImportManager;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// 加载题库数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_list)]
        public async Task<ResponseDto> LoadExamQuestionList(ExamQuestionQueryInput queryInput)
        {
            var data = await _questionService.LoadExamQuestionListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加/修改 考题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_edit)]
        public async Task<ResponseDto> SaveExamQuestion(ExamQuestionAddOrUpdateInput input)
        {
            await _questionService.SaveExamQuestionAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_list)]
        public async Task<ResponseDto> ExamQuestionByIdAsync(long id)
        {
            var data = await _questionService.ExamQuestionByIdAsync(id);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="ids">,号切割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_del)]
        public async Task<ResponseDto> DeletedExamQuestion(long id)
        {
            await _questionService.DeletedExamQuestionAsync(id.ToString());

            return new ResponseDto();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">,号切割</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_del_batch)]
        public async Task<ResponseDto> DeletedBatchExamQuestion(string ids)
        {
            await _questionService.DeletedExamQuestionAsync(ids);

            return new ResponseDto();
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_post)]
        public async Task<ResponseDto> PostExamQuestion(string ids)
        {
            await _questionService.UpdateStatusExamQuestionAsync(ids,1);

            return new ResponseDto();
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_post)]
        public async Task<ResponseDto> CancelExamQuestion(string ids)
        {
            await _questionService.UpdateStatusExamQuestionAsync(ids, 0);

            return new ResponseDto();
        }

        /// <summary>
        /// 下载模板(文件地址/template/考题模板.xlsx)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.No_Permission)]
        public async Task DownloadTemplate()
        {
            string filePath = Path.Combine(
                         _folderPath.Value.PhysicalPath,
                         "/template/考题模板.xslx".TrimStart('/').Replace('/', '\\')
                     );

            string suffix = ".xslx";

            await ReadFile(filePath, suffix);
        }

        /// <summary>
        /// 导入问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.exam_question_import)]
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
                    var importList = await _excelImportManager.ImportAsync<ExamQuestionImportDto>(stream, opt =>
                    {
                        opt.SheetIndex = 0;
                        opt.CheckError();
                    });

                    // 获取excel里的数据
                    var list = importList.GetAllData().ToList();

                    // 插入数据到数据库
                    await _questionService.BatchImportExamQuestionAsync(list);
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
        [PermissionAuthorize(PermissionCodeEnum.exam_question_list)]
        public async Task<ResponseDto> ShowExamQuestionDetailNext(ExamQuestionQueryInput queryInput)
        {
            var data = await _questionService.ShowExamQuestionDetailNextAsync(queryInput);

            return new ResponseDto(data);
        }
    }
}
