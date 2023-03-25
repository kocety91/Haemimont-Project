using Haemimont_Interview.FilesOutput;
using System.Globalization;

namespace Haemimont_Interview.Inputs
{
    public class InputData
    {

        public readonly Dictionary<string, FilesOutput.File> Dict = new Dictionary<string, FilesOutput.File>()
        {
             {".csv", new CsvFile()},
             {".html", new HtmlFile()},
        };

        public int MinCredits { get; set; }

        public DateTime ParsedStartDate { get; set; }

        public DateTime ParsedEndtDate{ get; set; }

        public string FolderName { get; set; }

        public string FileName { get; set; }

        public List<string> StudentPins { get; set; }

        public void ReadInputData()
        {
            var studentsInput = Console.ReadLine().Split(",",StringSplitOptions.RemoveEmptyEntries);
            this.MinCredits = int.Parse(studentsInput[0]);

            var parsedData =  DateTimeParser.ParseDateRange(studentsInput[1], studentsInput[2]);
            this.ParsedStartDate = parsedData.parsedStartDate;
            this.ParsedEndtDate = parsedData.parsedEndtDate;

            this.FolderName = studentsInput[3];
            this.FileName = studentsInput[4];

            this.StudentPins = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
