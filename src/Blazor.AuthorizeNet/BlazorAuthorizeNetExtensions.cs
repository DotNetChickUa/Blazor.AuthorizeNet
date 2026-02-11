using Microsoft.Extensions.DependencyInjection;

namespace Blazor.AuthorizeNet;

public static class BlazorAuthorizeNetExtensions
{
    public static void AddBlazorAuthorizeNet(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<BlazorAuthorizeNetJsInterop>();
    }
}