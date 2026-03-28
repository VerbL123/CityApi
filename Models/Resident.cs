using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirtsApi.Models
{
    public class Resident : IEntityTypeConfiguration<Resident>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PersonalCode { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;

        public void Configure(EntityTypeBuilder<Resident> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
