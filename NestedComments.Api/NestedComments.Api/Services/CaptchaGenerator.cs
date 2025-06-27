using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;
using System.Text;

public static class CaptchaGenerator
{
    public static string GenerateCaptchaCode(int length = 6)
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

    public static byte[] GenerateCaptchaImage(string captchaCode)
    {
        int width = 200;
        int height = 60;

        using var image = new Image<Rgba32>(width, height, Color.White);

        var font = SystemFonts.CreateFont("Arial", 36, FontStyle.Bold);

        image.Mutate(ctx =>
        {
            ctx.DrawText(captchaCode, font, Color.Black, new PointF(20, 10));
            // додати шум/лінії тут якщо потрібно
        });

        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        return ms.ToArray();
    }
}
