using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 在线考试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AppExamController : ControllerBase
    {
        private readonly IAppExamService _appExamService;

        public AppExamController(IAppExamService appExamService)
        {
            _appExamService = appExamService;
        }

        /// <summary>
        /// 用户是否有未完成的考试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadNoFinishedExam()
        {
            var data = await _appExamService.CheckUserNoFinishedExamAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 创建考试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> CreateExam(string categoryIds)
        {
            var data = await _appExamService.CreateExamAsync(categoryIds);

            return new ResponseDto(data);
        }
    }
}
