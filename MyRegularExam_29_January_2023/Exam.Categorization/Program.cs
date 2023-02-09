using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Categorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Categorizator categorizator = new Categorizator();

            Category category1 = new Category("1", "a", "te");
            Category category2 = new Category("2", "b", "tet");
            Category category3 = new Category("3", "c", "te");
            Category category4 = new Category("4", "d", "te");
            Category category5 = new Category("5", "e", "te");

            categorizator.AddCategory(category1);
            categorizator.AddCategory(category2);
            categorizator.AddCategory(category3);
            categorizator.AddCategory(category4);
            categorizator.AddCategory(category5);

            categorizator.AssignParent(category3.Id, category2.Id);
            categorizator.AssignParent(category4.Id, category2.Id);
            categorizator.AssignParent(category5.Id, category4.Id);
            categorizator.AssignParent(category2.Id, category1.Id);

            var set = categorizator.GetTop3CategoriesOrderedByDepthOfChildrenThenByName();

            Console.WriteLine(string.Join(", ", set.Select(c => c.Id)));
        }
    }
}
