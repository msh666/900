using System.Collections.Generic;

namespace _900.Models
{
    public partial class Professor
    {
        public Professor()
        {
            SubjectProfessor = new HashSet<SubjectProfessor>();
        }

        public int IdProfessor { get; set; }
        public string Professor1 { get; set; }

        public virtual ICollection<SubjectProfessor> SubjectProfessor { get; set; }
    }
}
