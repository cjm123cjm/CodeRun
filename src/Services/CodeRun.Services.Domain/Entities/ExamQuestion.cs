namespace CodeRun.Services.Domain.Entities
{
    /// <summary>
    /// 考试题目
    /// </summary>
    public class ExamQuestion
    {
        /// <summary>
        /// 考试题目id
        /// </summary>
        public long QuestionId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public int DifficultyLevel { get; set; }
        /// <summary>
        /// 问题类型:0-判断，1-单选题，2-多选
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string QuestionAnswer { get; set; }
        /// <summary>
        /// 回答解释
        /// </summary>
        public string AnswerAnalysis { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态:0-未发布 1-已发布
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreatedUserId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatedUserName { get; set; }
        /// <summary>
        /// 0-内部 1-外部投稿
        /// </summary>
        public int PostUserType { get; set; }
    }
}
