using CodeRun.Services.AppApi.Filters;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using CodeRun.Services.IService.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 个人中心控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MyController : ControllerBase
    {
        private readonly IAppUserInfoService _userInfoService;
        private readonly IQuestionInfoService _questionInfoService;
        private readonly IAppUserCollectService _userCollectService;
        private readonly IShareInfoService _shareInfoService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly IAppExamService _examService;
        private readonly IOptions<FolderPath> _options;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFeedbackService _feedbackService;

        public MyController(
            IAppUserInfoService userInfoService,
            IQuestionInfoService questionInfoService,
            IAppUserCollectService userCollectService,
            IShareInfoService shareInfoService,
            IExamQuestionService examQuestionService,
            IAppExamService examService,
            IOptions<FolderPath> options,
            IHttpContextAccessor contextAccessor,
            IFeedbackService feedbackService)
        {
            _userInfoService = userInfoService;
            _questionInfoService = questionInfoService;
            _userCollectService = userCollectService;
            _shareInfoService = shareInfoService;
            _examQuestionService = examQuestionService;
            _examService = examService;
            _options = options;
            _contextAccessor = contextAccessor;
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetUserInfo()
        {
            var data = await _userInfoService.GetUserInfoAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 查询收藏数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadCollect(AppUserCollectQueryInput queryInput)
        {
            //获取用户收藏数据
            var collects = await _userCollectService.GetUserAppCollectByUserIdIdAsync(queryInput);

            var objectIds = collects.Data.Select(t => t.ObjectId).ToList();

            if (!objectIds.Any())
            {
                return new ResponseDto();
            }

            switch (queryInput.CollectType)
            {
                case 0:
                    var questions = await _questionInfoService.LoadQuestionInfoWhereListAsync(new IService.Dtos.Inputs.Web.QuestionInfoQueryInput
                    {
                        QuestionIds = objectIds
                    });
                    foreach (var question in questions)
                    {
                        question.CollectId = collects.Data.First(t => t.ObjectId == question.QuestionId).CollectId;
                    }
                    return new ResponseDto(new PageDto<QuestionInfoDto>
                    {
                        Data = questions,
                        PageIndex = collects.PageIndex,
                        PageSize = collects.PageSize,
                        TotalCount = collects.TotalCount,
                    });
                case 1:
                    var shares = await _shareInfoService.LoadShareWhereListAsync(new IService.Dtos.Inputs.Web.ShareInfoQueryInput
                    {
                        ShareIds = objectIds
                    });
                    foreach (var share in shares)
                    {
                        share.CollectId = collects.Data.First(t => t.ObjectId == share.ShareId).CollectId;
                    }
                    return new ResponseDto(new PageDto<ShareInfoDto>
                    {
                        Data = shares,
                        PageIndex = collects.PageIndex,
                        PageSize = collects.PageSize,
                        TotalCount = collects.TotalCount,
                    });
                case 2:
                    var exams = await _examQuestionService.LoadExamQuestionWhereListAsync(new IService.Dtos.Inputs.Web.ExamQuestionQueryInput
                    {
                        ExamQuestionIds = objectIds
                    });
                    foreach (var exam in exams)
                    {
                        exam.CollectId = collects.Data.First(t => t.ObjectId == exam.QuestionId).CollectId;
                    }
                    return new ResponseDto(new PageDto<ExamQuestionDto>
                    {
                        Data = exams,
                        PageIndex = collects.PageIndex,
                        PageSize = collects.PageSize,
                        TotalCount = collects.TotalCount,
                    });
            }
            return new ResponseDto(false, "参数错误");
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="showNextDetailInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetCollectNext(ShowNextDetailInput showNextDetailInput)
        {
            var collect = await _userCollectService.ShowDetailNextAsync(showNextDetailInput);

            switch (showNextDetailInput.CollectType)
            {
                case 0:
                    var questions = await _questionInfoService.LoadQuestionInfoWhereListAsync(new IService.Dtos.Inputs.Web.QuestionInfoQueryInput
                    {
                        QuestionIds = new List<long> { collect.CollectId }
                    });
                    return new ResponseDto(questions);
                case 1:
                    var shares = await _shareInfoService.LoadShareWhereListAsync(new IService.Dtos.Inputs.Web.ShareInfoQueryInput
                    {
                        ShareIds = new List<long> { collect.CollectId }
                    });
                    return new ResponseDto(shares);
            }

            return new ResponseDto(false, "参数错误");
        }

        /// <summary>
        /// 我的考试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadMyExam(PageInput pageInput)
        {
            var exams = await _examService.LoadUserExamAsync(pageInput);

            return new ResponseDto(exams);
        }

        /// <summary>
        /// 我的错题集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadWrongQuestion(PageInput pageInput)
        {
            var exams = await _examService.LoadUserWroingExamAsync(pageInput);

            return new ResponseDto(exams);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="avatarFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> UploadAvatar(IFormFile avatarFile)
        {
            var uploadFolder = Path.Combine(_options.Value.PhysicalPath, "avatar");
            //202510/
            if (Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            //登陆人id
            string userId = HttpContext.User.Claims.First(t => t.Type == "UserId").Value;

            // 为文件重命名，防止文件重名
            var extension = Path.GetExtension(avatarFile.FileName);
            var fileName = userId + extension;

            // 文件保存的文件夹路径
            var fileFullPath = Path.Combine(uploadFolder, fileName);

            using var targetStream = System.IO.File.Create(fileFullPath);

            await avatarFile.CopyToAsync(targetStream);

            //todo:生成缩略图

            targetStream.Dispose();

            //保存用户头像
            await _userInfoService.UpDateUserAvatarAsync("/avatar/" + fileName);

            return new ResponseDto(fileFullPath);
        }

        /// <summary>
        /// 获取用户头像地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetUserAvatar()
        {

            //E:\easywecaht\upload
            string path = _options.Value.PhysicalPath;

            //查询用户信息
            var userInfo = await _userInfoService.GetUserInfoAsync();
            if (string.IsNullOrWhiteSpace(userInfo.Avatar))
            {
                return new ResponseDto { Result = "" };
            }

            //E:\\easywecaht\\upload\\avatar\\....jpg
            string avatarUrl = Path.Combine(path, userInfo.Avatar.TrimStart('/').Replace('/', '\\'));

            string serverUrl = PhysicalPathToUrl(avatarUrl);

            return new ResponseDto { Result = serverUrl };
        }

        /// <summary>
        /// 物理路径转成网络路径
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        [NonAction]
        public string PhysicalPathToUrl(string physicalPath)
        {
            string rootPath = _options.Value.PhysicalPath;
            string relativePath = Path.GetRelativePath(rootPath, physicalPath);
            string urlPath = relativePath.Replace('\\', '/'); // 转换 \ 为 /


            string serverUrl = $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext!.Request.Host}{_options.Value.virtualPath}/{urlPath}";
            return serverUrl;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> UpdateUserInfo(UpdateUserInfoInput userInfoInput)
        {
            await _userInfoService.UpdateUserInfoAsync(userInfoInput);

            return new ResponseDto();
        }

        /// <summary>
        /// 获取反馈列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadFeedback(PageInput pageInput)
        {
            var data = await _feedbackService.LoadFeedbackListAsync(new FeedbackQueryInput
            {
                PageIndex = pageInput.PageIndex,
                PageSize = pageInput.PageSize,
                ParentFeekbackId = 0
            });

            return new ResponseDto(data);
        }

        /// <summary>
        /// 获取反馈回复
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> LoadFeedbackReply(long parentFeekback)
        {
            var data = await _feedbackService.FeedbackDetailAsync(parentFeekback);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RateLimit(limit: 20, seconds: 86400)]
        public async Task<ResponseDto> SendFeedback(ReplayFeedbackInput replayFeedbackInput)
        {
            replayFeedbackInput.UserId = Convert.ToInt64(HttpContext.User.Claims.First(t => t.Type == "UserId").Value);

            await _feedbackService.ReplayFeedbackAsync(replayFeedbackInput);

            return new ResponseDto();
        }
    }
}
