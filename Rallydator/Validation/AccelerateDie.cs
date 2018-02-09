using System;

namespace Rallydator.Validation
{
    internal class AccelerateDie : Die
    {
        public AccelerateDie(int gear) : base(gear)
        {
            if (gear < 0 || gear > 5) throw new InvalidOperationException("Acceleration die must be between 0 and 5.");
        }

        protected override string DieType => "A";
    }
}