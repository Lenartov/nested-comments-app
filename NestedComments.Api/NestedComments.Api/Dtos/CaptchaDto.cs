using System.ComponentModel.DataAnnotations;

namespace NestedComments.Api.Dtos
{
    public class CaptchaDto
    {
        [Required(ErrorMessage = "Captcha image is required")]
        public required string CaptchaImageBase64 { get; set; }

        [Required(ErrorMessage = "Captcha token is required")]
        public required string CaptchaToken { get; set; }
    }
}
