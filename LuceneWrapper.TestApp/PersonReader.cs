namespace LuceneWrapper.TestApp
{
    public class PersonSearcher:BaseSearcher
    {
        public PersonSearcher(string dataFolder) : base(dataFolder)
        {
        }

        public SearchResult SearchPeople(string searchTerm, string field)
        {
            return Search<PersonDocument>(field, searchTerm);
        }

    }
}
