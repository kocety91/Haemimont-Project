using System;
using System.Collections.Generic;

namespace Haemimont_Interview.Models
{
    public  class Course
    {
        public Course()
        {
            StudentsCoursesXrefs = new HashSet<StudentsCoursesXref>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int InstructorId { get; set; }
        public byte TotalTime { get; set; }
        public byte Credit { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<StudentsCoursesXref> StudentsCoursesXrefs { get; set; }
    }
}
