using AutoMapper;
using CityApi.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirtsApi.Models
{
    public class Apartment : IEntityTypeConfiguration<Apartment>
    {
        public Guid Id { get; set; }
        public int ApartmentNumber { get; set; }
        public int Floor { get; set; }
        public int RoomsQuantity { get; set; }
        public int ResidentsQuantity { get; set; }
        public double FullArea { get; set; }
        public double LivingArea { get; set; }
        public Guid HomeId { get; set; }
        public Home Home { get; set; } = null!;
        public ICollection<Resident> Residents { get; set; } = null!;

        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(x => x.Residents)
                .WithOne(x => x.Apartment)
                .HasForeignKey(x => x.ApartmentId);
        }
    }
}
