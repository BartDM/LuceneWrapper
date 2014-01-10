using System.Collections.Generic;

namespace LuceneWrapper
{
    /// <summary>
    /// Class to represent the Searh results
    /// </summary>
    public class SearchResult
    {
        public string SearchTerm { get; set; }
        public List<SearchResultItem> SearchResultItems { get; set; }
        public int Hits { get; set; }
    }

    /// <summary>
    /// Class to represent the Search result item
    /// </summary>
    public class SearchResultItem
    {
        public int Id { get; set; }
        public float Score { get; set; }
    }
}
