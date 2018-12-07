using Rallydator.Core.AIMA;

namespace Rallydator.Core.Validation
{
    internal class RallyStepCost : StepCost<RallyState, RallyAction>
    {
        public override double Cost(RallyState state, RallyAction action)
        {
            return 1;
        }
    }
}