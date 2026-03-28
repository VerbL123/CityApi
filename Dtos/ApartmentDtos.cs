namespace CityApi.Dtos
{
    public class ApartmentDto
    {
        public Guid Id { get; set; }
        public byte ApartmentNumber { get; set; }
        public byte Floor { get; set; }
        public byte RoomsQuantity { get; set; }
        public short ResidentsQuantity { get; set; }
        public decimal FullArea { get; set; }
        public decimal LivingArea { get; set; }
        public Guid HomeId { get; set; }
    }
}
