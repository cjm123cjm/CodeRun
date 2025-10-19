namespace CodeRun.Services.IService.Dtos.Inputs
{
    public class QuestionInfoQueryInput : PageInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public int? DifficultyLevel { get; set; }
        /// <summary>
        /// 状态:0-未发布 1-已发布
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string? CreatedUserName { get; set; }
    }
}
