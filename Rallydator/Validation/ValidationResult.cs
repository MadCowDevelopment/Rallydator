using System.Collections.Generic;

namespace Rallydator.Validation
{
    public class ValidationResult
    {
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;

        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}