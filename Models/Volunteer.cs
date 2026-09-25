using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please state your primary skills.")]
        [Display(Name = "Skills / Expertise")]
        public string Skills { get; set; } = string.Empty; // e.g. Medical, Logistics, General Aid

        [Required(ErrorMessage = "Please select your availability.")]
        public string Availability { get; set; } = string.Empty; // Full-Time, Part-Time, Weekends

        public DateTime DateApplied { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}