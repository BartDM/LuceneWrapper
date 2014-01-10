using System.Linq;
using System.Reflection;
using System.Text;
using Lucene.Net.Documents;

namespace LuceneWrapper
{
    /// <summary>
    /// Abstract class as base for all Seach Documents
    /// </summary>
    public abstract class ADocument : IDocument
    {
        private int id;

        /// <summary>
        /// The ID of the item
        /// </summary>
        [SearchField]
        public int Id
        {
            set
            {
                id = value;
                AddParameterToDocument("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED);
            }
            get { return id; }
        }

        /// <summary>
        /// The type of the object parsed to string
        /// </summary>
        public string TypeString { get; set; }

#pragma warning disable 649
        private readonly Document document;
#pragma warning restore 649

        /// <summary>
        /// The Lucene Document
        /// </summary>
        public Document Document { get { return document; } }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ADocument()
        {
            document = new Document();
        }

        /// <summary>
        /// Overload constructor
        /// </summary>
        /// <param name="typeString">The type of the object parsed to string</param>
        protected ADocument(string typeString)
        {
            TypeString = typeString;
        }

        /// <summary>
        /// Method to add parameters to the Lucene Document
        /// Only parameters that are added to the Lucene document can be searched on
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="store">The Store setting</param>
        /// <param name="index">The Index setting</param>
        private void AddParameterToDocument(string name, dynamic value, Field.Store store, Field.Index index)
        {
            document.Add(new Field(name, value.ToString(), store, index));
        }

        protected void AddParameterToDocumentStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value,Field.Store.YES,Field.Index.ANALYZED);
        }

        protected void AddParameterToDocumentNoStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value,Field.Store.NO,Field.Index.ANALYZED);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("ADocument implementation of Type {0}", GetType()));
            sb.AppendLine(string.Format("ID: {0}", Id));
            sb.AppendLine(string.Format("TypeString: {0}", TypeString));
            sb.AppendLine("SearchFields:");

            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var o in attributes)
                {
                    var attr = o as SearchField;
                    if (attr != null)
                    {
                        sb.AppendLine(string.Format("{0}: {1}",property.Name, property.GetValue(property)));
                        if (attr.CombinedSearchFields.Any())
                        {
                            sb.AppendLine("Searchfield has combined fields:");
                            for (int i = 0; i < attr.CombinedSearchFields.Count(); i++)
                            {
                                sb.AppendLine(string.Format("{1} is combined with: {0}:",attr.CombinedSearchFields[i], property.Name));
                            }
                        }
                    }
                }
            }
            sb.AppendLine(string.Format("Lucene document has properties: {0}", Document.fields_ForNUnit));
            return sb.ToString();
        }
    }

    /// <summary>
    /// Custom attribute to define the field that can be seached on
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class SearchField : System.Attribute
    {
        public string[] CombinedSearchFields;


        public SearchField(params string[] values)
        {
            this.CombinedSearchFields = values;
        }
    }
}
