using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Categorization
{
    public class Categorizator : ICategorizator
    {
        private readonly IDictionary<string, Category> categoriesById;

        public Categorizator()
        {
            this.categoriesById = new Dictionary<string, Category>();
        }

        public void AddCategory(Category category)
        {
            if (this.categoriesById.ContainsKey(category.Id))
            {
                throw new ArgumentException();
            }

            this.categoriesById.Add(category.Id, category);
        }

        public void AssignParent(string childCategoryId, string parentCategoryId)
        {
            if (!this.categoriesById.ContainsKey(childCategoryId)
                || !this.categoriesById.ContainsKey(parentCategoryId))
            {
                throw new ArgumentException();
            }

            Category childCategory = this.categoriesById[childCategoryId];
            Category parentCategory = this.categoriesById[parentCategoryId];

            if (parentCategory.Children.Any(c => c.Id == childCategoryId)
                || childCategory.Parent != null)
            {
                throw new ArgumentException();
            }

            parentCategory.Children.Add(childCategory);
            childCategory.Parent = parentCategory;
        }

        public bool Contains(Category category)
        {
            return this.categoriesById.ContainsKey(category.Id);
        }

        public IEnumerable<Category> GetChildren(string categoryId)
        {
            if (!this.categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            Category category = this.categoriesById[categoryId];

            Queue<Category> nodes = new Queue<Category>();
            var allDirectAndIndirectChildren = new Queue<Category>();

            nodes.Enqueue(category);

            while (nodes.Count > 0)
            {
                Category currentCategory = nodes.Dequeue();

                allDirectAndIndirectChildren.Enqueue(currentCategory);

                foreach (var childCategory in currentCategory.Children)
                {
                    nodes.Enqueue(childCategory);
                }
            }

            allDirectAndIndirectChildren.Dequeue();

            return allDirectAndIndirectChildren;
        }

        public IEnumerable<Category> GetHierarchy(string categoryId)
        {
            if (!this.categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            Category category = this.categoriesById[categoryId];
            Stack<Category> parentCategories = new Stack<Category>();

            parentCategories.Push(category);

            Category parentCategory = category.Parent;

            while (parentCategory != null)
            {
                parentCategories.Push(parentCategory);
                parentCategory = parentCategory.Parent;
            }

            return parentCategories;
        }

        public IEnumerable<Category> GetTop3CategoriesOrderedByDepthOfChildrenThenByName()
        {
            // Get the depth of all nodes

            //foreach (var category in this.categoriesById.Values.Where(c => c.Parent == null))
            //{
            //    Queue<Category> nodes = new Queue<Category>();
            //    nodes.Enqueue(category);

            //    while (nodes.Count > 0)
            //    {
            //        Category currentNode = nodes.Dequeue();
            //        int depth = 0;

            //        Category parentCategory = currentNode.Parent;

            //        while (parentCategory != null)
            //        {
            //            depth++;

            //            parentCategory = parentCategory.Parent;
            //        }

            //        currentNode.Depth = depth;

            //        foreach (var childNode in currentNode.Children)
            //        {
            //            nodes.Enqueue(childNode);
            //        }
            //    }
            //}

            // This is the better code for finding levels and depths of nodes

            foreach (var leafCategory in this.categoriesById.Values.Where(c => c.Children.Count == 0))
            {
                int currLevel = 0;
                Category currentParent = leafCategory.Parent;

                while (currentParent != null)
                {
                    currLevel++;
                    currentParent.Level = currLevel;

                    currentParent = currentParent.Parent;
                }
            }

            return this.categoriesById.Values
                .OrderByDescending(c => c.Level)
                .ThenBy(c => c.Name)
                .Take(3)
                .ToArray();
        }

        public void RemoveCategory(string categoryId)
        {
            if (!this.categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            Category categoryToRemove = this.categoriesById[categoryId];
            Category categoryToRemoveParent = categoryToRemove.Parent;

            if (categoryToRemoveParent != null)
            {
                categoryToRemove.Parent = null;
                categoryToRemoveParent.Children.Remove(categoryToRemove);
            }
            
            this.categoriesById.Remove(categoryId);

            Queue<Category> nodes = new Queue<Category>();

            nodes.Enqueue(categoryToRemove);

            while (nodes.Count > 0)
            {
                Category currentCategory = nodes.Dequeue();

                if (this.categoriesById.ContainsKey(currentCategory.Id))
                {
                    this.categoriesById.Remove(currentCategory.Id);
                }

                foreach (var childCategory in currentCategory.Children)
                {
                    nodes.Enqueue(childCategory);
                }
            }
        }

        public int Size()
        {
            return this.categoriesById.Count;
        }
    }
}
