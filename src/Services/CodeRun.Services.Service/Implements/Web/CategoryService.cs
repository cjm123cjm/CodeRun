using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.Domain.Repository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Service.Implements.Web
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDto>> LoadCategoryListAsync()
        {
            var categories = await _categoryRepository.Query().AsNoTracking().OrderBy(t => t.Sort).ToListAsync();

            var categoryDtos = ObjectMapper.Map<List<CategoryDto>>(categories);

            return categoryDtos;
        }

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        public async Task SaveCategoryAsync(CategoryDto categoryDto)
        {
            //名称是否重复
            var anyName = await _categoryRepository.QueryWhere(t => t.CategoryId != categoryDto.CategoryId && t.CategoryName == categoryDto.CategoryName).AnyAsync();
            if (anyName)
            {
                throw new BusinessException("分类名称已存在,请更换");
            }

            if (categoryDto.CategoryId == 0)
            {
                var category = ObjectMapper.Map<Category>(categoryDto);
                category.CategoryId = SnowIdWorker.NextId();

                //查询排序
                int count = await _categoryRepository.CountAsync(t => true);

                category.Sort = count + 1;

                await _categoryRepository.AddAsync(category);
            }
            else
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.CategoryId);
                if (category == null)
                {
                    throw new BusinessException("数据不存在");
                }

                if (category.CategoryName != categoryDto.CategoryName)
                {
                    ObjectMapper.Map(categoryDto, category);

                    await _categoryRepository.UpdateCategoryAsync(category);
                }
                else
                {
                    ObjectMapper.Map(categoryDto, category);

                    _categoryRepository.Update(category);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task DeleteCategoryAsync(long categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new BusinessException("数据不存在");
            }

            _categoryRepository.Delete(category);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="categoriesId"></param>
        /// <returns></returns>
        public async Task ChangeCategorySortAsync(string categoriesId)
        {
            var ids = categoriesId.Split(",").Select(t => Convert.ToInt64(t)).ToList();

            var categories = await _categoryRepository.QueryWhere(t => ids.Contains(t.CategoryId), true).ToListAsync();

            int index = 1;
            foreach (var item in categories)
            {
                item.Sort = index;

                index++;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据类型获取分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<CategoryDto>> LoadCategoryListByTypeAsync(int type)
        {
            var categories = await _categoryRepository.QueryWhere(t => t.Type == 2 || t.Type == type).OrderBy(t => t.Sort).ToListAsync();

            var categoryDtos = ObjectMapper.Map<List<CategoryDto>>(categories);

            return categoryDtos;
        }
    }
}
