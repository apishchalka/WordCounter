using AntonPaarWordCounter.Models;

namespace AntonPaarWordCounter.Interfaces
{
    public interface IAntonPaarWordCounter
    {
        Task<WordCountResult> CountAsync(string fileName, IProgress<int> progress, CancellationToken token);
    }
}
