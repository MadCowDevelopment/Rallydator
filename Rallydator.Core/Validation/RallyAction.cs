namespace Rallydator.Core.Validation
{
    internal class RallyAction
    {
        public RallyAction(Space spaceToMoveTo, Roll rollUsed)
        {
            SpaceToMoveTo = spaceToMoveTo;
            RollUsed = rollUsed;
        }

        public Space SpaceToMoveTo { get; }
        public Roll RollUsed { get; }
    }
}