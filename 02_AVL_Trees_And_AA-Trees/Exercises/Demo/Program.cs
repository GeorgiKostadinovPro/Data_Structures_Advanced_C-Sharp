using System;
using AVLTree;

namespace Demo
{
    public class Program
    {
        public static void Main()
        {
            var tree = new AVL<int>();

            tree.Insert(41);
            tree.Insert(20);
            tree.Insert(65);
            tree.Insert(11);
            tree.Insert(50);
            tree.Insert(29);
            tree.Insert(91);
            tree.Insert(32);
            tree.Insert(72);
            tree.Insert(99);

            tree.Delete(50);
            tree.Delete(99);
            tree.Delete(65);
            tree.Delete(72);

            tree.EachInOrder(Console.WriteLine);
        }
    }
}
