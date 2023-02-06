namespace CollectionOfPeople
{
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class PeopleCollection : IPeopleCollection
    {
        private readonly IDictionary<string, Person> peopleByEmail;
        private readonly IDictionary<string, SortedSet<Person>> peopleByEmailDomain;
        private readonly IDictionary<(string, string), SortedSet<Person>> peopleByNameAndTown;
        
        private readonly OrderedDictionary<int, SortedSet<Person>> peopleByAge;
        private readonly IDictionary<string, OrderedDictionary<int, SortedSet<Person>>>
            peopleByTownAndAge;

        public PeopleCollection()
        {
            this.peopleByEmail = new Dictionary<string, Person>();
            this.peopleByEmailDomain = new Dictionary<string, SortedSet<Person>>();
            this.peopleByNameAndTown = new Dictionary<(string, string), SortedSet<Person>>();

            this.peopleByAge = new OrderedDictionary<int, SortedSet<Person>>();
            this.peopleByTownAndAge = new Dictionary<string, OrderedDictionary<int, SortedSet<Person>>>();
        }

        public int Count => this.peopleByEmail.Count;

        public bool Add(string email, string name, int age, string town)
        {
            Person person = this.Find(email);

            if (person != null)
            {
                return false;
            }

            person = new Person(email, name, age, town);
            this.peopleByEmail.Add(email, person); 
            
            string emailDomain = email.Split('@')[1];
            this.peopleByEmailDomain.AppendValueToKey(emailDomain, person);
            
            // The AppendValueToKey method is equal to this whole functionality
            //if (!this.peopleByEmailDomain.ContainsKey(emailDomain))
            //{
            //    this.peopleByEmailDomain.Add(emailDomain, new SortedSet<Person>());
            //}

            //this.peopleByEmailDomain[emailDomain].Add(person);

            this.peopleByNameAndTown
                .AppendValueToKey((name, town), person);

            this.peopleByAge.AppendValueToKey(age, person);

            this.peopleByTownAndAge.EnsureKeyExists(town);
            this.peopleByTownAndAge[town].AppendValueToKey(age, person);

            return true;
        }

        public bool Delete(string email)
        {
            Person person = this.Find(email);

            if (person == null)
            {
                return false;
            }

            this.peopleByEmailDomain[email.Split('@')[1]]
                .Remove(person);

            this.peopleByNameAndTown[(person.Name, person.Town)]
                .Remove(person);

            this.peopleByAge.Remove(person.Age);

            this.peopleByTownAndAge[person.Town][person.Age].Remove(person);

            return this.peopleByEmail.Remove(email);
        }

        public Person Find(string email)
        {
            try
            {
                Person person = this.peopleByEmail[email];

                return person;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public IEnumerable<Person> FindPeople(string emailDomain)
        {
            return this.peopleByEmailDomain.GetValuesForKey(emailDomain);
        }

        public IEnumerable<Person> FindPeople(string name, string town)
        {
            return this.peopleByNameAndTown.GetValuesForKey((name, town));
        }

        public IEnumerable<Person> FindPeople(int startAge, int endAge)
        {
            OrderedDictionary<int, SortedSet<Person>>.View peopleInRange 
                = this.peopleByAge.Range(startAge, true, endAge, true);
            
            // This is the iterative foreach way
            //foreach (var kvp in peopleInRange)
            //{
            //    foreach (var person in kvp.Value)
            //    {
            //        yield return person;
            //    }
            //}

            return peopleInRange.SelectMany(kvp => kvp.Value.Select(p => p))
                .OrderBy(p => p.Age);
        }

        public IEnumerable<Person> FindPeople(int startAge, int endAge, string town)
        {
            if (!this.peopleByTownAndAge.ContainsKey(town))
            {
                return Enumerable.Empty<Person>();
            }

            return this.peopleByTownAndAge[town]
                .Range(startAge, true, endAge, true)
                .SelectMany(kvp => kvp.Value.Select(p => p))
                .OrderBy(p => p.Age);
        }
    }
}