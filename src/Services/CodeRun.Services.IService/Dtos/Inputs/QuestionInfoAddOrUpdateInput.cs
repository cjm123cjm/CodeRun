using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Dtos.Inputs
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
    }
}
