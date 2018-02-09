using System.Linq;
using Rallydator.AIMA;

namespace Rallydator.Validation
{
    public class ResultValidator
    {
        private readonly SpecialStage _specialStage;
        private readonly RaceResult _raceResult;

        public ResultValidator(SpecialStage specialStage, RaceResult raceResult)
        {
            _specialStage = specialStage;
            _raceResult = raceResult;
        }

        public ValidationResult Validate()
        {
            var result = ValidateSpecialStage();
            return result;
        }

        private ValidationResult ValidateSpecialStage()
        {
            var actionFunction = new RallyActionFunction();
            var resultFunction = new RallyResultFunction();
            var goalTest = new RallyGoalTest();
            var stepCost = new RallyStepCost();

            var initialState = new RallyState(_specialStage, _raceResult, _specialStage.StartSpace, Roll.Start, 0, Damage.None);
            var searchAlgorithm = new GraphSearch<RallyState, RallyAction>();

            return SolveProblem(actionFunction, resultFunction, goalTest, stepCost, initialState, searchAlgorithm);
        }

        private TResult SolveProblem<TState, TAction, TResult>(
            ActionFunction<TState, TAction> actionFunction,
            ResultFunction<TState, TAction> resultFunction,
            GoalTest<TState> goalTest,
            StepCost<TState, TAction> stepCost,
            TState initialState,
            IGraphSearch<TState, TAction, TResult> searchAlgorithm)
        {
            var problem = new Problem<TState, TAction>(
                initialState, actionFunction, resultFunction, goalTest, stepCost);

            var result = searchAlgorithm.Search(problem);
            return result;
        }
    }
}