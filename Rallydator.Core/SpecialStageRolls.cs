using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rallydator.Core
{
    public class SpecialStageRolls
    {
        public SpecialStageRolls(IEnumerable<Roll> rolls, TireType tireType)
        {
            Rolls = new ReadOnlyCollection<Roll>(rolls.ToList());
        }

        public IEnumerable<Roll> Rolls { get; }
    }
}