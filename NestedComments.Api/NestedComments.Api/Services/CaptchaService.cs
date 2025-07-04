namespace NestedComments.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private const string SessionKey = "CaptchaCode";

        public bool ValidateCaptcha(HttpContext context, string userInput)
        {
            Console.WriteLine("Get KEy in valid" + context.Session.GetString(SessionKey));
            var storedCaptcha = context.Session.GetString(SessionKey);
            Console.WriteLine("server " + storedCaptcha);
            Console.WriteLine("bravser " + userInput);
            return !string.IsNullOrEmpty(storedCaptcha) &&
                   string.Equals(storedCaptcha, userInput, StringComparison.OrdinalIgnoreCase);
        }
    }
}
