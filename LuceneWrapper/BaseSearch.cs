using System.IO;
using Lucene.Net.Store;

namespace LuceneWrapper
{
    /// <summary>
    /// The abstract base class to be implemented by everything that uses the Lucene directory
    /// </summary>
    public abstract class BaseSearch
    {
        private const string LuceneIndexFolder = "LuceneIndex";
        private readonly FSDirectory luceneDirectory;
        private readonly string dataFolder;

        /// <summary>
        /// The App Data folder - or the folder where the lucene folder is placed under
        /// </summary>
        public string DataFolder
        {
            get { return DataFolder; }
        }

        /// <summary>
        /// The App Data folder - or the folder where the lucene folder is placed under as FSDirectory object
        /// </summary>
        public FSDirectory LuceneDirectory
        {
            get { return luceneDirectory; }
        }

        /// <summary>
        /// Constructor that will initialise the LuceneDirectory
        /// </summary>
        /// <param name="dataFolder">The App Data folder - or the folder where the lucene folder is placed under</param>
        protected BaseSearch(string dataFolder)
        {
            this.dataFolder = dataFolder;
            var di = new DirectoryInfo(Path.Combine(dataFolder,LuceneIndexFolder));
            if (!di.Exists)
            {
                di.Create();
            }
            luceneDirectory = FSDirectory.Open(di.FullName);
        }
    }
}
