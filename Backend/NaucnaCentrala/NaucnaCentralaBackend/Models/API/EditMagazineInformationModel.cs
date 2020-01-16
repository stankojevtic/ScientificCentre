using System.Collections.Generic;

namespace NaucnaCentralaBackend.Models.API
{
    public class EditMagazineInformationModel
    {
        public string ISSN { get; set; }
        public bool IsOpenAccess { get; set; }
        public string Name { get; set; }
        public List<ScientificArea> ScientificAreas { get; set; }
    }
}
