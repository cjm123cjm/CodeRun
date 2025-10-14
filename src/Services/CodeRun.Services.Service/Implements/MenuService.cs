using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements
{
    public class MenuService : ServiceBase, IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuTreeDto>> LoadMenuTreeAsync()
        {
            var menus = await _menuRepository.QueryWhere(null).ToListAsync();

            menus.Add(new Menu
            {
                MenuId = 0,
                ParentId = -1,
                MenuName = "所有菜单"
            });

            var treeDtos = ObjectMapper.Map<List<MenuTreeDto>>(menus);

            var menuTreeDtos = BuildTreeMenu(treeDtos, -1);

            return menuTreeDtos;
        }

        /// <summary>
        /// 添加/修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveMenuAsync(MenuAddOrUpdateInput input)
        {
            //添加
            if (input.MenuId == 0)
            {
                int permissCodeCount = await _menuRepository.QueryWhere(t => t.PermissionCode == input.PermissionCode).CountAsync();
                if (permissCodeCount != 0)
                {
                    throw new BusinessException(200, input.PermissionCode + "已存在");
                }

                var menu = ObjectMapper.Map<Menu>(input);

                await _menuRepository.AddAsync(menu);
            }
            else
            {
                int permissCodeCount = await _menuRepository.QueryWhere(t => t.PermissionCode == input.PermissionCode && t.MenuId != input.MenuId).CountAsync();
                if (permissCodeCount != 0)
                {
                    throw new BusinessException(200, input.PermissionCode + "已存在");
                }

                var menu = await _menuRepository.GetByIdAsync(input.MenuId);
                if (menu == null)
                {
                    throw new BusinessException(200, "数据不存在");
                }

                ObjectMapper.Map(input, menu);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单id</param>
        /// <returns></returns>
        public async Task DeletedMenuAsync(long menuId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
            {
                throw new BusinessException(200, "数据不存在");
            }

            _menuRepository.Delete(menu);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
