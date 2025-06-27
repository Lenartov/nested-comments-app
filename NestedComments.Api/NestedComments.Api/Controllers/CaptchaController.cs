using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CaptchaController : ControllerBase
{
    [HttpGet("captcha")]
    public IActionResult GetCaptcha()
    {
        var code = CaptchaGenerator.GenerateCaptchaCode();
        HttpContext.Session.SetString("CaptchaCode", code);

        var imageBytes = CaptchaGenerator.GenerateCaptchaImage(code);
        return File(imageBytes, "image/png");
    }
}