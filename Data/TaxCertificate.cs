namespace GiftOfTheGivers.Data;

public class TaxCertificate
{
    public string CertificateNumber { get; set; } = string.Empty;
    public string DonorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ZAR";
    public DateTime DateIssuedUtc { get; set; }
    public string Message { get; set; } = string.Empty;
}
