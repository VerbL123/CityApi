namespace CityApi.Dtos
{
    public class HomeDto
    {
        public Guid Id { get; set; }
        public string HouseNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostIndex { get; set; } = null!;
    }
}
