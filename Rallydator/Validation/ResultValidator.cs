using System.Collections.Generic;
using Rallydator.AIMA;

namespace Rallydator.Validation
{
    public class ResultValidator
    {
        private readonly Rally _rally;

        public ResultValidator(Rally rally)
        {
            _rally = rally;
        }

        public RallyValidationResult Validate()
        {
            var validationResults = new List<ValidationResult>();
            ValidationResult previousResult = null;
            SpecialStageResult previousSpecialStageResult = null;
            foreach (var specialStageResult in _rally.SpecialStages)
            {
                previousResult = ValidateSpecialStage(specialStageResult, previousSpecialStageResult, previousResult);
                validationResults.Add(previousResult);
                previousSpecialStageResult = specialStageResult;
            }

            return new RallyValidationResult(validationResults);
        }

        private ValidationResult ValidateSpecialStage(SpecialStageResult specialStageResult, SpecialStageResult previousSpecialStageResult, ValidationResult previousValidationResult)
        {
            var damage = previousSpecialStageResult?.SpecialStage.HasAssistance == true || previousValidationResult == null
                ? Damage.None
                : previousValidationResult.Damage;

            var initialState = new RallyState(specialStageResult.SpecialStage, specialStageResult.RaceResult, specialStageResult.SpecialStage.StartSpace, Roll.Start, 0, damage);
            return SolveProblem(initialState);
        }

        private ValidationResult SolveProblem(RallyState initialState)
        {
            var actionFunction = new RallyActionFunction();
            var resultFunction = new RallyResultFunction();
            var goalTest = new RallyGoalTest();
            var stepCost = new RallyStepCost();
            var searchAlgorithm = new GraphSearch<RallyAction>();
            var problem = new Problem<RallyState, RallyAction>(initialState, actionFunction, resultFunction, goalTest, stepCost);
            return searchAlgorithm.Search(problem);
        }
    }
}