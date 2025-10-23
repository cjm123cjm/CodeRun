namespace CodeRun.Services.Domain.Entities.Web
{
    /// <summary>
    /// 考试问题选项
    /// </summary>
    public class ExamQuestionItem
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
