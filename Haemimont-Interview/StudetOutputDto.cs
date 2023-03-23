namespace Haemimont_Interview
{
    public class StudetOutputDto
    {
        public string FullName { get; set; }

        public IEnumerable<string> CourseNames { get; set; }

        public IEnumerable<byte> Times { get; set; }

        public IEnumerable<byte> Credits { get; set; }

        public IEnumerable<string> Instructors { get; set; }

        public int TotalCredit { get; set; }
    }
}
