namespace AntonPaarWordCounter.Models
{
    public class WordCountItem
    {
        public string Word { get; }
        public long Count { get;}

        public WordCountItem(string word, long count)
        {
            Word = word;
            Count = count;
        }
    }
}
