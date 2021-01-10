namespace EventHorizon.Platform.Docs
{
    using EventHorizon.Platform.Docs.Localization.Api;
    using EventHorizon.Platform.Docs.Localization.Model;
    using EventHorizon.Platform.Docs.Metadata.Api;
    using EventHorizon.Platform.Docs.Metadata.State;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Collections.Generic;
    using System.Globalization;

    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            services.AddSingleton<PageMetadataRepository, StandardPageMetadataRepository>();

            // ASP.NET Main Services
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // I18n Services
            services
                .AddScoped(typeof(Localizer<>), typeof(StringBasedLocalizer<>))
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .Configure<RequestLocalizationOptions>(
                    opts =>
                    {
                        var supportedCultures = new List<CultureInfo>
                        {
                            // Set Supported Locales
                            new CultureInfo("en-US"),
                        };

                        opts.DefaultRequestCulture = new RequestCulture("en-US");
                        // Formatting numbers, dates, etc.
                        opts.SupportedCultures = supportedCultures;
                        // UI strings that we have localized.
                        opts.SupportedUICultures = supportedCultures;
                    }
                );
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
