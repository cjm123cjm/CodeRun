using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    /// <summary>
    /// 八股文添加或修改输入参数
    /// </summary>
    public class QuestionInfoAddOrUpdateInput : QuestionInfoDto
    {
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Question { get; set; } = null!;
        /// <summary>
        /// 回答解释
        /// </summary>
        public string AnswerAnalysis { get; set; } = null!;

        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsCollect { get; set; }
    }
}
