namespace Rallydator.Core.Validation
{
    internal abstract class Die
    {
        public int Gear { get; }

        protected Die(int gear)
        {
            Gear = gear;
        }

        public override string ToString()
        {
            return $"{DieType}{Gear}";
        }

        protected abstract string DieType { get; }
    }
}