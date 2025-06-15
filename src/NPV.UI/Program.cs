using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NPV.UI.Services;

namespace NPV.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //this is the base url for NPV.Api.
            //The value is derived from: NPV.Api => Properties => launchSettings.json => profiles => https
            string apiBaseUrl = "https://localhost:7209";
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
            builder.Services.AddScoped<IApiService, ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
