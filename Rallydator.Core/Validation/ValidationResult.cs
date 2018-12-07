using System.Collections.Generic;

namespace Rallydator.Core.Validation
{
    public class ValidationResult
    {
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;
        public Damage Damage { get; set; } = Damage.None;

        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}