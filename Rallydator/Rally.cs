using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Rallydator.Validation;

namespace Rallydator
{
    public class Rally
    {
        private readonly IReadOnlyCollection<SpecialStageResult> _specialStageResults;

        public Rally(Driver driver, IEnumerable<SpecialStageResult> specialStageResults)
        {
            Driver = driver;
            _specialStageResults = new ReadOnlyCollection<SpecialStageResult>(specialStageResults.ToList());
        }

        public Driver Driver { get; }
        public IEnumerable<SpecialStageResult> SpecialStages => _specialStageResults;

        public static Rally Create(Driver driver, IEnumerable<SpecialStageResult> specialStageResults)
        {
            return new Rally(driver, specialStageResults);
        }

        public RallyValidationResult Validate()
        {
            return new ResultValidator(this).Validate();
        }
    }
}