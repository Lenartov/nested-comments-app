public interface ICaptchaService
{
    bool ValidateCaptcha(HttpContext context, string userInput);
}