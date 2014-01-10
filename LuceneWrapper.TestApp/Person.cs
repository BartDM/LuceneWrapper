using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneWrapper.TestApp
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public List<Language> Languages { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int RegistrationNumber { get; set; }
        public string RegistrationSuffix { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLineFormat("Id: {0}", Id);
            sb.AppendLineFormat("FirstName: {0}", FirstName);
            sb.AppendLineFormat("LastName: {0}", LastName);
            sb.AppendLineFormat("EmailAddress: {0}", EmailAddress);
            foreach (var language in Languages)
            {
                sb.AppendLineFormat("Language: {0}", language.LanguageCode);
            }

            sb.AppendLineFormat("Registration: {0}-{1}-{2}", RegistrationDate.Year, RegistrationNumber,
                RegistrationSuffix);

            return sb.ToString();
        }
    }

    public class Language
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
    }
}
