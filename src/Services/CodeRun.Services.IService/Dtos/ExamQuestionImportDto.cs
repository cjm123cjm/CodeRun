using System.ComponentModel.DataAnnotations;

namespace CodeRun.Services.IService.Dtos
{
    public class ExamQuestionImportDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string Title { get; set; } = null!;
        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "分类")]
        public string CategoryName { get; set; } = null!;
        /// <summary>
        /// 难度等级
        /// </summary>
        [Display(Name = "难度等级")]
        public int DifficultyLevel { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        [Display(Name = "问题类型")]
        public string QuestionType { get; set; }

        /// <summary>
        /// 问题选项
        /// </summary>
        [Display(Name = "问题选项")]
        public string QuestionItems { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        [Display(Name = "问题描述")]
        public string Question { get; set; } = null!;
        /// <summary>
        /// 答案
        /// </summary>
        public string QuestionAnswer { get; set; }
        /// <summary>
        /// 答案分析
        /// </summary>
        [Display(Name = "答案分析")]
        public string? AnswerAnalysis { get; set; }
    }
}
