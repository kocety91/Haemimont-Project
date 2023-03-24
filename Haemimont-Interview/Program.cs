using Haemimont_Interview.Inputs;
using Haemimont_Interview.Models;

try
{
    var inputData = new InputData();
    inputData.ReadInputData();
   

    //BaseIput file = new FileInput(new CourseraContext());
    BaseInput db = new DbInput(new CourseraContext());

    var studentDto = db.GetStudentsData
                             (inputData.MinCredits,
                             inputData.ParsedStartDate,
                             inputData.ParsedEndtDate,
                             inputData.StudentPins);

    if (inputData.FileName != string.Empty)
    {
        var file = inputData.Dict[inputData.FileName];
        file.WriteToFile(inputData.FolderName, inputData.FileName, studentDto.ToArray());
    }
    else
    {
        foreach (var item in inputData.Dict)
        {
            item.Value.WriteToFile(inputData.FolderName, item.Key, studentDto.ToArray());
        }
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
