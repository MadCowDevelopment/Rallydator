using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rallydator
{
    public class RaceResult
    {
        public RaceResult(IEnumerable<Roll> rolls)
        {
            Rolls = new ReadOnlyCollection<Roll>(rolls.ToList());
        }

        public IEnumerable<Roll> Rolls { get; }
    }
}