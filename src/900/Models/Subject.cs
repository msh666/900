using System.Collections.Generic;

namespace _900.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Schedule = new HashSet<Schedule>();
            SubjectProfessor = new HashSet<SubjectProfessor>();
            UserSubject = new HashSet<UserSubject>();
        }

        public int IdSubject { get; set; }
        public string SubjectFull { get; set; }
        public string SubjectShort { get; set; }
        public int IdGroup { get; set; }

        public virtual ICollection<Schedule> Schedule { get; set; }
        public virtual ICollection<SubjectProfessor> SubjectProfessor { get; set; }
        public virtual ICollection<UserSubject> UserSubject { get; set; }
        public virtual StudyGroup IdGroupNavigation { get; set; }
    }
}
