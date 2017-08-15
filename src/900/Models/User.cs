using System.Collections.Generic;

namespace _900.Models
{
    public partial class User
    {
        public User()
        {
            UserSubject = new HashSet<UserSubject>();
        }

        public int TelegramId { get; set; }
        public string TelegramName { get; set; }

        public virtual ICollection<UserSubject> UserSubject { get; set; }
    }
}
