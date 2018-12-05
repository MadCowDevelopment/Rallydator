using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rallydator
{
    public class SpecialStageRolls
    {
        public SpecialStageRolls(IEnumerable<Roll> rolls)
        {
            Rolls = new ReadOnlyCollection<Roll>(rolls.ToList());
        }

        public IEnumerable<Roll> Rolls { get; }
    }
}