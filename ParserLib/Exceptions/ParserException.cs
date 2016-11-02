using System;

namespace ParserLib.Exceptions
{
    /// <summary>
    ///     The exception that is thrown when there is failure in the parsing.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class ParserException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        public ParserException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ParserException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ParserException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}