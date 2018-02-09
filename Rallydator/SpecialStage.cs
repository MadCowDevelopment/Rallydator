using System.Collections.Generic;
using System.Linq;

namespace Rallydator
{
    public class SpecialStage
    {
        public SpecialStage(IEnumerable<CourseSection> sections)
        {
            Sections = sections.ToList();

            StartSpace = new Space(Surface.Asphalt, int.MaxValue, false, "Start");
            foreach (var firstSpace in Sections.First().FirstSpaces)
            {
                StartSpace.AddConnectedSpace(firstSpace);
            }
        }

        public IEnumerable<CourseSection> Sections { get; }
        public Space StartSpace { get; }
    }
}