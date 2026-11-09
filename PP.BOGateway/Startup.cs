using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
public class Startup
{

    public Startup(IConfiguration config)
    {
        Configuration = config;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvcCore().AddApiExplorer();
        services.AddSwaggerGen();
        services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
        services.AddSwaggerForOcelot(Configuration);
        services.AddHealthChecks();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseStaticFiles();
        if (env.IsDevelopment() || env.EnvironmentName.Equals("k8s"))
        {
            app.UseSwagger();
            app.UseSwaggerForOcelotUI();
        }
        app.UseHealthChecks("/health");
        await app.UseOcelot();

    }
}
