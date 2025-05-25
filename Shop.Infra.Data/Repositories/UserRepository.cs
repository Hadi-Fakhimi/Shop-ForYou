using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Domain.ViewModels.Pageing;
using Shop.Infra.Data.Context;

namespace Shop.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private readonly ShopDbContext _context;
        public UserRepository(ShopDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Account
        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
        }



        public async Task<User> GetUserById(long userId)
        {
            return await _context.Users.AsQueryable().SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.SingleOrDefaultAsync(u =>  u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsUserExistPhoneNumber(string phoneNumber)
        {
            return await _context.Users.AsQueryable().AnyAsync(u => u.PhoneNumber == phoneNumber);

        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public bool CheckPermission(long permissionId, string phoneNumber)
        {
            long userId = _context.Users.AsQueryable().Single(c => c.PhoneNumber == phoneNumber).Id;

            var userRole = _context.UserRoles.AsQueryable()
                .Where(c => c.UserId == userId).Select(c => c.RoleId).ToList();

            if(!userRole.Any()) return false;

            var permissions = _context.RolePermissions.AsQueryable()
                .Where(c =>c.PermissionId == permissionId).Select(c => c.RoleId).ToList();

            return permissions.Any(c => userRole.Contains(c));

        }
        #endregion

        #region Admin
        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filterUser)
        {
            var query = _context.Users.AsQueryable();


            #region Filter
            if (!string.IsNullOrEmpty(filterUser.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber == filterUser.PhoneNumber);
            }
            #endregion

            #region Pageing
            var pager = Pager.Build(filterUser.PageId, await query.CountAsync(), filterUser.TakeEntity, filterUser.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filterUser.SetPageing(pager).SetUsers(allData);
        }

        public async Task<EditUserFromAdminViewModel> GetEditUserFromAdmin(long userId)
        {
            return await _context.Users.AsQueryable().Include(c => c.UserRoles)
                .Where(c => c.Id == userId).
                Select(c => new EditUserFromAdminViewModel()
                {

                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    UserGender = c.UserGender,
                    RoleIds = c.UserRoles.Where(c => c.UserId  == userId).Select(c => c.RoleId).ToList()

                }).SingleOrDefaultAsync();

        }

        public async Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId)
        {
            return await _context.Roles.AsQueryable().Include(x => x.RolePermissions)
                .Where(c => c.Id == roleId)
                .Select(c => new CreateOrEditRoleViewModel()
                {
                    Id = c.Id,
                    RoleTitle = c.RoleTitle,
                    SelectedPermissions = c.RolePermissions.Select(c => c.PermissionId).ToList()
                }).SingleOrDefaultAsync();
        }

        public async Task CreateRole(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
        }

        public async Task<Role> GetRoleById(long id)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filterRoles)
        {
            var query =  _context.Roles.AsQueryable();


            #region Filter
            if (!string.IsNullOrEmpty(filterRoles.RoleName))
            {
                query = query.Where(q => EF.Functions.Like(q.RoleTitle,$"%{filterRoles.RoleName}%"));
            }
            #endregion

            #region Pageing
            var pager = Pager.Build(filterRoles.PageId, await query.CountAsync(), filterRoles.TakeEntity, filterRoles.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filterRoles.SetPageing(pager).SetRoles(allData);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _context.Permissions.Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task RemoveAllPermissionSelectedByRole(long roleId)
        {
            var allRolePermissions = await _context.RolePermissions.Where(c => c.RoleId == roleId).ToListAsync();

            if (allRolePermissions.Any())
            {
                _context.RolePermissions.RemoveRange(allRolePermissions);
            }
        }

        public async Task AddPermissionToRole(List<long> selectedPermission, long roleId)
        {
            var rolePermissions = new List<RolePermission>();
            foreach (var permissionId in selectedPermission)
            {
                rolePermissions.Add(new RolePermission()
                {
                    PermissionId = permissionId,
                    RoleId = roleId
                });
            }

            await _context.AddRangeAsync(rolePermissions);
            
        }

        public async Task<List<Role>> GetAllActiveRole()
        {
            return await _context.Roles.AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task RemoveAllRoleSelectedByRole(long userId)
        {
            var allUserRole = await _context.UserRoles.AsQueryable().Where(c => c.UserId == userId).ToListAsync();
            if (allUserRole.Any())
            {
                _context.UserRoles.RemoveRange(allUserRole);
            }
        }

        public async Task AddRoleToUser(List<long> selectedRole, long userId)
        {
            if (selectedRole != null && selectedRole.Any())
            {
                var userRoles = new List<UserRole>();
                foreach (var roleId in selectedRole)
                {
                    userRoles.Add(new UserRole()
                    {
                        RoleId = roleId,
                        UserId = userId
                    });
                }

                await _context.UserRoles.AddRangeAsync(userRoles);
            }
        }




        #endregion
    }
}
