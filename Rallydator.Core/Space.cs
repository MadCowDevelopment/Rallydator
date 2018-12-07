using System.Collections.Generic;

namespace Rallydator.Core
{
    public class Space
    {
        private readonly List<Space> _connectedSpaces = new List<Space>();

        public Space(Surface surface, int speedLimit, bool isJump, string identifier)
        {
            Surface = surface;
            SpeedLimit = speedLimit;
            IsJump = isJump;
            Identifier = identifier;
        }

        public Surface Surface { get; }
        public int SpeedLimit { get; }
        public bool IsJump { get; }
        public string Identifier { get; }

        public IEnumerable<Space> GetConnectedSpaces()
        {
            return _connectedSpaces.AsReadOnly();
        }

        public void AddConnectedSpace(Space connectedSpace)
        {
            _connectedSpaces.Add(connectedSpace);
        }

        public override string ToString()
        {
            var speedLimit = SpeedLimit < int.MaxValue ? SpeedLimit.ToString() : string.Empty;
            return $"{Identifier}: {Surface}{speedLimit}";
        }
    }
}