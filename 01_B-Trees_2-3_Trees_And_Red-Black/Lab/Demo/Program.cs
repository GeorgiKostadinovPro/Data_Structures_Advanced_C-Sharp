using System;
using System.Diagnostics.CodeAnalysis;
using _01.Two_Three;

namespace Demo
{
    public class Program
    {
        public static void Main()
        {
            var tree = new TwoThreeTree<string>();

            tree.Insert("A");
            tree.Insert("E");
            tree.Insert("D");
            tree.Insert("B");
            tree.Insert("F");
            tree.Insert("G");
            tree.Insert("C");

            Console.WriteLine(tree);
        }

        private class IntDemo : IComparable<IntDemo>
        {
            public IntDemo(int value)
            {
                this.Value = value;
            }

            public int Value { get; set; }

            public int CompareTo([AllowNull] IntDemo other)
            {
                return this.Value.CompareTo(other.Value);
            }

            public override string ToString()
            {
                return this.Value.ToString();
            }
        }
    }
}
