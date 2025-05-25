using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Account;
using System.Security.Claims;

namespace Shop.Presentation.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService , ICaptchaValidator captchaValidator)
        {
            _userService = userService;

            _captchaValidator = captchaValidator;
        }
        #endregion

        #region Register
        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");

            return View();
        }
        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {
            #region Captcha
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Token))
            {
                TempData[ErrorMessage] = "کد کپچا شما معتبر نمی باشد";
                return View(register);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(register);
                switch (result)
                {
                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "شماره تلفن وارد شده قبلا در سیستم ثبت شده است";
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت نام با موفقیت انجام شد";
                        return RedirectToAction("ActiveAccount", "Account", new {mobile = register.PhoneNumber});

                }
            }
            return View(register);
        }
        #endregion

        #region Login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("Login"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            #region Captcha
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Token))
            {
                TempData[ErrorMessage] = "کد کپچا شما معتبر نمی باشد";
                return View(login);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(login);
                switch (result)
                {
                    case LoginUserResult.NotFound:
                        TempData[WarningMessage] = "کاربر یافت نشد";
                        break;
                    case LoginUserResult.NotActive:
                        TempData[WarningMessage] = "حساب کاربری فعال نمی باشد";
                        break;
                    case LoginUserResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما توسط واحد پشتیبانی مسدود شده است";
                        TempData[InfoMessage] = "جهت اطلاع بیشتر به بخش تماس با ما مراجعه کنید";
                        break;
                    case LoginUserResult.Success:
                        var user = await _userService.GetUserByPhoneNumber(login.PhoneNumber);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.PhoneNumber),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties()
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principle, properties);
                        TempData[SuccessMessage] = "ورود با موفقیت انجام شد";
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(login);
        }
        #endregion

        #region Logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData[InfoMessage] = "شما از حساب کاربری خود خارج شدید";
            return Redirect("/");
        }
        #endregion

        #region Activate Account
        [HttpGet("Active-Account/{mobile}")]
        public async Task<IActionResult> ActiveAccount(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activeAccount = new ActiveAccountViewModel { PhoneNumber = mobile };

            return View(activeAccount);
        }
        [HttpPost("Active-Account/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveAccount(ActiveAccountViewModel activeAccount)
        {
            
            #region Captcha
            if (!await _captchaValidator.IsCaptchaPassedAsync(activeAccount.Token))
            {
                TempData[ErrorMessage] = "کد کپچا شما معتبر نمی باشد";
                return View(activeAccount);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.ActiveAccount(activeAccount);
                switch (result)
                {
                    case ActiveAccountResult.Error:
                        TempData[ErrorMessage] = "عملیات فعالسازی حساب کاربری با شکست مواجه شد";
                        break;
                    case ActiveAccountResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ActiveAccountResult.Success:
                        TempData[SuccessMessage] = "عملیات فعالسازی حساب کاربری با موفقیت انجام شد";
                        TempData[InfoMessage] = "لطفا جهت ادامه فرایند وارد حساب کاربری خود شوید";
                        return RedirectToAction(nameof(Login));
                }


            }

            return View(activeAccount);
        }
        #endregion


    }
}
