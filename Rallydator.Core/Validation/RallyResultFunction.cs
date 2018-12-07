using Rallydator.Core.AIMA;

namespace Rallydator.Core.Validation
{
    internal class RallyResultFunction : ResultFunction<RallyState, RallyAction>
    {
        public override RallyState Result(RallyState state, RallyAction action)
        {
            if (action.RollUsed.TireChanged) state.Damage.ChangeTire();
            return new RallyState(state.SpecialStage, state.RaceResult, action.SpaceToMoveTo, action.RollUsed,
                state.CurrentRollIndex + 1, state.Damage + action.RollUsed.Damage);
        }
    }
}