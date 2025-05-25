using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Riviera.ZarinPal.V4.Models;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Wallet;
using Shop.Presentation.Extensions;

namespace Shop.Presentation.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IConfiguration _configuration;
        private readonly Riviera.ZarinPal.V4.ZarinPalService _zarinpalService; 
        public AccountController(IUserService userService, IWalletService walletService, IConfiguration configuration, Riviera.ZarinPal.V4.ZarinPalService zarinpalService)
        {
            _userService = userService;
            _walletService = walletService;
            _configuration = configuration;
            _zarinpalService = zarinpalService;
        }
        #endregion

        #region Edit User Profile 

        [HttpGet("Edit-User-Profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userService.GetUserEditProfileById(User.GetUserId());
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost("Edit-User-Profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserProfile, IFormFile userAvatar)
        {
            var result = await _userService.EditUserProfile(User.GetUserId(), userAvatar, editUserProfile);
            switch (result)
            {
                case EditUserProfileRerult.NotFound:
                    TempData[ErrorMessage] = "کاربری با مشخصات زیر یافت نشد";
                    break;
                case EditUserProfileRerult.Success:
                    TempData[SuccessMessage] = "عملیات ویرایش اطلاعات کاربری با موفقیت انجام شد";
                    return RedirectToAction("EditUserProfile");


            }
            return View(result);
        }
        #endregion

        #region Change The Password
        [HttpGet("Change-Password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("Change-Password"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangeUserPassword(User.GetUserId() , changePassword);
                switch (result)
                {
                    case ChangePasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ChangePasswordResult.EqualPassword:
                        TempData[InfoMessage] = "لطفا از کلمه عبور جدیدی استفاده کنید";
                        ModelState.AddModelError("NewPassword", "لطفا از کلمه عبور جدیدی استفاده کنید");
                        break;
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "عملیات تغییر کلمه عبور با موفقیت انجام شد";
                        TempData[InfoMessage] = "جهت تکمیل عملیات تغییر کلمه عبور مجددا وارد حساب خور شوید";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login","Account" , new {area = ""});

                        

                }
            }

            return View();
        }
        #endregion

        #region Charge Wallet
        [HttpGet("Charge-Wallet")]
        public async Task<IActionResult> ChargeWallet()
        {
            //to do: show transaction for user
            return View();
        }

        [HttpPost("Charge-Wallet"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel chargeWallet)
        {

            if (ModelState.IsValid)
            {
                var walletId = await _walletService.ChargeWallet(User.GetUserId(), chargeWallet, $"شارژ به مبلغ {chargeWallet.Amount}");

                #region Payment
                var payment = new NewPayment
                {
                    Amount = chargeWallet.Amount,
                    Description = "This is a test payment",
                    CallbackUri = new Uri(_configuration.GetSection("DefaultUrl")["Host"] + "/user/online-payment/" + walletId),
                    Currency = "IRR",
                };

                var result = await _zarinpalService.RequestPaymentAsync(payment);

                if (result?.Data?.PaymentUri != null && result.Data.Code == 100)
                {
                    return Redirect(result.Data.PaymentUri.AbsoluteUri);
                }
                else
                {
                    TempData[ErrorMessage] = $"Failed, Error {result.Data.Code}: {result.Error.Message}";
                    return View();
                }
                
                #endregion
            }
            return View(chargeWallet);
        }
        #endregion

        #region Online Payment
        [HttpGet("Online-Payment/{id}")]
        public async Task<IActionResult> OnlinePayment(long id)
        {
            var wallet = await _walletService.GetUserWalletById(id);
            if (!Request.Query.TryGetValue("Status", out var status) ||
                !Request.Query.TryGetValue("Authority", out var authority))
            {
                TempData[ErrorMessage] = "Bad Request";
                return View();
            }

            if (_zarinpalService.IsStatusValid(status) == false)
            {
                TempData[ErrorMessage] = "Failed";
                return View();
               
            }

            var amount = wallet.Amount;
            var result = await _zarinpalService.VerifyPaymentAsync(amount, authority);

            if (result?.Data != null && (result.Data.Code == 100 || result.Data.Code == 101))
            {
                ViewBag.RefId = result.Data.RefId;
                ViewBag.Success = true;
                await _walletService.UpdateWalletForCharge(wallet);
                TempData[SuccessMessage] = "پرداخت با موفقیت انجام شد";
                return View();
            }
            TempData[ErrorMessage] = $"Failed, Error {result.Data.Code}: {result.Error.Message}";
            return View();
        }

        #endregion

        #region User Wallet
        [HttpGet("User-Wallet")]
        public async Task<IActionResult> UserWallet(FilterWalletViewModel filter)
        {
            filter.UserId = User.GetUserId();
            return View(await _walletService.FilterWallet(filter));
        }
        #endregion
    }
}
