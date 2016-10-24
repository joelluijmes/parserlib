using System;

namespace ParserLib
{
    public sealed class ParserException : Exception
    {
		public ParserException()
		{
		}

		public ParserException(Exception innerException) : base(innerException.Message, innerException)
		{
		}

		public ParserException(string message) : base(message)
        {
        }

        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}