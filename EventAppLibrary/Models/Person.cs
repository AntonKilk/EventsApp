using System.ComponentModel.DataAnnotations;

namespace EventAppLibrary.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(11)]
        public required string PersonalCode { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(1500)]
        public string? AdditionalInformation { get; set; }
    }

}
public enum PaymentMethod
{
    BankTransfer,
    Cash
}