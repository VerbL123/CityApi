using Microsoft.EntityFrameworkCore;

namespace CityApi.Dtos
{
    public class UserDtos
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Access { get; set; }
    }
}
