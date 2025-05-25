using Kavenegar;
using Shop.Application.Services.Interfaces;

namespace Shop.Application.Services.Implementations
{
    public class SmsService:ISmsService
    {
        private readonly string apiKey = "2F2F47694B3855597146525A76765A67354E3058474C514D4A52457875614C4C7634723637315A4B642F383D";

        public async Task SendVerificationCode(string phoneNumber, string activeCode)
        {
            KavenegarApi api = new KavenegarApi(apiKey);
            await api.VerifyLookup(phoneNumber, activeCode, "shop-foryou-verify");
        }
    }
}
