using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Environment = AuthorizeNet.Environment;

ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = Environment.SANDBOX;

ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
{
    name = "",
    ItemElementName = ItemChoiceType.transactionKey,
    Item = ""
};

var billingAddress = new customerAddressType
{
    firstName = "FirstName",
    lastName = "LastName",
    address = "Address",
    city = "City",
    zip = "Zip",
    country = "Country",
    email = "test@gmail.com",
    state = "State",
    phoneNumber = "PhoneNumber"
};

var transactionRequest = new transactionRequestType
{
    transactionType = nameof(transactionTypeEnum.authCaptureTransaction),
    amount = 20,

    customer = new customerDataType() { email = "test@gmail.com"},
    billTo = billingAddress,
    profile = new customerProfilePaymentType() { customerProfileId = "123" },
};

var hostedPaymentSettings = new settingType[]
{
            new settingType { settingName = "hostedPaymentReturnOptions", settingValue = "{\"showReceipt\": false, \"url\": \"https://127.0.0.1:7242/empty.html\", \"urlText\": \"Continue\"}" },
            new settingType { settingName = "hostedPaymentButtonOptions", settingValue = "{\"text\": \"Pay\"}" },
            new settingType { settingName = "hostedPaymentStyleOptions", settingValue = "{\"bgColor\": \"blue\"}" },
            new settingType { settingName = "hostedPaymentPaymentOptions", settingValue = "{\"cardCodeRequired\": false, \"showCreditCard\": true, \"showBankAccount\": false}" },
            new settingType { settingName = "hostedPaymentSecurityOptions", settingValue = "{\"captcha\": false}" },
            new settingType { settingName = "hostedPaymentShippingAddressOptions", settingValue = "{\"show\": false, \"required\": false}" },
            new settingType { settingName = "hostedPaymentBillingAddressOptions", settingValue = "{\"show\": true, \"required\": false}" },
            new settingType { settingName = "hostedPaymentCustomerOptions", settingValue = "{\"showEmail\": false, \"requiredEmail\": false, \"addPaymentProfile\": true}" },
            new settingType { settingName = "hostedPaymentOrderOptions", settingValue = "{\"show\": true, \"merchantName\": \"ACCU\"}" },
            new settingType { settingName = "hostedPaymentIFrameCommunicatorUrl", settingValue = "{\"url\": \"https://127.0.0.1:7242/iframecommunicator.html\"}" } // TODO: Get from configuration
};

var request = new getHostedPaymentPageRequest
{
    transactionRequest = transactionRequest,
    hostedPaymentSettings = hostedPaymentSettings
};

var controller = new getHostedPaymentPageController(request);
var result = controller.ExecuteWithApiResponse();
Console.WriteLine(result.token);
Console.ReadLine();