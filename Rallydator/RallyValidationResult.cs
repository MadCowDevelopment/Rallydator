using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Rallydator.Validation;

namespace Rallydator
{
    public class RallyValidationResult
    {
        private readonly IReadOnlyCollection<ValidationResult> _specialStageValidationResults;

        public RallyValidationResult(IEnumerable<ValidationResult> specialStageValidationResults)
        {
            _specialStageValidationResults = new ReadOnlyCollection<ValidationResult>(specialStageValidationResults.ToList());
        }

        public IEnumerable<ValidationResult> ValidationResults => _specialStageValidationResults;
    }
}