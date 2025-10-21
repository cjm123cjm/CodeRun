namespace CodeRun.Services.IService.Dtos.Outputs
{
    /// <summary>
    /// 考题选项
    /// </summary>
    public class ExamQuestionItemDto
    {
        /// <summary>
        /// 选项id
        /// </summary>
        public long ItemId { get; set; }
        /// <summary>
        /// 考试题目id
        /// </summary>
        public long QuestionId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
