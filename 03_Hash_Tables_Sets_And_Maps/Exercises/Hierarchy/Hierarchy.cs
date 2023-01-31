namespace Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    public class Hierarchy<T> : IHierarchy<T>
    {
        private class Node
        {
            private Node()
            {
                this.Children = new List<Node>();
            }

            public Node(T value)
                : this()
            {
                this.Value = value;
            }

            public T Value { get; set; }

            public Node Parent { get; set; }

            public List<Node> Children { get; set; }
        }
        
        private Node root;
        private Dictionary<T, Node> nodesByValue;

        private Hierarchy()
        {
            this.nodesByValue = new Dictionary<T, Node>();
        }

        public Hierarchy(T value)
            : this()
        {
            this.root = new Node(value);
            this.nodesByValue.Add(value, this.root);
        }

        public int Count => this.nodesByValue.Count;

        public void Add(T element, T child)
        {
            if (!this.nodesByValue.ContainsKey(element)
                || this.nodesByValue.ContainsKey(child))
            {
                throw new ArgumentException();
            }

            Node childNode = new Node(child);
            Node parentNode = this.nodesByValue[element];
            
            parentNode.Children.Add(childNode);
            childNode.Parent = parentNode;
            this.nodesByValue.Add(child, childNode);
        }

        public bool Contains(T element)
        {
            return this.nodesByValue.ContainsKey(element);
        }

        public IEnumerable<T> GetChildren(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            return this.nodesByValue[element]
                .Children.Select(n => n.Value)
                .ToArray();
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            //List<T> keys = new List<T>();

            //foreach (var key in this.nodesByValue.Keys)
            //{
            //    if (other.nodesByValue.ContainsKey(key))
            //    {
            //        keys.Add(key);
            //    }
            //}

            //return keys;

            return this.nodesByValue.Keys
                .Intersect(other.nodesByValue.Keys);
        }

        public T GetParent(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            if (this.nodesByValue[element].Parent == null)
            {
                return default;
            }

            return this.nodesByValue[element].Parent.Value;
        }

        public void Remove(T element)
        {   
            if (!this.nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            if (element.Equals(this.root.Value))
            {
                throw new InvalidOperationException();
            }

            Node nodeToRemove = this.nodesByValue[element];
            Node parentNode = nodeToRemove.Parent;

            parentNode.Children.Remove(nodeToRemove);
            parentNode.Children.AddRange(nodeToRemove.Children);

            foreach (var childNode in nodeToRemove.Children)
            {
                this.nodesByValue[childNode.Value].Parent = parentNode;
            }

            this.nodesByValue.Remove(element);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            Queue<Node> queue = new Queue<Node>();

            queue.Enqueue(this.root);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();
                yield return currentNode.Value;

                foreach (var childNode in currentNode.Children)
                {
                    queue.Enqueue(childNode);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}