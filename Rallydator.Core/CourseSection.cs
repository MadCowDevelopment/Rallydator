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
            InitializeLastSpaces(FirstSpaces);
        }

        public IEnumerable<Space> FirstSpaces { get; }

        public IEnumerable<Space> LastSpaces => _lastSpaces.AsReadOnly();

        public string Name { get; }

        private void InitializeLastSpaces(IEnumerable<Space> spaces)
        {
            foreach (var space in spaces)
            {
                var connectedSpaces = space.GetConnectedSpaces().ToList();
                if (!connectedSpaces.Any()) _lastSpaces.AddDistinct(space);
                else InitializeLastSpaces(connectedSpaces);
            }
        }
    }
}