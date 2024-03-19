using System.Reflection;
using FoodRecognition.Core.Services.Dish;
using FoodRecognition.Core.Services.Dish.Impl;
using FoodRecognition.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FoodRecognition.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var str = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        
        ConfigureSwagger(services);
        services.AddCors();
        services.AddScoped<IDishService, DishService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddControllersWithViews();
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext databaseContext)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        databaseContext.Database.EnsureCreated();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xpath = Path.Combine(AppContext.BaseDirectory, file);
            s.IncludeXmlComments(xpath);
            s.IncludeXmlComments(xpath);
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodRecognition", Version = "v1" });
          
            var currentAssembly = Assembly.GetExecutingAssembly();
            var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location)!, $"{a.Name}.xml"))
                .Where(File.Exists).ToArray();
            Array.ForEach(xmlDocs, d => { s.IncludeXmlComments(d); });
        });
    }
}