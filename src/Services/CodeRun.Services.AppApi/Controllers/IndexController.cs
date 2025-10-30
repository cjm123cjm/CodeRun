using CodeRun.Services.AppApi.Filters;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IAppCarouselService _carouselService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly IAppDeviceService _deviceService;

        public IndexController(
            ICategoryService categoryService,
            IAppCarouselService carouselService,
            IExamQuestionService examQuestionService,
            IAppDeviceService deviceService)
        {
            _categoryService = categoryService;
            _carouselService = carouselService;
            _examQuestionService = examQuestionService;
            _deviceService = deviceService;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadAllCategory(int type)
        {
            var data = await _categoryService.LoadCategoryListByTypeAsync(0);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadCarousel()
        {
            var data = await _categoryService.LoadCategoryListByTypeAsync(0);

            return new ResponseDto(data);
        }

        [HttpGet("{id}")]
        public async Task<ResponseDto> GetExamQuestionById(long id)
        {
            var data = await _examQuestionService.ExamQuestionByIdAsync(id);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 登录上报
        /// </summary>
        /// <param name="reportInput"></param>
        /// <returns></returns>
        [HttpPost]
        [RateLimit(limit: 5, seconds: 86400)]
        public async Task<ResponseDto> Report(ReportInput reportInput)
        {
            await _deviceService.ReportAsync(reportInput);

            return new ResponseDto();
        }
    }
}
