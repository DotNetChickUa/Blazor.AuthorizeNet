using Microsoft.AspNetCore.Components;

namespace Blazor.AuthorizeNet;

public partial class AuthorizeNetAcceptHosted(ExampleJsInterop exampleJsInterop) : ComponentBase
{
    [Parameter] public string FormToken { get; set; }
    [Parameter] public bool UseSandbox { get; set; } = true;
    private bool isInitialized = false;

    private string PaymentUrl => UseSandbox
        ? "https://test.authorize.net/payment/payment"
        : "https://accept.authorize.net/payment/payment";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!isInitialized && !string.IsNullOrEmpty(FormToken))
        {
            await exampleJsInterop.initCommunicator();
            isInitialized = true;
        }
    }
}