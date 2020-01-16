using System.ComponentModel.DataAnnotations.Schema;

namespace NaucnaCentralaBackend.Models.Database
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public bool IsActive { get; set; }
        public string Country { get; set; }
        public string Vocation { get; set; }
        public string City { get; set; }
        public bool IsReviewer { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool AdminConfirmed { get; set; }
        public string ScientificAreas { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
