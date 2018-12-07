using System.Collections.Generic;
using System.Linq;

namespace Rallydator.Core
{
    public static class SpecialStageFactory
    {
        public static SpecialStage Create(string courseLayout, bool hasAssistance)
        {
            var sections = CreateSections(courseLayout).ToList();
            if (sections.Count > 1)
            {
                MergeSections(sections);
            }

            var goalSpace = new Space(Surface.Asphalt, int.MaxValue, false, "Finish");
            foreach (var lastSpace in sections.Last().LastSpaces)
            {
                lastSpace.AddConnectedSpace(goalSpace);
            }

            return new SpecialStage(sections, hasAssistance);
        }

        private static void MergeSections(List<CourseSection> sections)
        {
            for (var i = 0; i < sections.Count - 1; i++)
            {
                var section1 = sections[i];
                var section2 = sections[i + 1];

                foreach (var section1LastSpace in section1.LastSpaces)
                {
                    foreach (var section2FirstSpace in section2.FirstSpaces)
                    {
                        section1LastSpace.AddConnectedSpace(section2FirstSpace);
                    }
                }
            }
        }

        private static IEnumerable<CourseSection> CreateSections(string courseLayout)
        {
            return courseLayout.Split(':').Select(CreateSection).ToList();
        }

        private static CourseSection CreateSection(string section)
        {
            var zone = section.Split('-')[0].Substring(0, 1);
            var start = int.Parse(section.Split('-')[0].Substring(1));
            var end = int.Parse(section.Split('-')[1].Substring(1));

            return CourseSectionFactory.Create(zone, start, end);
        }
    }
}