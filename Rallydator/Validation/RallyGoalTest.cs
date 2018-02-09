using System.Linq;
using Rallydator.AIMA;

namespace Rallydator.Validation
{
    internal class RallyGoalTest : GoalTest<RallyState>
    {
        public override bool IsGoal(RallyState state)
        {
            return !state.CurrentSpace.GetConnectedSpaces().Any();
        }
    }
}