using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace LuceneWrapper
{
    /// <summary>
    /// Base abstract class where that every Searcher should implmement
    /// </summary>
    public abstract class BaseSearcher : BaseSearch
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseSearcher));

        private const int HitsLimit = 1000;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataFolder">The App Data folder - or the folder where the lucene folder is placed under</param>
        protected BaseSearcher(string dataFolder)
            : base(dataFolder)
        {
            Log.DebugFormat("Initialisation Searcher with folder {0}", dataFolder);
        }

        /// <summary>
        /// Base Search method
        /// </summary>
        /// <typeparam name="T">The type of document that has tobe searched for</typeparam>
        /// <param name="field">The field that have to be searched for</param>
        /// <param name="searchQuery">The query as string with the search term</param>
        /// <returns>A SearchResult object</returns>
        protected SearchResult Search<T>(string field, string searchQuery) where T : ADocument
        {
            Log.DebugFormat("Searching for Type: {0} with query \"{1}\" for field \"{2}\"", typeof(T), searchQuery, field);
            //Fetch the possible fields to search on
            PropertyInfo[] properties = typeof(T).GetProperties();
            var fields = new List<string>();

            var fieldsToSearchOn = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var o in attributes)
                {
                    var attr = o as SearchField;
                    if (attr != null)
                    {
                        fields.Add(property.Name);
                        if (attr.CombinedSearchFields.Any() && field == property.Name)
                        {
                            fieldsToSearchOn.Add(property.Name);
                            for (int i = 0; i < attr.CombinedSearchFields.Count(); i++)
                            {
                                fieldsToSearchOn.Add(attr.CombinedSearchFields[i]);
                            }
                        }
                        else if (field == property.Name)
                        {
                            fieldsToSearchOn.Add(property.Name);
                        }
                    }
                }
            }

            Log.DebugFormat("Fields available to search on for Type {0}", typeof(T));
            fields.ForEach(f => Log.DebugFormat("{0}", f));

            if (!string.IsNullOrEmpty(field))
            {
                Log.DebugFormat("Searching on field {0}, Combined fields:", field);
                fieldsToSearchOn.ForEach(f => Log.DebugFormat("{0}", f));
            }

            using (var searcher = new IndexSearcher(LuceneDirectory))
            {
                Log.Debug("Starting new IndexSearcher");
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var searchResults = new SearchResult
                {
                    SearchTerm = searchQuery,
                    SearchResultItems = new List<SearchResultItem>()
                };

                ScoreDoc[] hits;
                if (!string.IsNullOrEmpty(field))
                {
                    if (!fields.Contains(field))
                    {
                        throw new SearchException(string.Format("Field {0} is not a search field for type {1}", field, typeof(T)));
                    }
                    QueryParser parser = fieldsToSearchOn.Count == 1 ?
                        new QueryParser(Version.LUCENE_30, fieldsToSearchOn.First(), analyzer) :
                        new MultiFieldQueryParser(Version.LUCENE_30, fieldsToSearchOn.ToArray(), analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    hits = searcher.Search(query, HitsLimit).ScoreDocs;
                }
                else
                {
                    var parser = new MultiFieldQueryParser(Version.LUCENE_30, fields.ToArray(), analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    hits = searcher.Search(query, null, HitsLimit, Sort.RELEVANCE).ScoreDocs;
                }
                if (hits != null)
                {
                    Log.DebugFormat("Hits found: {0}", hits.Count());
                    searchResults.Hits = hits.Count();
                    foreach (var hit in hits)
                    {
                        var doc = searcher.Doc(hit.Doc);
                        searchResults.SearchResultItems.Add(new SearchResultItem
                        {
                            Id = Convert.ToInt32(doc.Get("Id")),
                            Score = hit.Score,
                        });
                    }
                }
                else
                {
                    Log.DebugFormat("No hits found");
                }
                analyzer.Close();
                searcher.Dispose();
                return searchResults;
            }
        }

        /// <summary>
        /// Parse the givven query string to a Lucene Query object
        /// </summary>
        /// <param name="searchQuery">The query string</param>
        /// <param name="parser">The Lucense QueryParser</param>
        /// <returns>A Lucene Query object</returns>
        private Query ParseQuery(string searchQuery, QueryParser parser)
        {
            parser.AllowLeadingWildcard = true;
            Query q;
            try
            {
                q = parser.Parse(searchQuery);
            }

            catch (ParseException e)
            {
                Log.Error("Query parser exception", e);
                q = null;
            }

            if (q == null || string.IsNullOrEmpty(q.ToString()))
            {
                string cooked = Regex.Replace(searchQuery, @"[^\w\.@-]", " ");
                q = parser.Parse(cooked);
            }
            Log.DebugFormat("Parsed query for Lucene: \"{0}\"", q);

            return q;
        }
    }
}
