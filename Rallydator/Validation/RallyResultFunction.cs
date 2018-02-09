using Rallydator.AIMA;

namespace Rallydator.Validation
{
    internal class RallyResultFunction : ResultFunction<RallyState, RallyAction>
    {
        public override RallyState Result(RallyState state, RallyAction action)
        {
            return new RallyState(state.SpecialStage, state.RaceResult, action.SpaceToMoveTo, action.RollUsed,
                state.CurrentRollIndex + 1, state.Damage + action.RollUsed.Damage);
        }
    }
}