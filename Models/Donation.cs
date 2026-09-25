using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a donation amount.")]
        [Range(10, 1000000, ErrorMessage = "Donation amount must be between 10 and 1,000,000.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR"; // ZAR, USD, EUR

        [Required]
        public string DonationType { get; set; } = "One-Time"; // One-Time, Recurring

        public bool IsAnonymous { get; set; } = false;

        [Required(ErrorMessage = "Donor name is required.")]
        [Display(Name = "Full Name")]
        public string DonorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string DonorEmail { get; set; } = string.Empty;

        [Display(Name = "Tax Reference Number (Optional)")]
        public string? TaxReferenceNumber { get; set; }

        public DateTime DateDonated { get; set; } = DateTime.UtcNow;

        public string CertificateNumber { get; set; } = string.Empty;
    }
}