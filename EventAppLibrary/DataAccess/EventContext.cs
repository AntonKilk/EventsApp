using EventAppLibrary.Models;
using Microsoft.EntityFrameworkCore;


namespace EventAppLibrary.DataAccess
{

    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Company> Companies { get; set; }

        public EventContext(DbContextOptions<EventContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static void SeedData(EventContext context)
        {
            if (context.Events.Any())
            {
                return;
            }

            var events = new List<Event>
            {
                new() {
                    Name = "Tech Conference",
                    DateAndTime = new DateTime(2024, 4, 15, 10, 0, 0),
                    EventPlace = "Conference Center",
                    AdditionalInformation = "A tech conference for software developers.",
                    RegisteredPersons =
                    [
                        new() { FirstName = "John", LastName = "Doe", PersonalCode = "1234567890", PaymentMethod = PaymentMethod.BankTransfer },
                        new() { FirstName = "Jane", LastName = "Smith", PersonalCode = "0987654321", PaymentMethod = PaymentMethod.Cash }
                    ],
                    RegisteredCompanies =
                    [
                        new() { Name = "Tech Innovations Ltd.", CompanyRegistrationCode = "987654321", NumberOfParticipants = 5, PaymentMethod = PaymentMethod.BankTransfer }
                    ]
                },
                new () {
                    Name = "Tech Conference II",
                    DateAndTime = new DateTime(2024, 5, 20, 10, 0, 0),
                    EventPlace = "Innovation Hub",
                    AdditionalInformation = "The sequel to our tech conference, focusing on future technologies.",
                    RegisteredPersons = new List<Person>
                    {
                        new Person { FirstName = "Alice", LastName = "Johnson", PersonalCode = "2345678901", PaymentMethod = PaymentMethod.BankTransfer },
                        new Person { FirstName = "Bob", LastName = "Marley", PersonalCode = "3456789012", PaymentMethod = PaymentMethod.Cash }
                    },
                    RegisteredCompanies = new List<Company>
                    {
                        new Company { Name = "Future Tech Ltd.", CompanyRegistrationCode = "876543219", NumberOfParticipants = 3, PaymentMethod = PaymentMethod.BankTransfer }
                    }
                }
              };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}


