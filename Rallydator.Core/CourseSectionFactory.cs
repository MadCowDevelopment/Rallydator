using System;
using System.Collections.Generic;
using System.Linq;

namespace Rallydator.Core
{
    public static class CourseSectionFactory
    {
        private static readonly Dictionary<Tuple<string, int>, string> _descriptions =
            new Dictionary<Tuple<string, int>, string>();

        static CourseSectionFactory()
        {
            InitializeZones();
        }

        public static CourseSection Create(string zone, int start, int end)
        {
            var min = Math.Min(start, end);
            var direction = start < end ? Direction.Normal : Direction.Reverse;
            return new CourseSection($"{zone}{start}-{zone}{end}",
                CreateSection(_descriptions[new Tuple<string, int>(zone, min)], direction, zone, start));
        }

        private static IEnumerable<Space> CreateSection(string sectionDescription, Direction direction, string zone, int start)
        {
            var zoneIdentifier = zone + start;
            var spaceDescriptions = sectionDescription.Split('|');
            if (direction == Direction.Reverse) spaceDescriptions = spaceDescriptions.Reverse().ToArray();

            var startSpaces = new List<Space>();
            var previousSpaces = new List<Space>();
            int spaceIdentifier = 0;
            for (var i = 0; i < spaceDescriptions.Length; i++)
            {
                spaceIdentifier++;
                var currentSpaces = new List<Space>();
                var endSpaces = new List<Space>();
                
                var currentSpaceDescription = spaceDescriptions[i];
                var currentSubSpaceDescriptions = currentSpaceDescription.Split(':');

                for (var j = 0; j < currentSubSpaceDescriptions.Length; j++)
                {
                    Space previousSubSpace = null;
                    var currentSubSpace = currentSubSpaceDescriptions[j];
                    var singleSpaces = currentSubSpace.Split(',');
                    for (int k = 0; k < singleSpaces.Length; k++)
                    {
                        var singleSpace = singleSpaces[k];

                        var surface = GetSurface(singleSpace);
                        var speedLimit = GetSpeedLimit(singleSpace);
                        var isJump = GetIsJump(singleSpace);
                        var isDrift = j != 0;
                        var driftIdentifier = isDrift ? $".{k+1}" : string.Empty;
                        var space = new Space(surface, speedLimit, isJump, isDrift, $"{zoneIdentifier}.{spaceIdentifier}{driftIdentifier}");
                        
                        if (i == 0 && k == 0) startSpaces.Add(space);
                        if (k == 0) currentSpaces.Add(space);
                        if (k > 0) previousSubSpace.AddConnectedSpace(space);
                        if (j > 0) space.AddConnectedSpace(currentSpaces[0]);
                        if (k + 1 == singleSpaces.Length) endSpaces.Add(space);

                        previousSubSpace = space;
                    }
                }
                
                foreach (var previousSpace in previousSpaces)
                {
                    Space spaceToAdd = previousSpace;
                    while (spaceToAdd.GetConnectedSpaces().Any())
                    {
                        spaceToAdd = spaceToAdd.GetConnectedSpaces().First();
                    }

                    foreach (var currentSpace in currentSpaces)
                    {
                        previousSpace.AddConnectedSpace(currentSpace);
                    }
                }

                previousSpaces = new List<Space>(endSpaces);
            }

            return startSpaces;
        }

        private static Surface GetSurface(string singleSpace)
        {
            var identifier = singleSpace[0];
            switch (identifier)
            {
                case 'A': return Surface.Asphalt;
                case 'S': return Surface.Snow;
                case 'D': return Surface.Dirt;
                default: throw new InvalidOperationException("No valid surface identifier for space.");
            }
        }

        private static int GetSpeedLimit(string singleSpace)
        {
            var limit = singleSpace[1]-48;
            return limit == 0 ? int.MaxValue : limit;
        }

        private static bool GetIsJump(string singleSpace)
        {
            if (singleSpace.Length < 3) return false;
            return singleSpace[2] == 'J';
        }

