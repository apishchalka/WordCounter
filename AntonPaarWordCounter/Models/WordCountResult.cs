namespace AntonPaarWordCounter.Models
{
    public class WordCountResult
    {
        public IEnumerable<WordCountItem> Data { get; private set; }


        public WordCountResult(IEnumerable<WordCountItem> data)
        {
            Data = data;
        }

        internal WordCountResult(IDictionary<string, long> data)
        {
            Data = from item in data.OrderByDescending(x => x.Value)
                   select new WordCountItem(item.Key, item.Value);
        }
    }
}
