using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 提交考试输入参数
    /// </summary>
    public class PostExamInput
    {
        public long ExamId { get; set; }
        public string? Remark { get; set; }
        public List<AppExamQuestionDto> appExamQuestionDtos { get; set; } = new();
    }
}
