namespace _900.Models
{
    public partial class SubjectProfessor
    {
        public int IdSubProf { get; set; }
        public int IdSubject { get; set; }
        public int IdProfessor { get; set; }

        public virtual Professor IdProfessorNavigation { get; set; }
        public virtual Subject IdSubjectNavigation { get; set; }
    }
}
