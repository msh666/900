using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _900.Models
{
    public class SubjectDetails
    {
        public SubjectDetails()
        {
            Subject = new Subject();
            CanFollow = false;
            IsFollowed = false;
        }

        public Subject Subject { get; set; }
        public bool CanFollow { get; set; }
        public bool IsFollowed { get; set; }
    }
}
