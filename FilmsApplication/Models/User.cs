using Microsoft.AspNetCore.Identity;

namespace FilmsApplication
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
