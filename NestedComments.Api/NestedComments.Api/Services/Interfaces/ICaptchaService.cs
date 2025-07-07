using NestedComments.Api.Dtos;

namespace NestedComments.Api.Services.Interfaces
{
    public interface ICaptchaService
    {
        bool ValidateCaptcha(string userInput, string captchaToken);
        CaptchaDto GenerateCaptchaDto();

    }
}