using Haemimont_Interview.Dtos;
using System.Text.Json;

namespace Haemimont_Interview.Inputs
{
    public class FileInput : BaseInput
    {
        public override IEnumerable<StudetOutputDto> GetStudentsData(int minCredits, DateTime parsedStartDate, DateTime parsedEndtDate, List<string> studentPINs)
        {
            var filePath = Path.GetFullPath
                (Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory,
                "..\\..\\..\\", "file.txt"));

            var studentInputDtos = this.ReadFileData(filePath);

            var studentOutputDtos = studentInputDtos
                .Where(x => x.Courses.Sum(c => c.Credit) > minCredits
                && x.ParsedStartDate >= parsedStartDate && x.ParsedEndtDate <= parsedEndtDate)
                .Select(x=> new StudetOutputDto()
                {
                    FullName = x.FullName,
                    Courses = x.Courses,
                    TotalCredit = x.TotalCredit
                }).ToList();

            return studentOutputDtos;
        }

        private List<StudentInputDto> ReadFileData(string filePath)
        {
            var students = new List<StudentInputDto>();
            using StreamReader streamReader = new StreamReader(filePath);

            string line = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                var dict = line
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Split(":", StringSplitOptions.RemoveEmptyEntries))
                        .ToDictionary(x => x[0], x => x[1]);

                var parsedData = DateTimeParser.ParseDateRange(dict["StartDate"], dict["EndDate"]);


                var studentDto = new StudentInputDto()
                {
                    FullName = dict["FullName"],
                    ParsedStartDate = parsedData.parsedStartDate,
                    ParsedEndtDate = parsedData.parsedEndtDate,
                    MinCredits = int.Parse(dict["MinCredits"]),
                    Courses = new List<CourseOutputDto>()
                    {
                        new CourseOutputDto()
                        {
                            Name = dict["CourseName"],
                            TotalTime = byte.Parse(dict["CourseTime"]),
                            Credit = byte.Parse(dict["CourseCredit"]),
                            InstructorName = dict["InstructorName"],
                        }
                    },
                };

                var currentStudent = students.FirstOrDefault(x => x.FullName == studentDto.FullName);

                if (currentStudent != null)
                {
                    studentDto.Courses.ForEach(x => currentStudent.Courses.Add(x));
                }
                else
                {
                    students.Add(studentDto);
                }

            }

            students.ForEach(x =>
            {
                x.TotalCredit = x.Courses.Sum(s => s.Credit);
            });


            return students;
        }
    }
}
