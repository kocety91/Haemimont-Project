namespace Haemimont_Interview.Dtos
{
    public abstract class StudentDto
    {
        public string FullName { get; set; }

        public List<CourseOutputDto> Courses { get; set; }

        public int TotalCredit { get; set; }
    }
}
