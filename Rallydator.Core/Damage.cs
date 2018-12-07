using System;
using System.Text;

namespace Rallydator.Core
{
    public class Damage : IEquatable<Damage>
    {
        public bool Equals(Damage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GearDiceLost == other.GearDiceLost && AccelerateDiceLost == other.AccelerateDiceLost;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Damage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (GearDiceLost * 397) ^ AccelerateDiceLost;
            }
        }

        public int GearDiceLost { get; }
        public int AccelerateDiceLost { get; private set; }

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

        public void ChangeTire()
        {
            if (AccelerateDiceLost > 0) AccelerateDiceLost--;
        }

        public Damage Clone()
        {
            var clone = new Damage(GearDiceLost, AccelerateDiceLost);
            return clone;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (GearDiceLost > 0) builder.Append($"-{GearDiceLost}");
            if (AccelerateDiceLost > 0) builder.Append($"-{AccelerateDiceLost}");
            return builder.ToString();
        }
    }
}