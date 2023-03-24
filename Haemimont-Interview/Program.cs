using Haemimont_Interview;
using Haemimont_Interview.FilesOutput;
using Haemimont_Interview.Models;
using System.Globalization;

//// minumum required credits
try
{
    var minCredits = int.Parse(Console.ReadLine());

    // Period of time
    var startDateAsString = Console.ReadLine();
    var endDateAsString = Console.ReadLine();

    DateTime parsedStartDate;
    DateTime parsedEndtDate;

    ParseDateInput(startDateAsString, endDateAsString, out parsedStartDate, out parsedEndtDate);

    // output path
    //src/file.csv || src/file.html
    var folderName = Console.ReadLine();


    //output format
    var fileName = Console.ReadLine();
    var dict = new Dictionary<string, Haemimont_Interview.FilesOutput.File>()
    {
        {".csv", new CsvFile()},
        {".html", new HtmlFile()},
    };


    ////Students PIN's or Students with enought credits
    var studentPINs = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();


    var db = new CourseraContext();
    IQueryable<Student> studentsOutput = null;

    var studentDto = GetStudentsData(minCredits, parsedStartDate, parsedEndtDate, studentPINs, db, studentsOutput);

    if (fileName != string.Empty)
    {
        var file = dict[fileName];
        file.WriteToFile(folderName, fileName, studentDto);
    }
    else
    {
        foreach (var item in dict)
        {
            item.Value.WriteToFile(folderName, item.Key, studentDto);
        }
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}



static void ParseDateInput(string startDateAsString, string endDateAsString, out DateTime parsedStartDate, out DateTime parsedEndtDate)
{
    var isStartDateCorrect = DateTime.TryParseExact(
                                     startDateAsString, "yyyy-MM-dd",
                                     CultureInfo.InvariantCulture,
                                     DateTimeStyles.None,
                                     out parsedStartDate);

    var isEndDateCorrect = DateTime.TryParseExact
                                  (endDateAsString, "yyyy-MM-dd",
                                  CultureInfo.InvariantCulture,
                                  DateTimeStyles.None,
                                   out parsedEndtDate);

    if (!isStartDateCorrect || !isEndDateCorrect)
    {
        throw new ArgumentException("Failed to parse the date.");
    }

    if (parsedEndtDate < parsedStartDate)
    {
        throw new ArgumentException("End date cannot be less than start date.");
    }
}

static StudetOutputDto[] GetStudentsData(int minCredits, DateTime parsedStartDate, DateTime parsedEndtDate, List<string> studentPINs, CourseraContext db, IQueryable<Student> studentsOutput)
{
    if (studentPINs.Count > 0)
    {
        studentsOutput = db.Students
                .SelectMany(x => x.StudentsCoursesXrefs)
                .Where(x => studentPINs.Contains(x.StudentPin))
                .Select(x => x.StudentPinNavigation)
                .Distinct();
    }

    if (studentsOutput == null)
    {
        studentsOutput = db.Students
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
