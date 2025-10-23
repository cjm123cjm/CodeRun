using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.category_list)]
        public async Task<ResponseDto> LoadCategoryList()
        {
            var data = await _categoryService.LoadCategoryListAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.category_edit)]
        public async Task<ResponseDto> SaveCategory(CategoryDto categoryDto)
        {
            await _categoryService.SaveCategoryAsync(categoryDto);

            return new ResponseDto();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.category_del)]
        public async Task<ResponseDto> DeleteCategory(long categoryId)
        {
            await _categoryService.DeleteCategoryAsync(categoryId);

            return new ResponseDto();

        }

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="categoriesId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.category_edit)]
        public async Task<ResponseDto> ChangeCategorySort(string categoriesId)
        {
            await _categoryService.ChangeCategorySortAsync(categoriesId);

            return new ResponseDto();
        }

        /// <summary>
        /// 根据类型获取分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("{type}")]
        [PermissionAuthorize(PermissionCodeEnum.category_list)]
        public async Task<ResponseDto> LoadCategoryListByType(int type)
        {
            var data = await _categoryService.LoadCategoryListByTypeAsync(type);

            return new ResponseDto(data);
        }
    }
}
