namespace NestedComments.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private const string SessionKey = "CaptchaCode";

        public bool ValidateCaptcha(HttpContext context, string userInput)
        {
            var storedCaptcha = context.Session.GetString(SessionKey);
            return !string.IsNullOrEmpty(storedCaptcha) &&
                   string.Equals(storedCaptcha, userInput, StringComparison.OrdinalIgnoreCase);
        }
    }
}
