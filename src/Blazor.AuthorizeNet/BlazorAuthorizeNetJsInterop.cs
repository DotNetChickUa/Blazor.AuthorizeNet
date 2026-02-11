using Microsoft.JSInterop;

namespace Blazor.AuthorizeNet;

public class BlazorAuthorizeNetJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.AuthorizeNet/blazorAuthorizeNet.js")
        .AsTask());

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async ValueTask InitCommunicator(DotNetObjectReference<AuthorizeNetAcceptHosted> dotNetRef)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("initCommunicator", dotNetRef);
    }

    public async Task OpenPopup(string paymentUrl)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("AuthorizeNetPopup.openPopup", paymentUrl);
        }
        catch
        {
            // ignored
        }
    }

    public async Task Cancel()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("AuthorizeNetPopup.closePopup");
        }
        catch
        {
            // ignored
        }
    }
}