using System;
using System.Collections.Generic;

namespace Haemimont_Interview.Models
{
    public class Instructor
    {
        public Instructor()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
