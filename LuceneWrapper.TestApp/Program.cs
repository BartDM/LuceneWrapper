using System;
using System.Collections.Generic;
using System.Linq;

namespace LuceneWrapper.TestApp
{
    public class Program
    {
        private static List<Person> people;

        static void Main(string[] args)
        {
            string dataFolder = @"C:\Temp\LuceneWrapper";

            LoadPeople();
            var writer = new PersonWriter(dataFolder);
            writer.AddUpdatePeopleToIndex(people);

            var searcher = new PersonSearcher(dataFolder);

            Console.WriteLine("Search on first name Bart in FirstName field");
            var res = searcher.SearchPeople("Bart", "FirstName");
            PrintResult(res);

            Console.WriteLine("Search on 2014 in LastName field");
            res = searcher.SearchPeople("2014", "LastName");
            PrintResult(res);

            Console.WriteLine("Search on 2014 in RegistrationString field");
            res = searcher.SearchPeople("2014", "RegistrationString");
            PrintResult(res);

            Console.WriteLine("Search on nl in all fields");
            res = searcher.SearchPeople("nl", string.Empty);
            PrintResult(res);


            Console.ReadKey();

        }

        private static void PrintResult(SearchResult res)
        {
            Console.WriteLine();
            Console.WriteLine("Resuts found: {0}", res.Hits);
            foreach (var item in res.SearchResultItems)
            {
                Console.WriteLine("Result with ID: {0}", item.Id);
                Console.WriteLine(people.First(p => p.Id == item.Id));
            }

        }

        private static void LoadPeople()
        {
            var lang1 = new Language { Description = "Dutch", Id = 1, LanguageCode = "nl-BE" };
            var lang2 = new Language { Description = "French", Id = 2, LanguageCode = "nl-FR" };
            var lang3 = new Language { Description = "english", Id = 3, LanguageCode = "en-UK" };

            people = new List<Person>
            {
                new Person
                {
                    FirstName = "Bart",
                    LastName = "De Meyer",
                    EmailAddress = "test@localtest.me",
                    Id = 1,
                    RegistrationDate = new DateTime(2014, 1, 10),
                    RegistrationNumber = 1,
                    RegistrationSuffix = "a",
                    Languages = new List<Language> {lang1}
                },
                new Person
                {
                    FirstName = "Eddy",
                    LastName = "Janssens",
                    EmailAddress = "eddy@janssens.me",
                    Id = 2,
                    RegistrationDate = new DateTime(2014, 1, 4),
                    RegistrationNumber = 2,
                    Languages = new List<Language> {lang2, lang3}
                },
                new Person
                {
                    FirstName = "Luc",
                    LastName = "Peeters",
                    EmailAddress = "luc@peeters.me",
                    Id = 3,
                    RegistrationDate = new DateTime(2013, 12, 15),
                    RegistrationNumber = 3,
                    Languages = new List<Language> {lang1, lang3}
                },
                new Person
                {
                    FirstName = "Heike",
                    LastName = "Wouters",
                    EmailAddress = "heike@wouters.me",
                    Id = 4,
                    RegistrationDate = new DateTime(2013, 11, 20),
                    RegistrationNumber = 4,
                    Languages = new List<Language> {lang1, lang2}
                }
            };
        }
    }
}
