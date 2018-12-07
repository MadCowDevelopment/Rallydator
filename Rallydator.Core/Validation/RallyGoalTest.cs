using System.Linq;
using Rallydator.Core.AIMA;

namespace Rallydator.Core.Validation
{
    internal class RallyGoalTest : GoalTest<RallyState>
    {
        public override bool IsGoal(RallyState state)
        {
            return !state.CurrentSpace.GetConnectedSpaces().Any();
        }
    }
}