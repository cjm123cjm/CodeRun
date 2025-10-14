using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDto>> LoadCategoryListAsync();

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        Task SaveCategoryAsync(CategoryDto categoryDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task DeleteCategoryAsync(long categoryId);

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="categoriesId"></param>
        /// <returns></returns>
        Task ChangeCategorySortAsync(string categoriesId);

        /// <summary>
        /// 根据类型获取分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<CategoryDto>> LoadCategoryListByTypeAsync(int type);
    }
}
