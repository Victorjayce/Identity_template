using Microsoft.AspNetCore.Identity;

namespace Identity_template.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        // Add any additional properties you need for your user model
    }
}
