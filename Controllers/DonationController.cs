using System.Net.Http.Json;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

public class DonationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DonationController> _logger;

    public DonationController(
        ApplicationDbContext context,
        IHttpClientFactory httpClientFactory,
        ILogger<DonationController> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var donations = await _context.Donations
            .OrderByDescending(d => d.DateDonated)
            .ToListAsync();

        return View(donations);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Donation());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Donation donation)
    {
        if (!ModelState.IsValid)
        {
            return View(donation);
        }

        // Save the donation first so SQL Server generates its ID.
        donation.DateDonated = DateTime.UtcNow;

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        try
        {
            // Call the Azure Function.
            var client =
                _httpClientFactory.CreateClient("TaxCertificateFunction");

            var request = new TaxCertificateRequest
            {
                DonationId = donation.Id,
                DonorName = donation.DonorName,
                DonorEmail = donation.DonorEmail,
                Amount = donation.Amount,
                Currency = donation.Currency,
                TaxReferenceNumber = donation.TaxReferenceNumber
            };

            var response = await client.PostAsJsonAsync(
                "GenerateTaxCertificate",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"Azure Function returned {(int)response.StatusCode} " +
                    $"{response.StatusCode}. Response: {errorBody}");
            }

            var certificate =
                await response.Content.ReadFromJsonAsync<TaxCertificate>();

            if (certificate is null ||
                string.IsNullOrWhiteSpace(certificate.CertificateNumber))
            {
                throw new InvalidOperationException(
                    "The Azure Function returned an invalid certificate response.");
            }

            // Store the certificate number against the donation.
            donation.CertificateNumber = certificate.CertificateNumber;

            await _context.SaveChangesAsync();

            TempData["FunctionStatus"] =
                "Tax certificate generated successfully by the Azure Function.";

            return View("Certificate", donation);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unable to generate a tax certificate for donation {DonationId}.",
                donation.Id);

            TempData["FunctionError"] =
                "The donation was saved, but the tax certificate could not be generated. " +
                "Check that GiftOfTheGivers.Functions is running.";

            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Certificate(int id)
    {
        var donation = await _context.Donations.FindAsync(id);

        if (donation is null)
        {
            return NotFound();
        }

        return View(donation);
    }

    private sealed class TaxCertificateRequest
    {
        public int DonationId { get; set; }

        public string DonorName { get; set; } = string.Empty;

        public string DonorEmail { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "ZAR";

        public string? TaxReferenceNumber { get; set; }
    }
}