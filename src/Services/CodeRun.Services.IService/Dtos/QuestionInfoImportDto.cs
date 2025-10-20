using System.ComponentModel.DataAnnotations;

namespace CodeRun.Services.IService.Dtos
{
    /// <summary>
    /// 问题批量导入dto
    /// </summary>
    public class QuestionInfoImportDto
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
        /// 问题描述
        /// </summary>
        [Display(Name = "问题描述")]
        public string Question { get; set; } = null!;
        /// <summary>
        /// 回答解释
        /// </summary>
        [Display(Name = "回答解释")]
        public string? AnswerAnalysis { get; set; }
    }
}
