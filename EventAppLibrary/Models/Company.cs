using System.ComponentModel.DataAnnotations;

namespace EventAppLibrary.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(30)]
        public required string CompanyRegistrationCode { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of participants must be at least 1")]
        public int NumberOfParticipants { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(5000)]
        public string? AdditionalInformation { get; set; }
    }
}
