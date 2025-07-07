using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Services;
using NestedComments.Api.Settings;

namespace NestedComments.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<CaptchaSettings>(builder.Configuration.GetSection("CaptchaSettings"));
            builder.Services.AddScoped<ICaptchaService, CaptchaService>();
            builder.Services.AddScoped<ICommentSanitizer, CommentSanitizer>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAngularApp");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
