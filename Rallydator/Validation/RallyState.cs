using System.Linq;

namespace Rallydator.Validation
{
    public class RallyState
    {
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