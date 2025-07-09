using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Extensions;
using NestedComments.Api.Hubs;

namespace NestedComments.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAppSwagger(builder.Environment);
            builder.Services.AddAppSignalR();
            builder.Services.AddCorsPolicy(builder.Configuration);
            builder.Services.AddAppServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAngularApp");
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<CommentHub>("/commentHub");

            app.Run();
        }
    }
}
