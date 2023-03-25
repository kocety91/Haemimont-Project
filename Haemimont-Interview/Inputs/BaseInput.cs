using Haemimont_Interview.Dtos;

namespace Haemimont_Interview.Inputs
{
    public abstract class BaseInput
    {
        public abstract IEnumerable<StudetOutputDto> GetStudentsData(int minCredits, DateTime parsedStartDate, DateTime parsedEndtDate, List<string> studentPINs);
    }
}
