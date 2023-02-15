using AntonPaarWordCounter.Interfaces;
using AntonPaarWordCounter.Models;
using System.Text;

namespace AntonPaarWordCounter.Implementation
{
    public class DefaultAntonPaarWordCounter : IAntonPaarWordCounter
    {
        private static readonly char[] delimiters = { ' ', '\r', '\n' };
        private readonly IFileValidator _fileValidator;
        public DefaultAntonPaarWordCounter()
        {
            _fileValidator = new TextFileValidator();
        }

        public async Task<WordCountResult> CountAsync(string fileName, IProgress<int> progress, CancellationToken token)
        {
            _fileValidator.Validate(fileName);

            IDictionary<string, long> data = new Dictionary<string, long>();

            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bufferStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferStream, Encoding.UTF8))
                    {
                        string line;
                        int percentageCompleted = fileStream.Length > 0 ? 0 : 100;

                        while ((line = await streamReader.ReadLineAsync()) != null)
                        {
                            token.ThrowIfCancellationRequested();

                            var words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                            foreach (var word in words)
                            {
                                var wordToCheck = word.ToLowerInvariant();

                                if (!data.ContainsKey(wordToCheck))
                                {
                                    data.Add(wordToCheck, 1);
                                }
                                else
                                {
                                    data[wordToCheck] += 1;
                                }
                            }
                            var currentPercentageCompleted = (int)(bufferStream.Position * 100 / fileStream.Length);

                            if (currentPercentageCompleted > percentageCompleted)
                            {
                                percentageCompleted = currentPercentageCompleted;
                                progress.Report(percentageCompleted);
                            }
                        }

                        progress.Report(percentageCompleted);
                    }
                }
            }

            return new WordCountResult(data);
        }
    }
}
