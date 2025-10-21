namespace CodeRun.Services.IService.Dtos.Inputs
{
    /// <summary>
    /// 题库查询输入参数
    /// </summary>
    public class ExamQuestionQueryInput : PageInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public int? DifficultyLevel { get; set; }
        /// <summary>
        /// 问题了类型：0-判断，1-单选题，2-多选
        /// </summary>
        public int? QuestionType { get; set; }
        /// <summary>
        /// 状态:0-未发布 1-已发布
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedUserName { get; set; }

        /// <summary>
        /// 当前查询的考题id
        /// </summary>
        public long? CurrentQuestionId { get; set; }
        /// <summary>
        /// 0-上一页,1-下一页
        /// </summary>
        public int Type { get; set; }
    }
}
