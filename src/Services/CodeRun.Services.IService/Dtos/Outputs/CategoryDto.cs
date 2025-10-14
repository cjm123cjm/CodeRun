namespace CodeRun.Services.IService.Dtos.Outputs
{
    public class CategoryDto
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string IconPath { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BgColor { get; set; }
        /// <summary>
        /// 0:问题分类，1:考题分类,2:问题分类和考题分类
        /// </summary>
        public int Type { get; set; }
    }
}
