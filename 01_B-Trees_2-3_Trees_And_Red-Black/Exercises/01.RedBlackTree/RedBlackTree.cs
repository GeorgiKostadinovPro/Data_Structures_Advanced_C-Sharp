namespace _01.RedBlackTree
{
    using System;
    using System.Xml;

    public class RedBlackTree<T> where T : IComparable
    {
        private const bool Red = true;
        private const bool Black = false;

        public class Node
        {
            public Node(T value)
            {
                this.Value = value;
                this.Color = Red;
            }

            public T Value { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public bool Color { get; set; }
        }

        public Node root;

        private RedBlackTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        public RedBlackTree()
        {

        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(action, this.root);
        }

        public RedBlackTree<T> Search(T element)
        {
            Node currentNode = this.FindNode(element);

            return new RedBlackTree<T>(currentNode);
        }

        public void Insert(T value)
        {
            this.root = this.InsertNode(this.root, value);
            this.root.Color = Black;
        }

        public void Delete(T key)
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.DeleteNode(this.root, key);
        }

        public void DeleteMin()
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.DeleteMinNode(this.root);

            if (this.root != null)
            {
                this.root.Color = Black;
            }
        }

        public void DeleteMax()
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.DeleteMaxNode(this.root);

            if (this.root != null)
            {
                this.root.Color = Black;
            }
        }

        public int Count()
        {
            return this.CountNodes(this.root);
        }

        // Rotations
        private Node RotateLeft(Node node)
        {
            var temp = node.Right;
            node.Right = temp.Left;
            temp.Left = node;
            temp.Color = temp.Left.Color;
            temp.Left.Color = Red;

            return temp;
        }

        private Node RotateRight(Node node)
        {
            var temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;
            temp.Color = temp.Right.Color;
            temp.Right.Color = Red;

            return temp;
        }

        private void FlipColors(Node node)
        {
            node.Color = !node.Color;
            node.Left.Color = !node.Left.Color;
            node.Right.Color = !node.Right.Color;
        }

        private bool IsRed(Node node)
        {
            if (node == null)
            {
                return false;
            }

            return node.Color == Red;
        }

        // Helpers
        private Node DeleteNode(Node node, T key)
        {
            if (this.IsSmaller(key, node.Value))
            {
                if (!this.IsRed(node.Left) && !this.IsRed(node.Left.Left))
                {
                    node = this.MoveRedLeft(node);
                }

                node.Left = this.DeleteNode(node.Left, key);
            }
            else
            {
                if (this.IsRed(node.Left))
                {
                    node = RotateRight(node);
                }

                if (this.AreEqual(key, node.Value) && node.Right == null)
                {
                    return null;
                }

                if (!this.IsRed(node.Right) && !this.IsRed(node.Right.Left))
                {
                    node = this.MoveRedRight(node);
                }

                if (this.AreEqual(key, node.Value))
                {
                    node.Value = this.FindMinValueInSubtree(node.Right);
                    node.Right = this.DeleteMinNode(node.Right);
                }
                else
                {
                    node.Right = this.DeleteNode(node.Right, key);
                }
            }

            return this.FixUp(node);
        }

        private T FindMinValueInSubtree(Node node)
        {
            if (node.Left == null)
            {
                return node.Value;
            }

            return this.FindMinValueInSubtree(node.Left);
        }

        private Node DeleteMinNode(Node node)
        {
            if (node.Left == null)
            {
                return null;
            }

            if (!this.IsRed(node.Left) && !this.IsRed(node.Left.Left))
            {
                node = this.MoveRedLeft(node);
            }

            node.Left = this.DeleteMinNode(node.Left);

            return this.FixUp(node);
        }
        
        private Node DeleteMaxNode(Node node)
        {
            if (this.IsRed(node.Left))
            {
                node = this.RotateRight(node);
            }

            if (node.Right == null)
            {
                return null;
            }

            if (!this.IsRed(node.Right) && !this.IsRed(node.Right.Left))
            {
                node = this.MoveRedRight(node);
            }

            node.Right = this.DeleteMaxNode(node.Right);

            return this.FixUp(node);
        }

        private Node MoveRedLeft(Node node)
        {
            this.FlipColors(node);

            if (this.IsRed(node.Right.Left))
            {
                node.Right = this.RotateRight(node.Right);
                node = this.RotateLeft(node);
                this.FlipColors(node);
            }

            return node;
        } 
        
        private Node MoveRedRight(Node node)
        {
            this.FlipColors(node);

            if (this.IsRed(node.Left.Left))
            {
                node = this.RotateRight(node);
                this.FlipColors(node);
            }

            return node;
        }
        
        private Node FixUp(Node node)
        {
            if (this.IsRed(node.Right))
            {
                node = this.RotateLeft(node);
            }

            if (this.IsRed(node.Left) && this.IsRed(node.Left.Left))
            {
                node = this.RotateRight(node);
            }

            if (this.IsRed(node.Left) && this.IsRed(node.Right))
            {
                this.FlipColors(node);
            }

            return node;
        }

        private bool IsSmaller(T a, T b)
        {
            return a.CompareTo(b) == -1;
        }

        private bool AreEqual(T a, T b)
        {
            return a.CompareTo(b) == 0;
        } 
        
        private void PreOrderCopy(Node node)
        {
            if (node == null)
            {
                return;
            }

            this.Insert(node.Value);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }
        
        private void EachInOrder(Action<T> action, Node node)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(action, node.Left);

            action.Invoke(node.Value);

            this.EachInOrder(action, node.Right);
        }
        
        private Node FindNode(T value)
        {
            var currentNode = this.root;

            while (currentNode != null)
            {
                if (this.IsSmaller(value, currentNode.Value))
                {
                    currentNode = currentNode.Left;
                }
                else if (this.IsSmaller(currentNode.Value, value))
                {
                    currentNode = currentNode.Right;
                }
                else
                {
                    break;
                }
            }

            return currentNode;
        }
        
        private Node InsertNode(Node node, T value)
        {
            if (node == null)
            {
                return new Node(value);
            }

            if (this.IsSmaller(value, node.Value))
            {
                node.Left = this.InsertNode(node.Left, value);
            }
            else
            {
                node.Right = this.InsertNode(node.Right, value);
            }
            
            if (this.IsRed(node.Right))
            {
                node = RotateLeft(node);
            }

            if (this.IsRed(node.Left) && this.IsRed(node.Left.Left))
            {
                node = this.RotateRight(node);
            }

            if (this.IsRed(node.Left) && this.IsRed(node.Right))
            {
                this.FlipColors(node);
            }

            return node;
        }
        
        private int CountNodes(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + this.CountNodes(node.Left) + this.CountNodes(node.Right);
        }
    }
}