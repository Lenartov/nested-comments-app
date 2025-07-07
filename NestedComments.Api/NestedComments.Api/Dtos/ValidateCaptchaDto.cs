namespace NestedComments.Api.Dtos
{
    public class ValidateCaptchaDto
    {
        public ValidateCaptchaDto(string userInput, string captchaToken)
        {
            UserInput = userInput;
            CaptchaToken = captchaToken;
        }

        public string UserInput { get; set; }
        public string CaptchaToken { get; set; }
    }
}
