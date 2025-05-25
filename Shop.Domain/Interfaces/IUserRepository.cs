using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IUserRepository
    {
        #region Account
        Task<bool> IsUserExistPhoneNumber(string phoneNumber);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> GetUserById(long userId);
        Task CreateUser(User user);
        void UpdateUser(User user);
        Task SaveChange();
        #endregion

        #region Admin
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filterUser);
        Task<EditUserFromAdminViewModel> GetEditUserFromAdmin(long userId);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filterRoles);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<List<Permission>> GetAllActivePermission();
        Task RemoveAllPermissionSelectedByRole(long roleId);
        Task AddPermissionToRole(List<long> selectedPermission, long roleId);
        bool CheckPermission(long permissionId, string phoneNumber);

        Task<List<Role>> GetAllActiveRole();
        Task RemoveAllRoleSelectedByRole(long userId);
        Task AddRoleToUser(List<long> selectedRole, long userId);

        Task<Role> GetRoleById(long id);
        Task CreateRole(Role role); 
        void UpdateRole(Role role);

        #endregion
    }
}
