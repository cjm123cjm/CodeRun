using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Interfaces.Web
{
    public interface IQuestionInfoService
    {
        /// <summary>
        /// 加载八股文列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<QuestionInfoDto>> LoadQuestionInfoListAsync(QuestionInfoQueryInput queryInput);

        /// <summary>
        /// 添加或修改八股文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task QuestionInfoAddOrUpdateAsync(QuestionInfoAddOrUpdateInput input);

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuestionInfoAddOrUpdateInput> QuestionInfoByIdAsync(long id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <returns></returns>
        Task DeleteQuestionInfoAsync(string questionInfoIds);

        /// <summary>
        /// 发布/取消发布
        /// </summary>
        /// <param name="questionInfoIds">根据,号分割</param>
        /// <param name="status">0-未发布,1-已发布</param>
        /// <returns></returns>
        Task UpdateStatusQuestionInfoAsync(string questionInfoIds, int status);

        /// <summary>
        /// 批量导入问题
        /// </summary>
        /// <param name="importDtos"></param>
        /// <returns></returns>
        Task BatchImportQuestionInfoAsync(List<QuestionInfoImportDto> importDtos);

        /// <summary>
        /// 上一页/下一页查看
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<QuestionInfoAddOrUpdateInput> ShowQuestionInfoDetailNextAsync(QuestionInfoQueryInput queryInput);
    }
}
