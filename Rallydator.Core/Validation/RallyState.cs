using System;
using System.Linq;

namespace Rallydator.Core.Validation
{
    public class RallyState : IEquatable<RallyState>
    {
        public bool Equals(RallyState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(CurrentSpace, other.CurrentSpace) && Equals(PreviousRoll, other.PreviousRoll) && CurrentRollIndex == other.CurrentRollIndex && Equals(Damage, other.Damage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RallyState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (CurrentSpace != null ? CurrentSpace.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PreviousRoll != null ? PreviousRoll.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CurrentRollIndex;
                hashCode = (hashCode * 397) ^ (Damage != null ? Damage.GetHashCode() : 0);
                return hashCode;
            }
        }

        public RallyState(SpecialStage specialStage, SpecialStageRolls raceResult, Space currentSpace, Roll previousRoll, int currentRollIndex, Damage damage)
        {
            SpecialStage = specialStage;
            RaceResult = raceResult;
            CurrentSpace = currentSpace;
            PreviousRoll = previousRoll;
            CurrentRollIndex = currentRollIndex;
            Damage = damage;
        }

        public SpecialStage SpecialStage { get; }
        public SpecialStageRolls RaceResult { get; }
        public Space CurrentSpace { get; }
        public Roll PreviousRoll { get; }
        public int CurrentRollIndex { get; }
        public Roll CurrentRoll => CurrentRollIndex < RaceResult.Rolls.Count() ? RaceResult.Rolls.ElementAt(CurrentRollIndex) : null;
        public Damage Damage { get; }
    }
}