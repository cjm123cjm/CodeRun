using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    /// <summary>
    /// 考题管理
    /// </summary>
    public interface IExamQuestionService
    {
        /// <summary>
        /// 加载题库数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<ExamQuestionDto>> LoadExamQuestionListAsync(ExamQuestionQueryInput queryInput);

        /// <summary>
        /// 添加/修改 考题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveExamQuestionAsync(ExamQuestionAddOrUpdateInput input);

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExamQuestionAddOrUpdateInput> ExamQuestionByIdAsync(long id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">,号切割</param>
        /// <returns></returns>
        Task DeletedExamQuestionAsync(string ids);

        /// <summary>
        /// 发布/取消
        /// </summary>
        /// <param name="questionIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateStatusExamQuestionAsync(string questionIds, int status);

        /// <summary>
        /// 上一页/下一页
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<ExamQuestionAddOrUpdateInput> ShowExamQuestionDetailNextAsync(ExamQuestionQueryInput queryInput);

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="importDtos"></param>
        /// <returns></returns>
        Task BatchImportExamQuestionAsync(List<ExamQuestionImportDto> importDtos);
    }
}
