using CodeRun.Services.AppApi.Filters;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 搜索
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IQuestionInfoService _questionInfoService;
        private readonly IShareInfoService _shareInfoService;
        private readonly IExamQuestionService _examQuestionService;

        public SearchController(
            IQuestionInfoService questionInfoService,
            IShareInfoService shareInfoService,
            IExamQuestionService examQuestionService)
        {
            _questionInfoService = questionInfoService;
            _shareInfoService = shareInfoService;
            _examQuestionService = examQuestionService;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RateLimit(limit: 20, seconds: 60)]
        public async Task<ResponseDto> Search(SearchInput searchInput)
        {
            switch (searchInput.Type)
            {
                case 0:
                    var data = await _questionInfoService.LoadQuestionInfoListAsync(new IService.Dtos.Inputs.Web.QuestionInfoQueryInput
                    {
                        Title = searchInput.KeyWord,
                        ReadCountAdd = true,
                        PageIndex = searchInput.PageIndex,
                        PageSize = searchInput.PageSize,
                    });
                    return new ResponseDto(data);
                case 1:
                    var share = await _shareInfoService.LoadShareInfoListAsync(new IService.Dtos.Inputs.Web.ShareInfoQueryInput
                    {
                        Title = searchInput.KeyWord,
                        Status = 1,
                        PageIndex = searchInput.PageIndex,
                        PageSize = searchInput.PageSize,
                    });
                    return new ResponseDto(share);
                case 2:
                    var exam = await _examQuestionService.LoadExamQuestionListAsync(new IService.Dtos.Inputs.Web.ExamQuestionQueryInput
                    {
                        Title = searchInput.KeyWord,
                        Status = 1,
                        PageIndex = searchInput.PageIndex,
                        PageSize = searchInput.PageSize,
                    });
                    return new ResponseDto(exam);
            }
            return new ResponseDto(false, "参数错误");
        }
    }
}
