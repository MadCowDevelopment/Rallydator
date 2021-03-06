﻿namespace Rallydator.Core.AIMA
{
    public abstract class StepCost<TState, TAction>
    {
        public abstract double Cost(TState state, TAction action);
    }
}
