using System.Collections.Generic;

namespace NaucnaCentralaBackend.Models.API
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Vocation { get; set; }
        public bool IsReviewer { get; set; }
        public List<ScientificArea> ScientificAreas { get; set; }
    }
}
