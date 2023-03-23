using System;
using System.Collections.Generic;

namespace Haemimont_Interview.Models
{
    public class Student
    {
        public Student()
        {
            StudentsCoursesXrefs = new HashSet<StudentsCoursesXref>();
        }

        public string Pin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual ICollection<StudentsCoursesXref> StudentsCoursesXrefs { get; set; }
    }
}
