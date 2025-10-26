using System.Reflection;
using Microsoft.OpenApi.Models;
using RatingService.Core.Interfaces;
using RatingService.Database.Repositories.Extensions;
using RatingService.Services.RatingService.Extensions;
using RatingService.Database.Context.Extensions;
using RatingService.Server.Attributes;
using RatingService.Services.DataInitializer;

namespace RatingService.Server;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RatingService.Server", Version = "v1" });
            
            c.OperationFilter<SwaggerUserHeaderAttribute>();
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });
        services.AddSwaggerGenNewtonsoftSupport();
        
        services.AddDbContext(Configuration);
        services.AddRepositories();
        services.AddRatingService();
        services.AddScoped<IDataInitializer, DataInitializer>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "/api/v1/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/api/v1/swagger/v1/swagger.json", "RatingService.Server.Http v1");
            c.RoutePrefix = "api/v1/swagger";
        });
        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}