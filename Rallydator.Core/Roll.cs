using System;
using System.Text;

namespace Rallydator.Core
{
    public class Roll
    {
        public bool TireChanged { get; private set; }
        public int FinalGear { get; private set; }
        public int TimeAttack { get; private set; }
        public int SpentSeconds { get; private set; }
        public bool LostControl { get; private set; }
        public bool SisuUsed { get; private set; }
        public Damage Damage { get; private set; }

        public static Roll Start => new Roll();

        public static Roll Parse(string roll)
        {
            var index = 0;
            var result = new Roll();

            result.TireChanged = ParseTireChanged(roll, ref index);
            result.FinalGear = ParseFinalGear(roll, ref index);
            result.TimeAttack = ParseTimeAttack(roll, ref index);
            if (result.TimeAttack == 0) result.SpentSeconds = ParseSpentSeconds(roll, ref index);
            result.LostControl = ParseLostControl(roll, ref index);
            result.SisuUsed = ParseSisuUsed(roll, ref index);
            result.Damage = ParseDamage(roll, ref index);

            return result;
        }

        private static bool ParseTireChanged(string roll, ref int index)
        {
            if (roll.Length < 1) ThrowError(roll, "is empty");
            if (roll[0] != 'T') return false;

            index++;
            return true;
        }

        private static int ParseFinalGear(string roll, ref int index)
        {
            if (roll.Length <= index) ThrowError(roll, "does not contain final gear information");
            if (!int.TryParse(roll[index].ToString(), out var finalGear)) ThrowError(roll, "does not contain final gear information");
            if (finalGear < 0 || finalGear > 5) ThrowError(roll, $"has invalid final gear '{finalGear}'");

            index++;
            return finalGear;
        }

        private static int ParseTimeAttack(string roll, ref int index)
        {
            if (roll.Length + index < 3 || roll[index] != '(' || roll[index + 2] != ')') return 0;
            if (!int.TryParse(roll[index + 1].ToString(), out var timeAttack)) return 0;
            if (timeAttack < 2 || timeAttack > 7) ThrowError(roll, $"has invalid time attack '{timeAttack}'.");

            index += 3;
            return timeAttack;
        }

        private static int ParseSpentSeconds(string roll, ref int index)
        {
            if (roll.Length + index < 4 || roll[index] != '(' || roll[index + 1] != '-' || (roll[index + 3] != ')' && roll[index + 4] != ')')) return 0;
            var indexOfClosingBrace = roll.IndexOf(')', index);
            var number = roll.Substring(index + 1, indexOfClosingBrace - index);
            if (!int.TryParse(number, out var spentSeconds)) return 0;
            if (spentSeconds != 1 && spentSeconds != 3 && spentSeconds != 6 && spentSeconds != 10 &&
                spentSeconds != 15 &&
                spentSeconds != 21) ThrowError(roll, $"has invalid number of spent seconds '{spentSeconds}'.");

            index += indexOfClosingBrace == index + 3 ? 4 : 5;
            return spentSeconds;
        }

        private static bool ParseLostControl(string roll, ref int index)
        {
            if (roll.Length <= index) return false;
            if (roll[index] != '!') return false;

            index++;
            return true;
        }

        private static bool ParseSisuUsed(string roll, ref int index)
        {
            if (roll.Length <= index) return false;
            if (roll[index] != 'S') return false;

            index++;
            return true;
        }

        private static Damage ParseDamage(string roll, ref int index)
        {
            if (roll.Length <= index + 2) return Damage.None;
            if (roll[index] != '-') return Damage.None;
            if (!int.TryParse(roll[index + 1].ToString(), out var numberOfLostDice)) ThrowError(roll, "cannot parse number of lost dice");
            if (numberOfLostDice < 1 || numberOfLostDice > 2) ThrowError(roll, $"has invalid number of lost '{numberOfLostDice}'");
            if (roll[index + 2] == 'G') return new Damage(numberOfLostDice, 0);
            if (roll[index + 2] == 'A') return new Damage(0, numberOfLostDice);

            ThrowError(roll, "has invalid damage type");
            return null; // Never reached because exception thrown before.
        }

        private static void ThrowError(string roll, string error)
        {
            throw new InvalidOperationException($"Roll '{roll}' {error}.");
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (TireChanged) builder.Append("T");

            builder.Append(FinalGear);

            if (TimeAttack > 0) builder.Append($"({TimeAttack})");
            else if (SpentSeconds > 0) builder.Append($"(-{SpentSeconds})");

            if (LostControl)
            {
                builder.Append("!");
                if (SisuUsed) builder.Append("S");
                else builder.Append(Damage);
            }

            return builder.ToString();
        }
    }
}
