using System;
using System.Collections.Generic;
using System.Linq;

namespace WordCruncher
{
    public class Program
    {
        public static void Main()
        {
            string[] words = Console.ReadLine()
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string targetString = Console.ReadLine();

            WordCruncher wordCruncher = new WordCruncher(words, targetString);

            foreach (var path in wordCruncher.GetSyllablePaths())
            {
                Console.WriteLine(string.Join(" ", path));
            }
        }
    }

    public class WordCruncher
    {
        private class Node
        {
            public Node(string syllable, IList<Node> nextSyllables)
            {
                this.Syllable = syllable;
                this.NextSyllables = nextSyllables;
            }

            public string Syllable { get; }

            public IList<Node> NextSyllables { get; }
        }

        private readonly IList<Node> syllableGroups;

        public WordCruncher(string[] words, string targetString)
        {
            this.syllableGroups = this.GenerateSyllableGroups(words, targetString);
        }

        private IList<Node> GenerateSyllableGroups(string[] words, string targetString)
        {
            if (string.IsNullOrEmpty(targetString)
                || words.Length == 0)
            {
                return null;
            }

            var resultValues = new List<Node>();

            for (int i = 0; i < words.Length; i++)
            {
                var syllable = words[i];

                if (targetString.StartsWith(syllable))
                {
                    var nextSyllables = this.GenerateSyllableGroups(
                        words.Where((_, index) => index != i).ToArray(),
                        targetString.Substring(syllable.Length));
                        
                    Node node = new Node(syllable, nextSyllables);
                    resultValues.Add(node);
                }
            }

            return resultValues;
        }

        public IEnumerable<string> GetSyllablePaths()
        {
            var allPaths = new List<List<string>>();

            this.GeneratePaths(this.syllableGroups, new List<string>(), allPaths);

            return new HashSet<string>(allPaths.Select(path => string.Join(" ", path)));
        }

        private void GeneratePaths(IList<Node> syllableGroups, List<string> currentPath, List<List<string>> allPaths)
        {
            if (syllableGroups == null)
            {
                allPaths.Add(new List<string>(currentPath));
                return;
            }

            foreach (var node in syllableGroups)
            {
                currentPath.Add(node.Syllable);

                this.GeneratePaths(node.NextSyllables, currentPath, allPaths);

                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
    } 
}
