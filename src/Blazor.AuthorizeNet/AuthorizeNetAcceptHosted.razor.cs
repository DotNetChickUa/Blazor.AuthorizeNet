using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.AuthorizeNet;

public partial class AuthorizeNetAcceptHosted(ExampleJsInterop exampleJsInterop) : ComponentBase, IDisposable
{
    [Parameter] public string FormToken { get; set; }
    [Parameter] public bool UseSandbox { get; set; } = true;
    private bool isInitialized = false;
    // New event callbacks
    [Parameter] public EventCallback<string> OnSuccess { get; set; } 
    [Parameter] public EventCallback<string> OnCancel { get; set; } 
    private DotNetObjectReference<AuthorizeNetAcceptHosted>? _dotNetRef;

    private string PaymentUrl => UseSandbox
        ? "https://test.authorize.net/payment/payment"
        : "https://accept.authorize.net/payment/payment";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!isInitialized && !string.IsNullOrEmpty(FormToken))
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await exampleJsInterop.initCommunicator(_dotNetRef);
            isInitialized = true;
        }
    }

    [JSInvokable]
    public async Task HandleTransactionResponse(string responseJson)
    {
        await OnSuccess.InvokeAsync(responseJson);
    }

    [JSInvokable]
    public async Task HandleCancel(string reason)
    {
        await OnCancel.InvokeAsync(reason);
    }

    public void Dispose()
    {
        _dotNetRef?.Dispose();
    }
}