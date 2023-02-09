using System.Collections.Generic;

namespace Exam.Categorization
{
    public class Category
    {
        private Category() 
        {  
            this.Children = new HashSet<Category>();
        }

        public Category(string id, string name, string description)
            : this()
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public int Depth { get; set; }

        public int Level { get; set; }

        public Category Parent { get; set; }

        public ICollection<Category> Children { get; set; }
    }
}
