using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CaptchaController : ControllerBase
{
    [HttpGet]
    public IActionResult GetCaptcha()
    {
        var code = CaptchaGenerator.GenerateCaptchaCode();
        HttpContext.Session.SetString("CaptchaCode", code);
        Console.WriteLine("Set string " + HttpContext.Session.GetString("CaptchaCode"));
        var imageBytes = CaptchaGenerator.GenerateCaptchaImage(code);
        return File(imageBytes, "image/png");
    }
}