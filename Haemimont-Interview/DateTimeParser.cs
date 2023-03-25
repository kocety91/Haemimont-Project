using System.Globalization;

namespace Haemimont_Interview
{
    public static class DateTimeParser
    {
        public static (DateTime parsedStartDate, DateTime parsedEndtDate) ParseDateRange(string startDateAsString, string endDateAsString)
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


            return (parsedStartDate, parsedEndtDate);
        }
    }
}
