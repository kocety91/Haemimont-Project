namespace Haemimont_Interview.Dtos
{
    public class StudentInputDto : StudentDto
    {
        public DateTime ParsedStartDate { get; set; }

        public DateTime ParsedEndtDate { get; set; }

        public List<string> StudentPINs { get; set; }

        public int MinCredits { get; set; }
    }
}
