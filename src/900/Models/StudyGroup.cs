using System.Collections.Generic;

namespace _900.Models
{
    public partial class StudyGroup
    {
        public StudyGroup()
        {
            Subject = new HashSet<Subject>();
        }

        public int IdGroup { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<Subject> Subject { get; set; }
    }
}
