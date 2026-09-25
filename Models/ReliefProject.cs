using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty; // Flood Relief, Medical, Food Security, Drought

        [Range(0, double.MaxValue, ErrorMessage = "Target amount must be zero or greater.")]
        public decimal TargetAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Current amount must be zero or greater.")]
        public decimal CurrentAmount { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}