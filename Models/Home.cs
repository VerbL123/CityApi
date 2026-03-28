using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace FirtsApi.Models
{
    public class Home : IEntityTypeConfiguration<Home>
    {
        public Guid Id { get; set; }
        public string HouseNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostIndex { get; set; } = null!;
        public ICollection<Apartment> Apartments { get; set; } = null!;

        public void Configure(EntityTypeBuilder<Home> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(x => x.Apartments)
                .WithOne(x => x.Home)
                .HasForeignKey(x => x.HomeId);
        }
    }
}
