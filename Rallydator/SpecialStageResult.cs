namespace Rallydator
{
    public class SpecialStageResult
    {
        public SpecialStageResult(SpecialStage specialStage, SpecialStageRolls raceResult)
        {
            SpecialStage = specialStage;
            RaceResult = raceResult;
        }

        public SpecialStage SpecialStage { get; }
        public SpecialStageRolls RaceResult { get; }
    }
}