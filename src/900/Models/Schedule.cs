namespace _900.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int IdSubject { get; set; }
        public int IdTime { get; set; }
        public long Date { get; set; }
        public int RoomNumber { get; set; }

        public virtual Subject IdSubjectNavigation { get; set; }
        public virtual Time IdTimeNavigation { get; set; }
    }
}
