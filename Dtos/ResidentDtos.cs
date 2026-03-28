using FirtsApi.Models;

namespace CityApi.Dtos
{
    public class ResidentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PersonalCode { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid ApartmentId { get; set; }
    }
}
