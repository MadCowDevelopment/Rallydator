using System.Collections.Generic;
using System.Linq;
using Rallydator.Utils;

namespace Rallydator.Validation
{
    internal class DiceSequence
    {
        private List<Die> _usedDice = new List<Die>();
        private Damage _damage;
        private Roll _previousRoll;
        private Roll _currentRoll;

        public DiceSequence(RallyState state)
        {
            _damage = state.Damage;
            _previousRoll = state.PreviousRoll;
            _currentRoll = state.CurrentRoll;
        }

        private DiceSequence()
        {
        }

        public bool DiceAvailable => AvailableDice.Any();

        public IEnumerable<Die> AvailableDice => !_usedDice.Any() ? GetAvailableDiceForFirstSpace() : GetAvailableDiceForSubsequentSpaces();

        private IEnumerable<Die> GetAvailableDiceForFirstSpace()
        {
            var availableDice = new List<Die>();

            if (_damage.AccelerateDiceLost < 2) availableDice.Add(new AccelerateDie(_previousRoll.FinalGear));

            if (_previousRoll.FinalGear > 0) availableDice.Add(new GearDie(_previousRoll.FinalGear));
            if (_previousRoll.FinalGear > 1) availableDice.Add(new GearDie(_previousRoll.FinalGear - 1));
            if (_previousRoll.FinalGear < 5) availableDice.Add(new GearDie(_previousRoll.FinalGear + 1));

            return availableDice;
        }

        private IEnumerable<Die> GetAvailableDiceForSubsequentSpaces()
        {
            var availableDice = new List<Die>();
            if (IsTimeAttack && AllTimeAttackDiceUsed) return availableDice;

            if (_usedDice.Count(p => p is AccelerateDie) + availableDice.Count(p => p is AccelerateDie) <
                2 - _damage.AccelerateDiceLost)
            {
                availableDice.Add(new AccelerateDie(LastUsedGear));
            }

            if (!IsInFinalGear)
            {
                var direction = GetDirection();
                if (direction != Direction.Up) availableDice.AddNotNull(GetNextLowerGear());
                if (direction != Direction.Down) availableDice.AddNotNull(GetNextHigherGear());
            }

            return availableDice;
        }

        private Die GetNextLowerGear()
        {
            return LastUsedGear > 1 ? new GearDie(LastUsedGear - 1) : null;
        }

        private Die GetNextHigherGear()
        {
            return LastUsedGear < 5 ? new GearDie(LastUsedGear + 1) : null;
        }

        private bool IsInFinalGear => _usedDice.OfType<GearDie>().LastOrDefault()?.Gear == _currentRoll.FinalGear;
        private int LastUsedGear => _usedDice.Last().Gear;
        public bool IsCompleteRoll => IsInFinalGear && (!IsTimeAttack || AllTimeAttackDiceUsed);
        private bool IsTimeAttack => _currentRoll.TimeAttack > 0;
        private bool AllTimeAttackDiceUsed => _currentRoll.TimeAttack == _usedDice.Count;

        private Direction GetDirection()
        {
            var usedGearDice = _usedDice.OfType<GearDie>().ToList();
            if (usedGearDice.Count < 2)
            {
                return Direction.Undefined;
            }

            if (usedGearDice[0].Gear < usedGearDice[1].Gear) return Direction.Up;
            return Direction.Down;
        }

        private enum Direction
        {
            Undefined,
            Up,
            Down
        }

        public DiceSequence AddDie(Die die)
        {
            var clone = new DiceSequence();
            clone._usedDice = new List<Die>(_usedDice);
            clone._usedDice.Add(die);
            clone._damage = _damage;
            clone._previousRoll = _previousRoll;
            clone._currentRoll = _currentRoll;
            return clone;
        }


        public override string ToString()
        {
            return $"Used:{string.Join(",", _usedDice)} --- Available:{string.Join(",", AvailableDice)}";
        }
    }
}