using AntonPaarWordCounter.Interfaces;

namespace AntonPaarWordCounter.Implementation
{
    internal class TextFileValidator : IFileValidator
    {
        public void Validate(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"File {fileName} does not exist.");
            if (!Path.GetExtension(fileName).Equals(".txt"))
                throw new ArgumentException($"File format is not supported.");
        }
    }
}
