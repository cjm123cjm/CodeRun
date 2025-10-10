using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Service.Implementations
{
    public class MenuService : IMenuService
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

            //todo:转成MenuTreeDto

            List<MenuTreeDto> menuTreeDtos = new List<MenuTreeDto>();

            menus.Add(new Menu
            {
                MenuId = 0,
                ParentId = -1,
                MenuName = "所有菜单"
            });

            return menuTreeDtos;
        }

        private List<MenuTreeDto> BuildTreeMenu(List<MenuTreeDto> menus, long parentId)
        {
            List<MenuTreeDto> menuTrees = new List<MenuTreeDto>();
            foreach (var item in menus)
            {
                if (item.ParentId == parentId)
                {
                    item.ChildMenu.AddRange(BuildTreeMenu(menus, item.MenuId));
                    menuTrees.Add(item);
                }
            }

            return menuTrees;
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

                //todo:转成model
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
                //todo:转成model

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
