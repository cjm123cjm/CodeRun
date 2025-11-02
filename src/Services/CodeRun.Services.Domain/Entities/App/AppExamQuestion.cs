namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// 考试问题
    /// </summary>
    public class AppExamQuestion
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 考试id
        /// </summary>
        public long ExamId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 问题id
        /// </summary>
        public long QuestionId { get; set; }
        /// <summary>
        /// 用户答案
        /// </summary>
        public string UserAnswer { get; set; } = null!;
        /// <summary>
        /// 0:未作答,1:正确,2:错误
        /// </summary>
        public int AnswerStatus { get; set; }
    }
}
