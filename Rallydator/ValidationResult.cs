using System.Collections.Generic;

namespace Rallydator
{
    public class ValidationResult
    {
        private readonly List<string> _errors = new List<string>();

        public ValidationResult()
        {
            
        }

        public IEnumerable<string> Errors => _errors;
    }
}