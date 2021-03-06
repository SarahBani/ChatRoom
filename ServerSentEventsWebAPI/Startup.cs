using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ServerSentEventsWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                //builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                builder.WithOrigins("https://localhost:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //endpoints.MapGet("/stream", async context =>
                //{
                //    var response = context.Response;
                //    response.Headers.Add("connection", "keep-alive");
                //    response.Headers.Add("cach-control", "no-cache");
                //    response.Headers.Add("content-type", "text/event-stream");

                //    while (true)
                //    {
                //        var obj = new { id = 1, name = "sdf" };
                //        await response.Body
                //            .WriteAsync(Encoding.UTF8.GetBytes($"data: {JsonSerializer.Serialize(obj)}\n\n"));

                //        await response.Body.FlushAsync();
                //        await Task.Delay(5 * 1000);
                //    }

                //});
            });
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.ToString().Equals("/sse"))
            //    {
            //        var response = context.Response;
            //        response.Headers.Add("Content-Type", "text/event-stream");

            //        for (var i = 0; true; ++i)
            //        {
            //            // WriteAsync requires `using Microsoft.AspNetCore.Http`
            //            await response
            //                .WriteAsync($"data: Middleware {i} at {DateTime.Now}\r\r");

            //            await response.Body.FlushAsync();
            //            await Task.Delay(5 * 1000);
            //        }
            //    }

            //    await next.Invoke();
            //});
        }
    }
}
