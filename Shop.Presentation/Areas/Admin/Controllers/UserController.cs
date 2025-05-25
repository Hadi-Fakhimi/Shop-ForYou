using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Presentation.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        #region Constructor
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }
        #endregion

        #region Filter User
        public async Task<IActionResult> FilterUser(FilterUserViewModel filterUser)
        {

            return View(await _userService.FilterUsers(filterUser));
        }
        #endregion

        #region Edit User From Admin
        [HttpGet]
        public async Task<IActionResult> EditUser(long userId)
        {

            var user = await _userService.GetEditUserFromAdmin(userId);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = await _userService.GetAllActiveRole();
            return View(user);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserFromAdminViewModel editUser)
        {
            ViewData["Roles"] = await _userService.GetAllActiveRole();
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserFromAdmin(editUser);
                switch (result)
                {
                    case EditUserFromAdminRerult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case EditUserFromAdminRerult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterUser));

                }
            }
            return View(editUser);
        }
        #endregion

        #region Filter Roles
        public async Task<IActionResult> FilterRole(FilterRolesViewModel filterRoles)
        {
            return View(await _userService.FilterRoles(filterRoles));
        }
        #endregion

        #region Create Role
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            ViewData["Permission"] = await _userService.GetAllActivePermission();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateOrEditRoleViewModel createRole)
        {
            ViewData["Permission"] = await _userService.GetAllActivePermission();
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateOrEditRole(createRole);
                switch (result)
                {
                    case CreateOrEditRoleResult.NotFound:
                        TempData[ErrorMessage] = "نقش مورد نظر بافت نشد ";
                        break;
                    case CreateOrEditRoleResult.NotExistPermission:
                        TempData[ErrorMessage] = "لطفا نقشی را انتخاب کنید";
                        break;
                    case CreateOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "نقش جدید با موفقیت ایجاد شد ";

                        return RedirectToAction(nameof(FilterRole));

                }
            }
            return View(createRole);
        }
        #endregion

        #region Edit Role
        [HttpGet]
        public async Task<IActionResult> EditRole(long roleId)
        {
            ViewData["Permission"] = await _userService.GetAllActivePermission();
            var role = await _userService.GetEditRoleById(roleId);
            if (role == null)
            {
                TempData[ErrorMessage] = "نقش مورد نظر بافت نشد ";
                return View();
            }

            return View(role);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(CreateOrEditRoleViewModel editRole)
        {
            ViewData["Permission"] = await _userService.GetAllActivePermission();

            if (ModelState.IsValid)
            {
                var result = await _userService.CreateOrEditRole(editRole);
                switch (result)
                {
                    case CreateOrEditRoleResult.NotFound:
                        TempData[ErrorMessage] = "نقش مورد نظر بافت نشد ";
                        break;
                    case CreateOrEditRoleResult.NotExistPermission:
                        TempData[ErrorMessage] = "لطفا نقشی را انتخاب کنید";
                        break;
                    case CreateOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "نقش جدید با موفقیت ایجاد شد ";

                        return RedirectToAction(nameof(FilterRole));

                }
            }
            return View(editRole);
        }
        #endregion
    }
}
