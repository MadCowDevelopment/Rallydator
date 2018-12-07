using System.Collections.Generic;

namespace Rallydator.Core.AIMA
{
    public abstract class ActionFunction<TState, TAction>
    {
        public abstract List<TAction> Actions(TState state);
    }
}
