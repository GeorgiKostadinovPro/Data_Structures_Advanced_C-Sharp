namespace AVLTree
{
    using System;

    public class AVL<T> where T : IComparable<T>
    {
        public class Node
        {
            public Node(T value)
            {
                this.Value = value;
                this.Height = 1;
            }

            public T Value { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public int Height { get; set; }

            public override string ToString()
            { 
                return $"V: [{this.Value}] R: [{this.Right}] L: [{this.Left}] H: [{this.Height}]";
            }
        }

        public Node Root { get; private set; }

        public bool Contains(T element)
        {
            return this.SearchDFS(this.Root, element) != null;
        }

        public void Delete(T element)
        {
            this.Root = this.DeleteNode(this.Root, element);
        }

        public void DeleteMin()
        {
            if (this.Root == null)
            {
                return;
            }

            var tempNode = this.GetSmallestChild(this.Root);
            this.Root = this.DeleteNode(this.Root, tempNode.Value);
        }

        public void Insert(T element)
        {
            this.Root = this.InsertNodes(this.Root, element);
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.Root, action);
        }

        // Helpers
        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        } 
        
        private Node SearchDFS(Node node, T element)
        {
            if (node == null)
            {
                return null;
            }

            int compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                return this.SearchDFS(node.Left, element);
            }
            else if (compare > 0)
            {
                return this.SearchDFS(node.Right, element);
            }

            return node;
        }

        private int GetHeight(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.Height;
        }
        
        private Node InsertNodes(Node node, T element)
        {
            if (node == null)
            {
                return new Node(element);
            }

            if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.InsertNodes(node.Left, element);
            }
            else
            {
                node.Right = this.InsertNodes(node.Right, element);
            }

            node = this.BalanceTree(node);
            node.Height = this.ReBalanceHeight(node);

            return node;
        }

        private Node BalanceTree(Node node)
        {
            var balanceFactor = this.GetHeight(node.Left) - this.GetHeight(node.Right);

            if (balanceFactor > 1)
            {
                var childBalanceFactor = this.GetHeight(node.Left.Left) - this.GetHeight(node.Left.Right);

                if (childBalanceFactor < 0)
                {
                    node.Left = this.RotateLeft(node.Left);
                }

                node = this.RotateRight(node);
            }
            else if (balanceFactor < -1)
            {
                var childBalanceFactor = this.GetHeight(node.Right.Left) - this.GetHeight(node.Right.Right);

                if (childBalanceFactor > 0)
                {
                    node.Right = this.RotateRight(node.Right);
                }

                node = this.RotateLeft(node);
            }

            return node;
        }

        private Node RotateRight(Node node)
        {
            var left = node.Left;
            node.Left = left.Right;
            left.Right = node;

            node.Height = this.ReBalanceHeight(node);

            return left;
        }

        private Node RotateLeft(Node node)
        {
            var right = node.Right;
            node.Right = right.Left;
            right.Left = node;

            node.Height = this.ReBalanceHeight(node);

            return right;
        }

        private Node DeleteNode(Node node, T element)
        {
            if (node == null)
            {
                return null;
            }

            int compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                node.Left = this.DeleteNode(node.Left, element);
            }
            else if (compare > 0)
            {
                node.Right = this.DeleteNode(node.Right, element);
            }
            else
            {
                if (node.Left == null && node.Right == null)
                {
                    return null;
                }
                else if (node.Left == null)
                {
                    node = node.Right;
                }
                else if (node.Right == null)
                {
                    node = node.Left;
                }
                else
                {
                    Node tempNode = this.GetSmallestChild(node.Right);

                    node.Value = tempNode.Value;
                    node.Right = this.DeleteNode(node.Right, tempNode.Value);
                }
            }

            node = this.BalanceTree(node);
            node.Height = this.ReBalanceHeight(node);

            return node;
        }

        private Node GetSmallestChild(Node node)
        {
            if (node.Left == null)
            {
                return node;
            }

            return this.GetSmallestChild(node.Left);
        }
        
        private int ReBalanceHeight(Node node)
        {
            return Math.Max(this.GetHeight(node.Left), this.GetHeight(node.Right)) + 1;
        }
    }
}
