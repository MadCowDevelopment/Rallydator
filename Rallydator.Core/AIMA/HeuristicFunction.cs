namespace Rallydator.Core.AIMA
{
    public abstract class HeuristicFunction<TState>
    {
        public abstract double Cost(TState state);
    }
}
