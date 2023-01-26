namespace AA_Tree
{
    using System;

    public class AATree<T> : IBinarySearchTree<T>
        where T : IComparable<T>
    {
        private class Node
        {
            public Node(T element)
            {
                this.Value = element;
                this.Level = 1;
            }

            public T Value { get; set; }

            public Node Right { get; set; }

            public Node Left { get; set; }

            public int Level { get; set; }
        }

        private Node root;

        public int Count()
        {
            return this.CountNodes(this.root);
        }

        public void Insert(T element)
        {
            this.root = this.InsertNode(this.root, element);
        }

        public bool Contains(T element)
        {
            var currentNode = this.root;

            while (currentNode != null)
            {
                if (currentNode.Value.CompareTo(element) == 0)
                {
                    return true;
                }
                else if (currentNode.Value.CompareTo(element) > 0)
                {
                    currentNode = currentNode.Left;
                }
                else
                {
                    currentNode = currentNode.Right;
                }
            }

            return false;
        }

        public void InOrder(Action<T> action)
        {
            this.InOrderNodes(this.root, action);
        }

        public void PreOrder(Action<T> action)
        {
            this.PreOrderNodes(this.root, action);
        }

        public void PostOrder(Action<T> action)
        {
            this.PostOrderNodes(this.root, action);
        }

        // Helpers
        private int CountNodes(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + this.CountNodes(node.Left) + this.CountNodes(node.Right);
        }

        private Node InsertNode(Node node, T element)
        {
            if (node == null)
            {
                return new Node(element);
            }

            if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.InsertNode(node.Left, element);
            }
            else
            {
                node.Right = this.InsertNode(node.Right, element);
            }

            node = this.Skew(node);
            node = this.Split(node);

            return node;
        }

        private Node Split(Node node)
        {
            if (node.Right == null || node.Right.Right == null)
            {
                return node;
            }
            else if (node.Right.Right.Level == node.Level)
            {
                node = this.RotateRight(node);
                node.Level++;
            }

            return node;
        }

        private Node Skew(Node node)
        {
            if (node.Left != null && node.Level == node.Left.Level)
            {
                node = this.RotateLeft(node);
            }

            return node;
        }

        private Node RotateRight(Node node)
        {
            var tempNode = node.Right;
            node.Right = tempNode.Left;
            tempNode.Left = node;

            return tempNode;
        }
        
        private Node RotateLeft(Node node)
        {
            var tempNode = node.Left;
            node.Left = tempNode.Right;
            tempNode.Right = node;

            return tempNode;
        }

        private void InOrderNodes(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.InOrderNodes(node.Left, action);

            action(node.Value);

            this.InOrderNodes(node.Right, action);
        }

        private void PreOrderNodes(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            action(node.Value);
            this.PreOrderNodes(node.Left, action);
            this.PreOrderNodes(node.Right, action);
        }
        
        private void PostOrderNodes(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.PostOrderNodes(node.Left, action);
            this.PostOrderNodes(node.Right, action);
            action(node.Value);
        }
    }
}