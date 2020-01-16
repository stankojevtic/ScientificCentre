using System.ComponentModel.DataAnnotations.Schema;

namespace NaucnaCentralaBackend.Models.Database
{
    [Table("Magazine")]
    public class Magazine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Editors { get; set; }
        public bool IsActive { get; set; }
        public string ChiefEditor { get; set; }
        public string ScientificAreas { get; set; }
        public string Reviewers { get; set; }
        public bool IsOpenAccess { get; set; }
        public bool AdminReviewed { get; set; }
        public bool DataValid { get; set; }
        public string ISSN { get; set; }
    }
}
