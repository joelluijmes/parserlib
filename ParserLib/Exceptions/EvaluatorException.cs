using System;

namespace ParserLib
{
    public sealed class EvaluatorException : Exception
    {
        public EvaluatorException()
        {
        }

        public EvaluatorException(string message) : base(message)
        {
        }

        public EvaluatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}