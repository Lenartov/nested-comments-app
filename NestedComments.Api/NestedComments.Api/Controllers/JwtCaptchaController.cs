using Microsoft.AspNetCore.Mvc;
using NestedComments.Api.Dtos;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtCaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;

        public JwtCaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        [HttpGet("generate")]
        public ActionResult<CaptchaDto> GetCaptcha()
        {
            var captcha = _captchaService.GenerateCaptchaDto();
            return Ok(captcha);
        }

        [HttpPost("validate")]
        public ActionResult ValidateCaptcha([FromBody] ValidateCaptchaDto request)
        {
            if (_captchaService.ValidateCaptcha(request.UserInput,request.CaptchaToken))
                return Ok();
            else
                return BadRequest("Invalid captcha");
        }
    }
}
