using System.Text.Json.Serialization;

namespace Blazor.AuthorizeNet;

public class TransactionDetail
{
    public string? AccountType { get; set; }
    public string? AccountNumber { get; set; }
    public string? TransId { get; set; }
    public int ResponseCode { get; set; }
    public string? Authorization { get; set; }
    public string? MerchantName { get; set; }
    public string? OrderDescription { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DutyAmount { get; set; }
    public string? CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PoNumber { get; set; }
    public string? OrderInvoiceNumber { get; set; }
    
    [JsonConverter(typeof(AuthorizeNetDateTimeConverter))]
    public DateTime DateTime { get; set; }
}