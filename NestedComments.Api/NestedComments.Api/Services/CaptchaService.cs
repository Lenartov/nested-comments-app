using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NestedComments.Api.Dtos;
using NestedComments.Api.Services.Interfaces;
using NestedComments.Api.Settings;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NestedComments.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly string _secretKey;

        public CaptchaService(IOptions<CaptchaSettings> options)
        {
            _secretKey = options.Value.SecretKey;
        }

        public bool ValidateCaptcha(string userInput, string captchaToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                };

                var principal = tokenHandler.ValidateToken(captchaToken, parameters, out var validatedToken);

                var captchaFromToken = principal.FindFirst("captcha")?.Value;

                return string.Equals(userInput, captchaFromToken, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public CaptchaDto GenerateCaptchaDto()
        {
            var captchaText = GenerateCaptchaCode(6);

            var captchaImageBase64 = GenerateCaptchaImage(captchaText);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim("captcha", captchaText)]),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new CaptchaDto
            {
                CaptchaImageBase64 = Convert.ToBase64String(captchaImageBase64),
                CaptchaToken = tokenString
            };
        }

        private string GenerateCaptchaCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var result = new StringBuilder(length);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }

        private byte[] GenerateCaptchaImage(string captchaCode)
        {
            int width = 200;
            int height = 60;

            using var image = new Image<Rgba32>(width, height, Color.White);

            var font = SystemFonts.CreateFont("Arial", 36, FontStyle.Bold);

            image.Mutate(ctx =>
            {
                ctx.DrawText(captchaCode, font, Color.Black, new PointF(20, 10));
                // add some noise
            });

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms.ToArray();
        }
    }
}