        private static void InitializeZones()
        {
            _descriptions.Add(new Tuple<string, int>("C", 0), "A0|A0|A3:A4,A4,A4|A0");
            _descriptions.Add(new Tuple<string, int>("C", 1), "A4:A5,A5,A5|A0|A0|A3:A4,A4,A4|A0|A2:A3,A3,A3|A0|A0|A2:A3,A3,A3:D3|A0|A0|A0|A4:A5,A5,A5|A0|A0|A0|A0|A2:A3,A3,A3|A0");
            _descriptions.Add(new Tuple<string, int>("C", 3), "A0|A2:A3,A3,A3|A0|A0|A1:A2,A2,A2:D2|A0|A1:A2,A2,A2:D2|A0|A0|A1:A2,A2,A2|A0|A0|A0|A0|A2:A3,A3,A3|A0|A0|A0|A0|A2:A3,A3,A3:D3|A0|A0|A4J|A0|A0");
            _descriptions.Add(new Tuple<string, int>("C", 4), "A0|A0|A0|A0|A0|A1:A2,A2,A2|A0|A3:A4,A4,A4|A0");
            _descriptions.Add(new Tuple<string, int>("C", 6), "A2:A3,A3,A3|A0|A0|A0|A0|A3:A4,A4,A4");

            _descriptions.Add(new Tuple<string, int>("C", 10), "S0|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("C", 11), "S4:S5,S5|S4:S5,S5|S0|S0|S3:S4,S4|S3:S4,S4|S0|S1:S2,S2,S2|S0|S0|S1:S2,S2,S2|S0|S0|S0|S4:S5,S5|S4:S5,S5|S0|S0|S0|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("C", 13), "S0|S2:S3,S3|S2:S3,S3|S0|S0|S1:S2,S2,S2,S2|S0|S1:S2,S2,S2|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S0|S2:S3,S3|S2:S3,S3|S0|S0|S0|S0|S2:S3,S3|S2:S3,S3|S0|S0|S4J|S0|S0");
            _descriptions.Add(new Tuple<string, int>("C", 14), "S0|S0|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("C", 16), "S2:S3,S3|S2:S3,S3|S0|S0|S0|S0|S3:S4,S4|S3:S4,S4");

            _descriptions.Add(new Tuple<string, int>("J", 0), "A0|A0|A0|A0|A1:A2,A2,A2|A0|A0|A2:A3,A3,A3:D3|A0");
            _descriptions.Add(new Tuple<string, int>("J", 1), "A4:A5,A5,A5|A0|A0|A3:A4,A4,A4|A0|A0|A0|A1:A2,A2,A2|A1:A2,A2,A2|A0|A0|A0|A4:A5,A5,A5|A0|A0|A0|A0|A0|A1:A2,A2,A2|A0|A0|A0|A1:A2,A2,A2|A0|A0|A0|A0");
            _descriptions.Add(new Tuple<string, int>("J", 2), "A0|A3:A4,A4|A0");
            _descriptions.Add(new Tuple<string, int>("J", 5), "A0|A2:A3,A3,A3:D3|A0|A0|A0|A3:A4,A4,A4:D4|A0");
            _descriptions.Add(new Tuple<string, int>("J", 7), "A4:A5,A5,A5|A0|A0|A1:A2,A2,A2:D2|A0|A0|A4:A5,A5,A5");

            _descriptions.Add(new Tuple<string, int>("J", 10), "S0|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S1:S2,S2,S2|S0");
            _descriptions.Add(new Tuple<string, int>("J", 11), "S4:S5,S5,S5|S0|S0|S3:S4,S4|S3:S4,S4|S0|S0|S0|S1:S2,S2|S1:S2,S2|S1:S2,S2|S0|S0|S0|S4:S5,S5|S4:S5,S5|S0|S0|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S0");
            _descriptions.Add(new Tuple<string, int>("J", 12), "S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("J", 15), "S0|S1:S2,S2,S2|S0|S0|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("J", 17), "S3:S4,S4,S4|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S3:S4,S4");

            _descriptions.Add(new Tuple<string, int>("L", 0), "A0|A2:A3,A3,A3|A0|A0|A0|A3:A4,A4,A4:D4|A0");
            _descriptions.Add(new Tuple<string, int>("L", 2), "A0|A2:A3,A3,A3:D3:A0|A0|A0|A0|A1:A2,A2,A2|A0|A0|A0|A0|A4J|A0|A0");
            _descriptions.Add(new Tuple<string, int>("L", 4), "A0|A0|A4:A5,A5,A5:D5|A0|A0|A3:A4,A4:D4|A0|A0|A4J|A0|A0|A0|A0|A0");
            _descriptions.Add(new Tuple<string, int>("L", 5), "A4:A5,A5,A5|A0|A0|A4:A5,A5,A5:D5|A0|A0|A0|A1:A2,A2,A2|A0|A0");
            _descriptions.Add(new Tuple<string, int>("L", 8), "A0|A2:A3,A3,A3|A0|A0|A1:A2,A2,A2|A0|A0|A0|A1:A2,A2,A2|A0|A0|A0|A0|A0");

            _descriptions.Add(new Tuple<string, int>("L", 10), "S0|S2:S3,S3|S2:S3,S3|S0|S0|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("L", 12), "S0|S2:S3,S3|S2:S3,S3|S0|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S4J|S0|S0");
            _descriptions.Add(new Tuple<string, int>("L", 14), "S0|S0|S4:S5,S5|S4:S5,S5|S0|S0|S3:S4,S4|S3:S4,S4|S0|S0|S4J|S0|S0|S0|S0|S0");
            _descriptions.Add(new Tuple<string, int>("L", 15), "S3:S4,S4,S4|S0|S0|S3:S4,S4,S4|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0");
            _descriptions.Add(new Tuple<string, int>("L", 18), "S0|S2:S3,S3|S2:S3,S3|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0|S0|S0");

            _descriptions.Add(new Tuple<string, int>("V", 0), "S0|S4:S5,S5|S4:S5,S5|S0|S0|S0|S3J|S0|S0|S0|S0|S3:S4,S4|S3:S4,S4|S0");
            _descriptions.Add(new Tuple<string, int>("V", 1), "S0|S0|S4:S5,S5|S4:S5,S5|S0|S0|S0|S0");
            _descriptions.Add(new Tuple<string, int>("V", 2), "S0|S0|S0|S1:S2,S2|S1:S2,S2|S0|S0|S0");
            _descriptions.Add(new Tuple<string, int>("V", 6), "S0|S0|S3:S4,S4|S3:S4,S4|S0|S0|S3:S4,S4|S3:S4,S4|S0|S0|S0|S2:S3,S3|S2:S3,S3|S0|S0");
            _descriptions.Add(new Tuple<string, int>("V", 7), "S0|S3:S4,S4|S3:S4,S4|S0");

            _descriptions.Add(new Tuple<string, int>("V", 10), "A0|A4:A5,A5:D5|A0|A0|A0|A3J|A0|A0|A0|A0|A3:A4,A4,A4:D4|A0");
            _descriptions.Add(new Tuple<string, int>("V", 11), "A0|A0|A4:A5,A5|A0|A0|A0|A0");
            _descriptions.Add(new Tuple<string, int>("V", 12), "A0|A0|A0|A1:A2,A2,A2:D2|A0|A0|A0");
            _descriptions.Add(new Tuple<string, int>("V", 16), "A0|A0|A3:A4,A4,A4:D4|A0|A0|A3:A4,A4,A4:D4|A0|A0|A0|A2:A3,A3,A3:D3|A0|A0");
            _descriptions.Add(new Tuple<string, int>("V", 17), "A0|A3:A4,A4|A3:A4,A4|A0");

        }
    }
}