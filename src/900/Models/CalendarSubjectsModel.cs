using System;
using System.Collections.Generic;

namespace _900.Models
{
    public class CalendarSubjectsModel
    {
        public IEnumerable<CalendarSubjectModel> Subjects { get; set; }
    }
    public class CalendarSubjectModel
    {
        public int IdSubject { get; set; }
        public DateTime Date { get; set; }
    }

    public class SearchResultModel
    {
        public IEnumerable<SearchResultSingleModel> Results { get; set; }
    }
    public class SearchResultSingleModel
    {
        public int IdSubject { get; set; }
        public string SubjectShort { get; set; }
        public string SubjectLong { get; set; }

        public IEnumerable<SearchLectorModel> Lectors { get; set; }

        public int IdGroup { get; set; }
        public string GroupName { get; set; }
    }

    public class SearchLectorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
