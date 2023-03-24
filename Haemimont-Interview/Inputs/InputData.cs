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
            this.ParseDateInput(studentsInput[1], studentsInput[2]);
            this.FolderName = studentsInput[3];
            this.FileName = studentsInput[4];

            this.StudentPins = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private void ParseDateInput(string startDateAsString, string endDateAsString)
        {
            DateTime parsedStartDate;
            DateTime parsedEndtDate;

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

            this.ParsedStartDate = parsedStartDate;
            this.ParsedEndtDate = parsedEndtDate;
        }
    }
}
