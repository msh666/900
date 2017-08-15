using System;
using System.Collections.Generic;

namespace _900.Models
{
    public class ScheduleGridModel
    {
        public User User { get; set; }
        public DateTime StartOfWeek { get; set; }
        public IEnumerable<Time> TimeSlots { get; set; }
        public IEnumerable<Schedule> Schedule { get; set; }
        public IEnumerable<StudyGroup> StudyGroups { get; set; }
    }
}
