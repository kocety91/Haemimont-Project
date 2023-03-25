using Haemimont_Interview.Dtos;

namespace Haemimont_Interview.FilesOutput
{
    public abstract class File
    {
        public abstract void WriteToFile(string folderName, string fileName, StudetOutputDto[] students);

        public string GetOutputDirectory(string folderName, string fileName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryPath = Path.GetDirectoryName(currentDirectory);

            var folerPath = Path.Combine(directoryPath, folderName);

            if (!Directory.Exists(folerPath))
            {
                Directory.CreateDirectory(folerPath);
            }

            var outputDirectory = Path.Combine(folerPath,fileName);

            return outputDirectory;
        }
    }
}
