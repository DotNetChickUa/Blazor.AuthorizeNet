using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.AuthorizeNet;

public enum AuthorizeNetMode
{
    EmbeddedIFrame,
    IFrameLightbox

}
public partial class AuthorizeNetAcceptHosted(BlazorAuthorizeNetJsInterop blazorAuthorizeNetJsInterop, IJSRuntime jsRuntime) : ComponentBase, IDisposable
{
    private DotNetObjectReference<AuthorizeNetAcceptHosted>? _dotNetRef;
    private bool _isInitialized;
    private bool _isOpened;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    [Parameter]
    [EditorRequired]
    public string? FormToken { get; set; }

    [Parameter]
    [EditorRequired]
    public bool UseSandbox { get; set; } = true;

    [Parameter]
    public EventCallback<TransactionDetail> OnSuccess { get; set; }

    [Parameter]
    public EventCallback<string> OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public AuthorizeNetMode Mode { get; set; }

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
            await blazorAuthorizeNetJsInterop.InitCommunicator(_dotNetRef, Mode);
            _isInitialized = true;
        }

        if (!_isOpened && !string.IsNullOrEmpty(FormToken))
        {
            switch (Mode)
            {
                case AuthorizeNetMode.EmbeddedIFrame:
                    await blazorAuthorizeNetJsInterop.OpenIFrame();
                    break;
                case AuthorizeNetMode.IFrameLightbox:
                    await blazorAuthorizeNetJsInterop.OpenPopup(PaymentUrl);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _isOpened = true;
        }
    }

    [JSInvokable]
    public async Task HandleTransactionResponse(string detail)
    {
        if (OnSuccess.HasDelegate)
        {
            var transactionDetail = JsonSerializer.Deserialize<TransactionDetail>(detail, JsonOptions);
            await OnSuccess.InvokeAsync(transactionDetail);
        }

        _isOpened = false;
    }

    [JSInvokable]
    public async Task HandleCancel(string reason)
    {
        if (OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync(reason);
        }

        await blazorAuthorizeNetJsInterop.Cancel();
        _isOpened = false;
    }
}