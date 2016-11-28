using System.Collections.Generic;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.People;

namespace LearningMpaAbp.Migrations.SeedData
{
    public class DefaultTestDataForPerson
    {
        private readonly LearningMpaAbpDbContext _context;

        private static readonly List<Person> Persons;

        public DefaultTestDataForPerson(LearningMpaAbpDbContext context)
        {
            _context = context;
        }

        static DefaultTestDataForPerson()
        {
            Persons = new List<Person>()
            {
                new Person() {Name = "Ê¥½Ü"},
                new Person() {Name = "Ð¡ÓÂ"},
                new Person() {Name = "Ð¡¶þÀÉ"}
            };
        }

        public void Create()
        {
            foreach (var person in Persons)
            {
                _context.People.Add(person);
            }

            _context.SaveChanges();
        }

    }
}