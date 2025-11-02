using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppExamService
    {
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
    }
}
