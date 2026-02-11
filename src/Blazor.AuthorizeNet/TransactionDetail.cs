using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blazor.AuthorizeNet;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateTimeFormat = "M/d/yyyy h:mm:ss tt";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
            return default;

        return DateTime.ParseExact(dateString, DateTimeFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
    }
}

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
    
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime DateTime { get; set; }
}