using System;

namespace Rallydator.Core
{
    public class RollParseException : Exception
    {
        public RollParseException(string message) : base(message)
        {
        }
    }
}