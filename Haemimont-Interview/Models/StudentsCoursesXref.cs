using System;
using System.Collections.Generic;

namespace Haemimont_Interview.Models
{
    public class StudentsCoursesXref
    {
        public string StudentPin { get; set; }
        public int CourseId { get; set; }
        public DateTime? CompletionDate { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student StudentPinNavigation { get; set; }
    }
}
