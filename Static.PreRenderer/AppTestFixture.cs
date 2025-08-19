namespace Static.PreRenderer;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class AppTestFixture : WebApplicationFactory<Server.Program>
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = base.CreateHostBuilder();
        builder.UseEnvironment(Environments.Production);

        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseStaticWebAssets();
            webHostBuilder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_ => CreateDefaultClient());
            });
        });
        return builder;
    }
}
