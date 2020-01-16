using System.Collections.Generic;

namespace NaucnaCentralaBackend.Models.API
{
    public class InsertEditorsAndReviewersForMagazineModel
    {
        public List<Reviewer> Reviewers { get; set; }
        public List<Editor> Editors { get; set; }

        public class Reviewer
        {
            public string ReviewerName { get; set; }
        }

        public class Editor
        {
            public string EditorName { get; set; }
        }
    }
}
