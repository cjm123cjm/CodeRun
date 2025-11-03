using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Dtos.Outputs.App
{
    public class UserExamQuestionListDto
    {
        /// <summary>
        /// AppExamQuestionId
        /// </summary>
        public long AppExamQuestionId { get; set; }
        /// <summary>
        /// 考试题目id
        /// </summary>
        public long QuestionId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public int DifficultyLevel { get; set; }
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
        /// 问题类型:0-判断，1-单选题，2-多选
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 用户答案
        /// </summary>
        public string? UserAnswer { get; set; }
        /// <summary>
        /// 0:未作答,1:正确,2:错误
        /// </summary>
        public int AnswerStatus { get; set; }
        /// <summary>
        /// 考试id
        /// </summary>
        public long ExamId { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool HaveCollect { get; set; }
        /// <summary>
        /// 问题选项
        /// </summary>
        public List<ExamQuestionItemDto> QuestionItemList { get; set; } = new();
    }
}
