using System.ComponentModel.DataAnnotations;

namespace EventAppLibrary.Models
{


    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        [Required]
        [StringLength(200)]
        public required string EventPlace { get; set; }

        [StringLength(1000)]
        public string? AdditionalInformation { get; set; }
        public List<Person> RegisteredPersons { get; set; } = new List<Person>();
        public List<Company> RegisteredCompanies { get; set; } = new List<Company>();
    }

}
