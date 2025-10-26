using System.Reflection;
using LibraryService.Core.Interfaces;
using Microsoft.OpenApi.Models;
using LibraryService.Database.Repositories.Extensions;
using LibraryService.Services.LibraryService.Extensions;
using LibraryService.Database.Context.Extensions;
using LibraryService.Services.DataInitializer;

namespace LibraryService.Server;

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryService.Server", Version = "v1" });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });
        services.AddSwaggerGenNewtonsoftSupport();
        
        services.AddDbContext(Configuration);
        services.AddRepositories();
        services.AddLibraryService();
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
            c.SwaggerEndpoint("/api/v1/swagger/v1/swagger.json", "LibraryService.Server.Http v1");
            c.RoutePrefix = "api/v1/swagger";
        });
        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}