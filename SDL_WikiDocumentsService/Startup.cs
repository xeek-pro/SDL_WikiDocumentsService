using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SDL_WikiDocumentLibrary;
using SDL_WikiDocumentLibrary.Downloaders;

namespace SDL_WikiDocumentsService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup configuration:
            services
                .Configure<SdlWikiDownloaderSettings>(Configuration.GetSection("Downloader"))
                .Configure<SdlWikiRepoSettings>(Configuration.GetSection("Repository"))
                .Configure<SdlWikiDocumentSearchSettings>(Configuration.GetSection("DocumentSearch"));

            // Services:
            services
                .AddSingleton<ISdlWikiDownloader, SdlWikiGitHubDownloader>()
                .AddSingleton<IWikiRepository, SdlWikiRepository>()
                .AddSingleton<ISdlWikiDocumentSearch, SdlWikiDocumentSearch>()
                .AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SDL_WikiDocumentsService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SDL_WikiDocumentsService v1"));
            }

            app
                //.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
