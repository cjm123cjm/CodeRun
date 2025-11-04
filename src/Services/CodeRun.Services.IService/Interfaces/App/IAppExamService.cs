using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppExamService
    {
        /// <summary>
        /// 获取用户已考试列表
        /// </summary>
        /// <returns></returns>
        Task<PageDto<AppExamDto>> LoadUserExamAsync(PageInput pageInput);

        /// <summary>
        /// 查询用户是否有未完成的考试
        /// </summary>
        /// <returns></returns>
        Task<List<AppExamDto>> CheckUserNoFinishedExamAsync();

        /// <summary>
        /// 创建考试
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        Task<AppExamDto> CreateExamAsync(string categoryIds);

        /// <summary>
        /// 获取考试和考题信息
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        Task<AppExamDto> GetExamQuestionAsync(long examId);

        /// <summary>
        /// 开始考试
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        Task StartExamAsync(long examId);

        /// <summary>
        /// 提交考试
        /// </summary>
        /// <param name="postExamInput"></param>
        /// <returns></returns>
        Task<AppExamDto> PostExamAsync(PostExamInput postExamInput);

        /// <summary>
        /// 删除考试
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        Task DeleteExamAsync(long examId);

        /// <summary>
        /// 加载用户错题集
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<PageDto<UserExamQuestionListDto>> LoadUserWroingExamAsync(PageInput pageInput);
    }
}
