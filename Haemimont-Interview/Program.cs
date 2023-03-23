using Haemimont_Interview;
using Haemimont_Interview.Models;
using System.Globalization;
using System.Text.Json;

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
    var outputPath = Console.ReadLine().Split("/", StringSplitOptions.RemoveEmptyEntries).ToArray();

    var folderName = outputPath[0];
    var fileName = outputPath[1];

    ////output format
    //var outputFormat = Console.ReadLine();

    ////Students PIN's or Students with enought credits
    var studentPINs = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();


    var db = new CourseraContext();
    IQueryable<Student> studentsOutput = null;

    var studentDto = GetStudentsData(minCredits, parsedStartDate, parsedEndtDate, studentPINs, db, studentsOutput);

    var currentDirectory = Directory.GetCurrentDirectory();
    var directoryPath = Path.GetDirectoryName(currentDirectory);

    var folerPath = Path.Combine(directoryPath, folderName);

    if (!Directory.Exists(folerPath))
    {
        Directory.CreateDirectory(folerPath);
    }

    var outputDirectory = Path.Combine(folerPath, fileName);
    var serializedStudents = JsonSerializer.Serialize(studentDto);

    File.WriteAllText(outputDirectory, serializedStudents);
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
                         CourseNames = x.StudentsCoursesXrefs.Select(c => c.Course.Name),
                         Times = x.StudentsCoursesXrefs.Select(t => t.Course.TotalTime),
                         Credits = x.StudentsCoursesXrefs.Select(c => c.Course.Credit),
                         Instructors = x.StudentsCoursesXrefs.Select(i => i.Course.Instructor.FirstName + " " + i.Course.Instructor.LastName),
                         TotalCredit = x.StudentsCoursesXrefs.Sum(c => c.Course.Credit)
                     }).ToArray();
    return output;
}
