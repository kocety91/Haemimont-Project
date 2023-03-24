using System.Text;
using System.Text.Json;

namespace Haemimont_Interview.FilesOutput
{
    public class CsvFile : File
    {
        public override void WriteToFile(string folderName, string fileName, StudetOutputDto[] students)
        {
            var outputDirectory = base.GetOutputDirectory(folderName, fileName);

            var reportAsCsvFile = this.GenerateCsvReport(students);

            using StreamWriter streamWriter = new StreamWriter(outputDirectory);
            streamWriter.WriteLine(reportAsCsvFile);
        }

        private string GenerateCsvReport(StudetOutputDto[] students)
        {
            var sb = new StringBuilder();
            foreach (var student in students)
            {
                sb.AppendLine($"<{student.FullName}>,<{student.TotalCredit}>)");
                foreach (var course in student.Courses)
                {
                    sb.AppendLine($"  <{course.Name}>,<{course.TotalTime}>,<{course.Credit}>,<{course.InstructorName}>");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
