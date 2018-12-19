using System.Collections.Generic;

namespace Rallydator.Core
{
    public class Space
    {
        private readonly List<Space> _connectedSpaces = new List<Space>();
        private readonly List<Space> _previousSpaces = new List<Space>();

        public Space(Surface surface, int speedLimit, bool isJump, bool isDrift, string identifier)
        {
            Surface = surface;
            SpeedLimit = speedLimit;
            IsJump = isJump;
            IsDrift = isDrift;
            Identifier = identifier;
        }

        public Surface Surface { get; }
        public int SpeedLimit { get; }
        public bool IsJump { get; }
        public bool IsDrift { get; }
        public string Identifier { get; }
        public bool IsNormal => Surface != Surface.Dirt && !IsDrift;

        public IEnumerable<Space> GetConnectedSpaces()
        {
            return _connectedSpaces.AsReadOnly();
        }

        public IEnumerable<Space> GetPreviousSpaces()
        {
            return _previousSpaces.AsReadOnly();
        }

        public void AddConnectedSpace(Space connectedSpace)
        {
            _connectedSpaces.Add(connectedSpace);
            connectedSpace._previousSpaces.Add(this);
        }

        public override string ToString()
        {
            var speedLimit = SpeedLimit < int.MaxValue ? SpeedLimit.ToString() : string.Empty;
            return $"{Identifier}: {Surface}{speedLimit}";
        }
    }
}