namespace Rallydator
{
    public class Damage
    {
        public int GearDiceLost { get; }
        public int AccelerateDiceLost { get; }

        public Damage(int gearDiceLost, int accelerateDiceLost)
        {
            GearDiceLost = gearDiceLost;
            AccelerateDiceLost = accelerateDiceLost;
        }

        public static Damage None => new Damage(0, 0);

        public static Damage operator +(Damage x, Damage y)
        {
            return new Damage(x.GearDiceLost + y.GearDiceLost, x.AccelerateDiceLost + y.AccelerateDiceLost);
        }
    }
}