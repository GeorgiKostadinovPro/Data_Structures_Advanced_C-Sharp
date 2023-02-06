namespace CollectionOfPeople
{
    using System.Collections.Generic;
    using System.Linq;

    public class PeopleCollectionSlow : IPeopleCollection
    {
        private readonly ICollection<Person> people;

        public PeopleCollectionSlow()
        {
            this.people = new List<Person>();
        }

        public int Count => this.people.Count;

        public bool Add(string email, string name, int age, string town)
        {
            Person person = this.Find(email);

            if (person != null)
            {
                return false;
            }

            person = new Person(email, name, age, town);
            this.people.Add(person);

            return true;
        }

        public bool Delete(string email)
        {
            Person person = this.Find(email);

            if (person == null)
            {
                return false;
            }

            this.people.Remove(person);

            return true;
        }

        public Person Find(string email)
        {
            return this.people.FirstOrDefault(p => p.Email == email);
        }

        public IEnumerable<Person> FindPeople(string emailDomain)
        {
            return this.people
                .Where(p => p.Email.EndsWith($"@{emailDomain}"))
                .OrderBy(p => p.Email)
                .ToList();
        }

        public IEnumerable<Person> FindPeople(string name, string town)
        {
            return this.people
                .Where(p => p.Name == name && p.Town == town)
                .OrderBy(p => p.Email)
                .ToList();
        }

        public IEnumerable<Person> FindPeople(int startAge, int endAge)
        {
            return this.people
               .Where(p => p.Age >= startAge && p.Age <= endAge)
               .OrderBy(p => p.Age)
               .ThenBy(p => p.Email)
               .ToList();
        }

        public IEnumerable<Person> FindPeople(int startAge, int endAge, string town)
        {
            return this.people
               .Where(p => p.Age >= startAge && p.Age <= endAge
                      && p.Town == town)
               .OrderBy(p => p.Age)
               .ThenByDescending(p => p.Town)
               .ThenBy(p => p.Email)
               .ToList();
        }
    }
}
