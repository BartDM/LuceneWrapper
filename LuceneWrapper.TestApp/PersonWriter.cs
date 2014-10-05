using System.Collections.Generic;
using System.Linq;

namespace LuceneWrapper.TestApp
{
    public class PersonWriter : BaseWriter
    {
        public PersonWriter(string dataFolder)
            : base(dataFolder)
        {
        }

        public void AddUpdatePersonToIndex(Person person)
        {
            AddUpdateItemsToIndex(new List<PersonDocument> { (PersonDocument)person });
        }

        public void AddUpdatePeopleToIndex(List<Person> people)
        {
            AddUpdateItemsToIndex(people.Select(p => (PersonDocument)p).ToList());
        }

        public void DeletePersonFromIndex(Person person)
        {
            DeleteItemsFromIndex(new List<PersonDocument> { (PersonDocument)person });
        }

        public void DeletePersonFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<PersonDocument> { new PersonDocument { Id = id } });
        }
    }
}
