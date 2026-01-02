using Blazored.Toast;
using Core.Services;
using crud_with_ai_ui;
using crud_with_ai_ui.Features.Chat.Services;
using crud_with_ai_ui.Features.Products.ProductDetail.Services;
using crud_with_ai_ui.Features.Products.ProductList.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions<ApiClientOptions>()
    .Bind(builder.Configuration.GetSection("ApiClientOptions"));

builder.Services.AddBlazoredToast();

builder.Services.AddHttpClient<ConversationsApiService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiClientOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.AddHttpClient<ProductsApiService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiClientOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.AddScoped<ChatPageService>();
builder.Services.AddScoped<ProductsPageService>();
builder.Services.AddScoped<ProductDetailsPageService>();

await builder.Build().RunAsync();
