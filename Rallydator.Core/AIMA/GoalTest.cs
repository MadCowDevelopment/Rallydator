namespace Rallydator.Core.AIMA
{
    public abstract class GoalTest<TState>
    {
        public abstract bool IsGoal(TState state);
    }
}
