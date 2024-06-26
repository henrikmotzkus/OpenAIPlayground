using System.Security.Claims;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenAIClient;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using OpenAIClient.Layout;
using Microsoft.Authentication.WebAssembly.Msal;
using Microsoft.Extensions.Http;
using Microsoft.Graph;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://search.azure.com/.default");
});


builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("WebAPI", 
    client => client.BaseAddress = new Uri("https://henrik-test-1.search.windows.net/"))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>()
.CreateClient("WebAPI"));

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Index1", policy => 
        policy.RequireClaim("roles", "Index1.Read"));
    options.AddPolicy("Index2", policy => 
        policy.RequireClaim("roles", "Index2.Read"));
});

await builder.Build().RunAsync();
