using System;
using System.Collections.Generic;
using System.Linq;

namespace Rallydator.Helpers
{
    internal static class SpecialStagePrinter
    {
        public static void Print(SpecialStage specialStage)
        {
            Console.Clear();
            var top = 0;
            foreach (var section in specialStage.Sections)
            {
                Console.WriteLine($"Section name: {section.Name}");
                PrintSection(section, 1);
                Console.ReadKey(false);
                Console.Clear();
            }
        }

        private static void PrintSection(CourseSection section, int top)
        {
            PrintSpaces(section.FirstSpaces, 0, top);

            Console.SetCursorPosition(0, 10);
        }

        private static void PrintSpaces(IEnumerable<Space> initialSpaces, int left, int top)
        {
            var spaces = initialSpaces.ToList();
            if (spaces.Count == 1)
            {
                var space = spaces.ElementAt(0);
                Console.SetCursorPosition(left, top + 2);
                Console.Write(space);
                left += 2;

                var connectedSpaces = space.GetConnectedSpaces().ToList();
                if (connectedSpaces.Count == 1)
                {
                    Console.Write("-");
                    left++;
                }
                else if (connectedSpaces.Count == 2)
                {
                    Console.SetCursorPosition(left, top + 1);
                    Console.Write("/");
                    Console.SetCursorPosition(left, top + 3);
                    Console.Write("\\");
                    left++;
                }

                PrintSpaces(connectedSpaces, left, top);
            }
            else if (spaces.Count == 2)
            {
                var singleSpace = spaces[0];
                Console.SetCursorPosition(left, top + 0);
                Console.Write(singleSpace);

                var multipleSpace = spaces[1];

                do
                {
                    Console.SetCursorPosition(left, top + 4);
                    Console.Write(multipleSpace);
                    Console.Write("-");

                    Console.SetCursorPosition(left + 2, top + 0);
                    Console.Write("--");

                    var temp = multipleSpace.GetConnectedSpaces().FirstOrDefault();
                    if (temp == null) break;
                    multipleSpace = multipleSpace.GetConnectedSpaces().First();
                    left += 3;
                } while (!HaveSameConnectedSpaces(singleSpace, multipleSpace));

                Console.SetCursorPosition(left, top + 4);
                Console.Write(multipleSpace);
                Console.Write("-");

                Console.SetCursorPosition(left + 1, top + 0);
                Console.Write("--");

                left += 2;

                if (singleSpace.GetConnectedSpaces().Count() == 1 && multipleSpace.GetConnectedSpaces().Count() == 1)
                {
                    Console.SetCursorPosition(left, top + 1);
                    Console.Write("\\");
                    Console.SetCursorPosition(left, top + 3);
                    Console.Write("/");
                    left++;
                }
                else if (singleSpace.GetConnectedSpaces().Count() == 2 &&
                         multipleSpace.GetConnectedSpaces().Count() == 2)
                {
                    Console.SetCursorPosition(left, top + 0);
                    Console.Write("---");
                    Console.SetCursorPosition(left, top + 4);
                    Console.Write("---");
                    Console.SetCursorPosition(left, top + 1);
                    Console.Write("\\ /");
                    Console.SetCursorPosition(left, top + 3);
                    Console.Write("/ \\");
                    Console.SetCursorPosition(left + 1, top + 2);
                    Console.Write("X");
                    left += 3;
                }

                PrintSpaces(new List<Space>(singleSpace.GetConnectedSpaces()), left, top);
            }
        }

        private static bool HaveSameConnectedSpaces(Space space1, Space space2)
        {
            var s1Connections = space1.GetConnectedSpaces().ToList();
            var s2Connections = space2.GetConnectedSpaces().ToList();

            if (s1Connections.Count != s2Connections.Count) return false;
            return !s1Connections.Where((t, i) => t != s2Connections[i]).Any();
        }
    }
}
