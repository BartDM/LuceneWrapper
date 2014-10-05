using System.Collections.Generic;
using System.Linq;

namespace LuceneWrapper.TestApp
{
    public class PersonDocument : ADocument
    {
        private string lastName;
        private string firstName;
        private IEnumerable<string> languages;
        private string regDate;
        private string regNr;
        private string regSuffix;

        [SearchField]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                AddParameterToDocumentNoStoreParameter("LastName", lastName);
            }
        }

        [SearchField]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                AddParameterToDocumentNoStoreParameter("FirstName", firstName);
            }
        }

        [SearchField]
        public IEnumerable<string> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                foreach (var language in languages)
                {
                    AddParameterToDocumentNoStoreParameter("Languages", language);
                }
            }
        }

        [SearchField]
        public string RegDate
        {
            get { return regDate; }
            set
            {
                regDate = value;
                AddParameterToDocumentNoStoreParameter("RegDate", regDate);
            }
        }

        [SearchField]
        public string RegNr
        {
            get { return regNr; }
            set
            {
                regNr = value;
                AddParameterToDocumentNoStoreParameter("RegNr", regNr);
            }
        }

        [SearchField]
        public string RegSuffix
        {
            get { return regSuffix; }
            set
            {
                regSuffix = value;
                AddParameterToDocumentNoStoreParameter("RegSuffix", regSuffix);
            }
        }

        [SearchField("RegDate", "RegNr", "RegSuffix")]
        public string RegistrationString { get; set; }

        public static explicit operator PersonDocument(Person person)
        {
            return new PersonDocument()
            {
                LastName = person.LastName,
                FirstName = person.FirstName,
                Languages = person.Languages.Select(l => l.LanguageCode),
                RegDate = person.RegistrationDate.Year.ToString(),
                RegNr = person.RegistrationNumber.ToString(),
                regSuffix = person.RegistrationSuffix,
                Id = person.Id
            };
        }
    }
}
