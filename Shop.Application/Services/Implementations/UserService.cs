using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using Shop.Application.Generator;
using Shop.Application.Security;
using Shop.Application.Services.Interfaces;
using Shop.Application.StaticTools;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Application.Services.Implementations
{

    public class UserService : IUserService
    {
        #region Constructor
        private readonly IUserRepository _userRepository;
        private readonly ISmsService _smsService;
        public UserService(IUserRepository userRepository, ISmsService smsService)
        {
            _userRepository = userRepository;
            _smsService = smsService;
        }

        #endregion

        #region Account


        public async Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register)
        {
            if (!await _userRepository.IsUserExistPhoneNumber(register.PhoneNumber))
            {
                var newUser = new User()
                {
                    PhoneNumber = register.PhoneNumber,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    IsBloacked = false,
                    IsDelete = false,
                    IsMobileActive = false,
                    UserAvatar = "default.png",
                    UserGender = UserGender.Unknown,
                    Password = PasswordHelper.HashPassword(register.Password),
                    MobileActiveCode = CodeGenerator.GenerateUniqCode()

                };

                await _smsService.SendVerificationCode(newUser.PhoneNumber, newUser.MobileActiveCode);

                await _userRepository.CreateUser(newUser);
                await _userRepository.SaveChange();

                return RegisterUserResult.Success;
            }

            return RegisterUserResult.MobileExists;
        }
        public async Task<LoginUserResult> LoginUser(LoginUserViewModel loginUser)
        {
            var user = await _userRepository.GetUserByPhoneNumber(loginUser.PhoneNumber);
            if (user == null) return LoginUserResult.NotFound;
            if (user.IsBloacked) return LoginUserResult.IsBlocked;
            if (!user.IsMobileActive) return LoginUserResult.NotActive;
            if (user.IsDelete) return LoginUserResult.NotFound;
            if (user.Password != PasswordHelper.HashPassword(loginUser.Password)) return LoginUserResult.NotFound;

            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public async Task<ActiveAccountResult> ActiveAccount(ActiveAccountViewModel activeAccount)
        {
            var user = await _userRepository.GetUserByPhoneNumber(activeAccount.PhoneNumber);
            if (user == null)
            {
                return ActiveAccountResult.NotFound;
            }
            if (user.MobileActiveCode == activeAccount.ActiveCode)
            {
                user.MobileActiveCode = CodeGenerator.GenerateUniqCode();
                user.IsMobileActive = true;
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChange();

                return ActiveAccountResult.Success;
            }

            return ActiveAccountResult.Error;
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        bool IUserService.CheckPermission(long permissionId, string phoneNumber)
        {
            return _userRepository.CheckPermission(permissionId, phoneNumber);
        }
        #endregion

        #region Profile
        public async Task<EditUserProfileViewModel> GetUserEditProfileById(long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return new EditUserProfileViewModel() { };
            }
            return new EditUserProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserGender = user.UserGender
            };
        }

        public async Task<EditUserProfileRerult> EditUserProfile(long userId, IFormFile userAvatar, EditUserProfileViewModel editUserProfile)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileRerult.NotFound;
            user.FirstName = editUserProfile.FirstName;
            user.LastName = editUserProfile.LastName;
            user.PhoneNumber = editUserProfile.PhoneNumber;
            user.UserGender = editUserProfile.UserGender;

            // upload image

            if (userAvatar != null && userAvatar.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(userAvatar.FileName);
                userAvatar.AddImageToServer(imageName, PathsExtensions.UserAvatarOriginServer, 150, 150, PathsExtensions.UserAvatarThumbServer,user.UserAvatar);

                user.UserAvatar = imageName;

            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChange();
            return EditUserProfileRerult.Success;
        }

        public async Task<ChangePasswordResult> ChangeUserPassword(long userId, ChangePasswordViewModel changePassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                var newPassword = PasswordHelper.HashPassword(changePassword.NewPassword);
                if (newPassword != user.Password)
                {
                    user.Password = newPassword;
                    _userRepository.UpdateUser(user);
                    await _userRepository.SaveChange();
                    return ChangePasswordResult.Success;

                }
                return ChangePasswordResult.EqualPassword;

            }



            return ChangePasswordResult.NotFound;
        }


        #endregion

        #region Admin
        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filterUser)
        {
            return await _userRepository.FilterUsers(filterUser);
        }

        public async Task<EditUserFromAdminViewModel> GetEditUserFromAdmin(long userId)
        {
            return await _userRepository.GetEditUserFromAdmin(userId);
        }

        public async Task<EditUserFromAdminRerult> EditUserFromAdmin(EditUserFromAdminViewModel editUserFromAdmin)
        {
            var user = await _userRepository.GetUserById(editUserFromAdmin.Id);

            if (user == null) return EditUserFromAdminRerult.NotFound;

            if (editUserFromAdmin.Password != null)
            {
                user.Password = PasswordHelper.HashPassword(editUserFromAdmin.Password);
            }
            if (editUserFromAdmin.PhoneNumber != null && editUserFromAdmin.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = editUserFromAdmin.PhoneNumber;
                user.IsMobileActive = false;
            }
            user.IsDelete = editUserFromAdmin.IsDelete;
            user.UserGender = editUserFromAdmin.UserGender;
            user.FirstName = editUserFromAdmin.FirstName;
            user.LastName = editUserFromAdmin.LastName;
            user.IsBloacked = editUserFromAdmin.IsBloacked;

            _userRepository.UpdateUser(user);


            await _userRepository.RemoveAllRoleSelectedByRole(editUserFromAdmin.Id);
            await _userRepository.AddRoleToUser(editUserFromAdmin.RoleIds,editUserFromAdmin.Id);

            await _userRepository.SaveChange();
            return EditUserFromAdminRerult.Success;

        }

        public async Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId)
        {
            return await _userRepository.GetEditRoleById(roleId);
        }

        public async Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole)
        {

            //Edit part
            if (createOrEditRole.Id != 0)
            {
                var role = await _userRepository.GetRoleById(createOrEditRole.Id);
                if (role == null) return CreateOrEditRoleResult.NotFound;

                role.RoleTitle = createOrEditRole.RoleTitle;
                _userRepository.UpdateRole(role);

                if(createOrEditRole.SelectedPermissions == null)
                {
                    return CreateOrEditRoleResult.NotExistPermission;
                }

                await _userRepository.RemoveAllPermissionSelectedByRole(createOrEditRole.Id);

                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermissions, createOrEditRole.Id);


                await _userRepository.SaveChange();
                return CreateOrEditRoleResult.Success;

            }
            //create part
            else
            {
                var newRole = new Role()
                {
                    RoleTitle = createOrEditRole.RoleTitle
                };
                await _userRepository.CreateRole(newRole);

                if (createOrEditRole.SelectedPermissions == null)
                {
                    return CreateOrEditRoleResult.NotExistPermission;
                }
                await _userRepository.SaveChange();
                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermissions,newRole.Id);
                await _userRepository.SaveChange();

                return CreateOrEditRoleResult.Success;
            }
        }

        public async Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filterRoles)
        {
            return await _userRepository.FilterRoles(filterRoles);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _userRepository.GetAllActivePermission();
        }

        public async Task<List<Role>> GetAllActiveRole()
        {
            return await _userRepository.GetAllActiveRole();
        }


        #endregion
    }
}
