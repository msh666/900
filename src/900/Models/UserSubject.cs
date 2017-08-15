namespace _900.Models
{
    public partial class UserSubject
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public int IdSubject { get; set; }

        public virtual Subject IdSubjectNavigation { get; set; }
        public virtual User Telegram { get; set; }
    }
}
