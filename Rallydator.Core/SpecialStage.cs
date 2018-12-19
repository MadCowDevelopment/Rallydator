using System.Collections.Generic;
using System.Linq;

namespace Rallydator.Core
{
    public class SpecialStage
    {
        public SpecialStage(IEnumerable<CourseSection> sections, bool hasAssistance)
        {
            HasAssistance = hasAssistance;
            Sections = sections.ToList();

            StartSpace = new Space(Surface.Asphalt, int.MaxValue, false, false, "Start");
            foreach (var firstSpace in Sections.First().FirstSpaces)
            {
                StartSpace.AddConnectedSpace(firstSpace);
            }
        }

        public IEnumerable<CourseSection> Sections { get; }
        public Space StartSpace { get; }

        public bool HasAssistance { get; }
    }
}