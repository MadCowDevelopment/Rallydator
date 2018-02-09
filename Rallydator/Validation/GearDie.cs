using System;

namespace Rallydator.Validation
{
    internal class GearDie : Die
    {
        public GearDie(int gear) : base(gear)
        {
            if (gear < 1 || gear > 5) throw new InvalidOperationException("Gear die must be between 1 and 5.");
        }

        protected override string DieType => "G";
    }
}