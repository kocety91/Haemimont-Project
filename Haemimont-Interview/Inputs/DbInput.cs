using Haemimont_Interview.Models;

namespace Haemimont_Interview.Inputs
{
    public class DbInput : BaseInput
    {
        public DbInput(CourseraContext db)
        {
            this.Db = db;
        }

        public CourseraContext Db { get; set; }

        public override IEnumerable<StudetOutputDto> GetStudentsData(int minCredits, DateTime parsedStartDate, DateTime parsedEndtDate, List<string> studentPINs)
        {
            IQueryable<Student> studentsOutput = null;


            if (studentPINs.Count > 0)
            {
                studentsOutput = Db.Students
                        .SelectMany(x => x.StudentsCoursesXrefs)
                        .Where(x => studentPINs.Contains(x.StudentPin))
                        .Select(x => x.StudentPinNavigation)
                        .Distinct();
            }

            if (studentsOutput == null)
            {
                studentsOutput = Db.Students
                       .Where(x => x.StudentsCoursesXrefs.Sum(c => c.Course.Credit) > minCredits).AsQueryable();

                studentsOutput = studentsOutput
                                           .SelectMany(x => x.StudentsCoursesXrefs)
                                           .Where(x => x.CompletionDate.Value >= parsedStartDate && x.CompletionDate.Value <= parsedEndtDate)
                                           .Select(x => x.StudentPinNavigation)
                                           .Distinct();
            }

            var output = studentsOutput.
                             Select(x => new StudetOutputDto
                             {
                                 FullName = x.FirstName + " " + x.LastName,
                                 Times = x.StudentsCoursesXrefs.Select(t => t.Course.TotalTime),
                                 Credits = x.StudentsCoursesXrefs.Select(c => c.Course.Credit),
                                 TotalCredit = x.StudentsCoursesXrefs.Sum(c => c.Course.Credit),
                                 Courses = x.StudentsCoursesXrefs.Select(c => c.Course).Select(c => new CourseOutputDto()
                                 {
                                     Credit = c.Credit,
                                     Name = c.Name,
                                     TotalTime = c.TotalTime,
                                     InstructorName = c.Instructor.FirstName + " " + c.Instructor.LastName,
                                 })
                             }).ToArray();
            return output;
        }
    }
}
