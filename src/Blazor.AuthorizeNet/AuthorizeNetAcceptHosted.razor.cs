using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.AuthorizeNet;

public partial class AuthorizeNetAcceptHosted(BlazorAuthorizeNetJsInterop blazorAuthorizeNetJsInterop) : ComponentBase, IDisposable
{
    private DotNetObjectReference<AuthorizeNetAcceptHosted>? _dotNetRef;
    private bool _isInitialized;
    private bool _isOpened;

    [Parameter]
    [EditorRequired]
    public required string FormToken { get; set; }

    [Parameter]
    public bool UseSandbox { get; set; } = true;

    [Parameter]
    public EventCallback<TransactionDetail> OnSuccess { get; set; }

    [Parameter]
    public EventCallback<string> OnCancel { get; set; }

    private string PaymentUrl => UseSandbox ? "https://test.authorize.net/payment/payment" : "https://accept.authorize.net/payment/payment";

    public void Dispose()
    {
        _dotNetRef?.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!_isInitialized)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await blazorAuthorizeNetJsInterop.InitCommunicator(_dotNetRef);
            _isInitialized = true;
        }
        
        if (!_isOpened && !string.IsNullOrEmpty(FormToken))
        {
            await blazorAuthorizeNetJsInterop.OpenPopup(PaymentUrl);
            _isOpened = true;
        }
    }

    [JSInvokable]
    public async Task HandleTransactionResponse(TransactionDetail detail)
    {
        await OnSuccess.InvokeAsync(detail);
        _isOpened = false;
    }

    [JSInvokable]
    public async Task HandleCancel(string reason)
    {
        await OnCancel.InvokeAsync(reason);
        await blazorAuthorizeNetJsInterop.Cancel();
        _isOpened = false;
    }
}