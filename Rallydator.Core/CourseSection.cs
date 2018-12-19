using System;
using System.Collections.Generic;
using System.Linq;
using Rallydator.Core.Utils;

namespace Rallydator.Core
{
    public class CourseSection
    {
        private readonly List<Space> _lastSpaces = new List<Space>();

        public CourseSection(string name, IEnumerable<Space> firstSpaces)
        {
            Name = name;
            FirstSpaces = firstSpaces;
            InitializeLastSpaces();
        }

        public IEnumerable<Space> FirstSpaces { get; }

        public IEnumerable<Space> LastSpaces => _lastSpaces.AsReadOnly();

        public string Name { get; }

        private void InitializeLastSpaces()
        {
            var spacesWithoutChildren = FindSpacesWithoutChildren();
            _lastSpaces.AddRange(spacesWithoutChildren);
            foreach (var lastSpace in _lastSpaces.ToList())
            {
                var lastDriftSpace = lastSpace.GetPreviousSpaces().LastOrDefault(p => p.IsDrift);
                _lastSpaces.AddNotNull(lastDriftSpace);
            }
        }

        private List<Space> FindSpacesWithoutChildren()
        {
            var result = new List<Space>();
            Stack<Space> frontier = new Stack<Space>();
            HashSet<Space> explored = new HashSet<Space>();

            foreach (var firstSpace in FirstSpaces)
            {
                frontier.Push(firstSpace);
            }

            for (; ; )
            {
                if (frontier.Count == 0)
                {
                    break;
                }

                var leaf = frontier.Pop();
                if (!leaf.GetConnectedSpaces().Any())
                {
                    result.Add(leaf);
                }

                explored.Add(leaf);

                var children = leaf.GetConnectedSpaces();
                foreach (var child in children)
                {
                    var child1 = child;
                    if (frontier.All(p => p != child1) &&
                        explored.All(p => p != child1))
                    {
                        frontier.Push(child);
                    }
                }
            }

            return result;
        }
    }
}