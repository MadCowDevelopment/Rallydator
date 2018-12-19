using System.Collections.Generic;
using System.Linq;
using Rallydator.Core.AIMA;
using Rallydator.Core.Utils;

namespace Rallydator.Core.Validation
{
    internal class RallyActionFunction : ActionFunction<RallyState, RallyAction>
    {
        public override List<RallyAction> Actions(RallyState state)
        {
            var possibleActions = new List<RallyAction>();
            if (state.CurrentRoll == null) return possibleActions;

            var possibleTargetSpaces = CalculateTargetSpaces(state);
            foreach (var possibleTargetSpace in possibleTargetSpaces)
            {
                possibleActions.Add(new RallyAction(possibleTargetSpace, state.CurrentRoll));
            }

            return possibleActions;
        }

        private IEnumerable<Space> CalculateTargetSpaces(RallyState state)
        {
            var targetSpaces = new List<Space>();
            CalculateTargetSpacesRec(targetSpaces, state.CurrentSpace, new DiceSequence(state), 0);
            return targetSpaces.Distinct().ToList();
        }

        private void CalculateTargetSpacesRec(List<Space> targetSpaces, Space previousSpace, DiceSequence diceSequence, int depth)
        {
            if (diceSequence.IsCompleteRoll)
            {
                targetSpaces.AddDistinct(previousSpace);
            }

            if (diceSequence.DiceAvailable)
            {
                foreach (var possibleDie in diceSequence.AvailableDice)
                {
                    foreach (var connectedSpace in previousSpace.GetConnectedSpaces())
                    {
                        var nextSpace = connectedSpace;
                        if (connectedSpace.IsJump)
                        {
                            if (possibleDie.Gear < connectedSpace.SpeedLimit)
                            {
                                nextSpace = connectedSpace;
                            }
                            else if (possibleDie.Gear == connectedSpace.SpeedLimit)
                            {
                                nextSpace = connectedSpace.GetConnectedSpaces().First();
                            }
                            else if (possibleDie.Gear == connectedSpace.SpeedLimit + 1)
                            {
                                nextSpace = connectedSpace.GetConnectedSpaces().First().GetConnectedSpaces().First();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (possibleDie.Gear > connectedSpace.SpeedLimit) continue;

                        }

                        CalculateTargetSpacesRec(targetSpaces, nextSpace, diceSequence.AddDie(possibleDie), depth+1);
                    }
                }
            }
        }
    }
}