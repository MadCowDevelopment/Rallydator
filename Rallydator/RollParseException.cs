using System;

namespace Rallydator
{
    public class RollParseException : Exception
    {
        public RollParseException(string message) : base(message)
        {
        }
    }
}