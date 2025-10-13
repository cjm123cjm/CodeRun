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
    public class RoleService : ServiceBase, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleForMenuRepository _roleForMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IRoleForMenuRepository roleForMenuRepository,
            IAccountRepository accountRepository)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _roleForMenuRepository = roleForMenuRepository;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// 加载角色列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<List<RoleDto>> LoadRoleListAsync(RoleQueryInput queryInput)
        {
            var query = _roleRepository.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(queryInput.RoleName))
            {
                query = query.Where(t => t.RoleName.Contains(queryInput.RoleName));
            }
            if (!string.IsNullOrWhiteSpace(queryInput.RoleDesc))
            {
                query = query.Where(t => t.RoleDesc != null && t.RoleDesc.Contains(queryInput.RoleDesc));
            }

            var roles = await query.ToListAsync();

            var roleDtos = ObjectMapper.Map<List<RoleDto>>(roles);

            return roleDtos;
        }

        /// <summary>
        /// 添加(角色+角色权限)/修改(角色)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveRoleAsync(RoleAddOrUpdateInput input)
        {
            int count = await _roleRepository.QueryWhere(t => t.RoleName == input.RoleName && t.RoleId != input.RoleId).CountAsync();
            if (count != 0)
            {
                throw new BusinessException("角色名称已存在,请更换");
            }
            //添加
            if (input.RoleId == 0)
            {
                //保存数据
                var role = ObjectMapper.Map<Role>(input);
                role.RoleId = SnowIdWorker.NextId();

                //保存菜单
                await SaveRoleMenu(input.RoleId, input.MenuIds, input.HalfMenuIds);

                await _roleRepository.AddAsync(role);
            }
            else
            {
                var role = await _roleRepository.GetByIdAsync(input.RoleId);
                if (role == null)
                {
                    throw new BusinessException("数据不存在");
                }

                ObjectMapper.Map(input, role);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 保存角色菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds">全选菜单id</param>
        /// <param name="halfMenuIds">半选菜单id</param>
        /// <returns></returns>
        public async Task SaveRoleMenu(long roleId, string menuIds, string halfMenuIds)
        {
            var oldMenuIds = await _roleForMenuRepository.QueryWhere(t => t.RoleId == roleId, true).ToListAsync();
            if (oldMenuIds.Any())
            {
                _roleForMenuRepository.Delete(oldMenuIds.ToArray());
            }

            var menuIdSplit = menuIds.Split(",").Select(t => Convert.ToInt64(t)).ToList();
            var halfMenuIdSplit = string.IsNullOrWhiteSpace(halfMenuIds) ? new List<long>() : halfMenuIds.Split(",").Select(t => Convert.ToInt64(t)).ToList();

            List<RoleForMenu> saveMenu = new List<RoleForMenu>();
            foreach (var item in menuIdSplit)
            {
                RoleForMenu roleForMenu = new RoleForMenu
                {
                    RoleId = roleId,
                    MenuId = item,
                    CheckType = 1
                };
                saveMenu.Add(roleForMenu);
            }

            foreach (var item in halfMenuIdSplit)
            {
                RoleForMenu roleForMenu = new RoleForMenu
                {
                    RoleId = roleId,
                    MenuId = item,
                    CheckType = 0
                };
                saveMenu.Add(roleForMenu);
            }

            if (saveMenu.Any())
            {
                await _roleForMenuRepository.AddAsync(saveMenu.ToArray());
            }
        }

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveRoleMenuAsync(RoleAddOrUpdateInput input)
        {
            if (string.IsNullOrWhiteSpace(input.MenuIds))
            {
                throw new BusinessException(input.MenuIds + "字段不能为空");
            }
            var role = await _roleRepository.GetByIdAsync(input.RoleId);
            if (role == null)
            {
                throw new BusinessException("数据不存在");
            }

            await SaveRoleMenu(input.RoleId, input.MenuIds, input.HalfMenuIds);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据角色id查询菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RoleDto> RoleMenuByRoleIdAsync(long roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            var menuIds = await _roleForMenuRepository.GetMenuIdsByRoleIdAsync(roleId);

            var roleDto = ObjectMapper.Map<RoleDto>(role);

            roleDto.MenuIds = menuIds;

            return roleDto;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeletedRoleAsync(long roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                throw new BusinessException("数据不存在");
            }

            //查询该角色有没有用户使用
            var roleAny = await _accountRepository.QueryWhere(t => t.Status == 1 && t.Roles != null && t.Roles.Contains(roleId.ToString())).AnyAsync();
            if (roleAny)
            {
                throw new BusinessException(message: "该角色正在使用,无法删除");
            }

            _roleRepository.Delete(role);

            //删除权限菜单
            var menus = await _roleForMenuRepository.QueryWhere(t => t.RoleId == roleId, true).ToListAsync();
            if (menus.Any())
            {
                _roleForMenuRepository.Delete(menus.ToArray());
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
