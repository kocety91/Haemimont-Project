using Haemimont_Interview.Dtos;
using System.Text;

namespace Haemimont_Interview.FilesOutput
{
    public class HtmlFile : File
    {
        public override void WriteToFile(string folderName, string fileName, StudetOutputDto[] students)
        {
            var outputDirectory = base.GetOutputDirectory(folderName, fileName);
            var reportAsHtmlTable = this.GenerateReport(students);

            using StreamWriter streamWriter = new StreamWriter(outputDirectory);
            streamWriter.WriteLine(reportAsHtmlTable);
        }

        private string GenerateReport(StudetOutputDto[] students)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table>");
            sb.AppendLine("<thead>" +
                "<tr>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">Student</th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">TotalCredit</th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\"></th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\"></th>" +
               $"<th style=\"border: 1px solid #ddd; padding: 8px;\"></th>" +
                "</tr>");

            sb.AppendLine("<tr>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\"></th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">CourseName</th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">Time</th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">Credit</th>" +
                $"<th style=\"border: 1px solid #ddd; padding: 8px;\">Instructor</th>" +
                "</tr>" +
                "</thead>");

            sb.AppendLine("<tbody>");
            foreach (var student in students)
            {

                sb.AppendLine($"<tr>" +
                    $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{student.FullName}</td>" +
                    $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{student.TotalCredit}</td>" +
                    $"<td style=\"border: 1px solid #ddd; padding: 8px;\"></td>" +
                    $"<td style=\"border: 1px solid #ddd; padding: 8px;\"></td>" +
                    $"<td style=\"border: 1px solid #ddd; padding: 8px;\"></td>" +
                    $"</tr>");


                foreach (var course in student.Courses)
                {
                    sb.AppendLine("<tr>" +
                        $"<td style=\"border: 1px solid #ddd; padding: 8px;\"></td>" +
                        $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{course.Name}</td>" +
                        $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{course.TotalTime}</td>" +
                        $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{course.Credit}</td>" +
                        $"<td style=\"border: 1px solid #ddd; padding: 8px;\">{course.InstructorName}</td>");
                    sb.AppendLine("</tr>");
                }

            }
            sb.AppendLine("</tbody>");

            sb.AppendLine("</table>");

            return sb.ToString().TrimEnd();
        }
    }
}
