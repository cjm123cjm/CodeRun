using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IReportService _reportService;

        public IndexController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// 获取数据概括
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetAllData()
        {
            var data = await _reportService.GetAllDataAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 获取app注册统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetAppWeekDataAsync()
        {
            var data = await _reportService.GetAppWeekDataAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 获取内容管理统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetContentWeekDataAsync()
        {
            var data = await _reportService.GetContentWeekDataAsync();

            return new ResponseDto(data);
        }
    }
}
