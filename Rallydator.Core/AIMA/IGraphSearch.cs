namespace Rallydator.Core.AIMA
{
    public interface IGraphSearch<TState, TAction, out TResult>
    {
        TResult Search(Problem<TState, TAction> problem);
    }
}