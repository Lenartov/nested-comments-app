using NestedComments.Api.Dtos;

public interface ICaptchaService
{
    bool ValidateCaptcha(string userInput, string captchaToken);
    CaptchaDto GenerateCaptchaDto();

}