using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Infra.Data.Context;

namespace Shop.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Account
        Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register);
        Task<LoginUserResult> LoginUser(LoginUserViewModel loginUser);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<ActiveAccountResult> ActiveAccount(ActiveAccountViewModel activeAccount);
        Task<User> GetUserById(long userId);
        bool CheckPermission(long permissionId, string phoneNumber);
        #endregion

        #region Profile
        Task<EditUserProfileViewModel> GetUserEditProfileById(long userId);
        Task<EditUserProfileRerult> EditUserProfile(long userId,IFormFile userAvatar,EditUserProfileViewModel editUserProfile);
        Task<ChangePasswordResult> ChangeUserPassword(long userId,ChangePasswordViewModel changePassword);
        #endregion

        #region Admin
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filterUser);
        Task<EditUserFromAdminViewModel> GetEditUserFromAdmin(long userId);
        Task<EditUserFromAdminRerult> EditUserFromAdmin(EditUserFromAdminViewModel editUserFromAdmin);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filterRoles);
        Task<List<Permission>> GetAllActivePermission();
        Task<List<Role>> GetAllActiveRole();
        #endregion
    }
}
