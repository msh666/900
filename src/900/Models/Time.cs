using System.Collections.Generic;

namespace _900.Models
{
    public partial class Time
    {
        public Time()
        {
            Schedule = new HashSet<Schedule>();
        }

        public int IdTime { get; set; }
        public string Time1 { get; set; }

        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
