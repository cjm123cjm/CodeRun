using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Options;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionInfoController : BaseController
    {
        private readonly IQuestionInfoService _questionInfoService;
        private readonly IOptions<FolderPath> _folderPath;

        public QuestionInfoController(
            IQuestionInfoService questionInfoService, 
            IOptions<FolderPath> folderPath)
        {
            _questionInfoService = questionInfoService;
            _folderPath = folderPath;
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
        public async Task ImportQuestionInfo(IFormFile formFile)
        {

        }
    }
}
