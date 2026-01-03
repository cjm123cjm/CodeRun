using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    /// <summary>
    /// 八股文添加或修改输入参数
    /// </summary>
    public class QuestionInfoAddOrUpdateInput
    {
        /// <summary>
        /// id
        /// </summary>
        public long QuestionId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string? CategoryName { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public int DifficultyLevel { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Question { get; set; } = null!;
        /// <summary>
        /// 回答解释
        /// </summary>
        public string? AnswerAnalysis { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsCollect { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
