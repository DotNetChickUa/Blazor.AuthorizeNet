namespace Blazor.AuthorizeNet;

public class TransactionDetail
{
    public string? AccountType { get; set; }
    public string? AccountNumber { get; set; }
    public string? TransId { get; set; }
    public string? ResponseCode { get; set; }
    public string? Authorization { get; set; }
    public string? MerchantName { get; set; }
    public string? OrderDescription { get; set; }
    public string? TaxAmount { get; set; }
    public string? ShippingAmount { get; set; }
    public string? DutyAmount { get; set; }
    public string? CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PoNumber { get; set; }
    public string? OrderInvoiceNumber { get; set; }
    public string? DateTime { get; set; }
}