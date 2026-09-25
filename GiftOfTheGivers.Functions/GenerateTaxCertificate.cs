using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Functions;

public class GenerateTaxCertificate
{
    private readonly ILogger<GenerateTaxCertificate> _logger;

    public GenerateTaxCertificate(
        ILogger<GenerateTaxCertificate> logger)
    {
        _logger = logger;
    }

    [Function("GenerateTaxCertificate")]
    public async Task<IActionResult> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            "post")] HttpRequest req)
    {
        // GET request allows us to test the Function in a browser.
        if (HttpMethods.IsGet(req.Method))
        {
            return new OkObjectResult(new
            {
                status = "ok",
                function = "GenerateTaxCertificate",
                message =
                    "Gift of the Givers tax-certificate Azure Function is running."
            });
        }

        TaxCertificateRequest? request;

        try
        {
            request =
                await JsonSerializer.DeserializeAsync<TaxCertificateRequest>(
                    req.Body,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid JSON received by GenerateTaxCertificate.");

            return new BadRequestObjectResult(new
            {
                error = "Invalid JSON request."
            });
        }

        // Validate the donation details received from the web application.
        if (request is null ||
            request.DonationId <= 0 ||
            string.IsNullOrWhiteSpace(request.DonorName) ||
            request.Amount <= 0)
        {
            return new BadRequestObjectResult(new
            {
                error =
                    "DonationId, DonorName and a positive Amount are required."
            });
        }

        // Generate a dummy tax-certificate number for the POE.
        var certificateNumber =
            $"GOTG-TAX-{DateTime.UtcNow:yyyyMMdd}-{request.DonationId:D6}";

        var certificate = new TaxCertificateResponse
        {
            CertificateNumber = certificateNumber,
            DonorName = request.DonorName,
            Amount = request.Amount,

            Currency = string.IsNullOrWhiteSpace(request.Currency)
                ? "ZAR"
                : request.Currency.ToUpperInvariant(),

            DateIssuedUtc = DateTime.UtcNow,

            Message =
                "Dummy Section 18A tax certificate generated successfully " +
                "for POE demonstration purposes."
        };

        _logger.LogInformation(
            "Generated certificate {CertificateNumber} for donation {DonationId}.",
            certificateNumber,
            request.DonationId);

        return new OkObjectResult(certificate);
    }
}

public sealed class TaxCertificateRequest
{
    public int DonationId { get; set; }

    public string DonorName { get; set; } = string.Empty;

    public string DonorEmail { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "ZAR";

    public string? TaxReferenceNumber { get; set; }
}

public sealed class TaxCertificateResponse
{
    public string CertificateNumber { get; set; } = string.Empty;

    public string DonorName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "ZAR";

    public DateTime DateIssuedUtc { get; set; }

    public string Message { get; set; } = string.Empty;
}