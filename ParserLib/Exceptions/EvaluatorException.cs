using System;

namespace ParserLib.Exceptions
{
	/// <summary>
	/// The exception that is thrown when there is failure in converting the matched result in a value.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public sealed class EvaluatorException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluatorException"/> class.
		/// </summary>
		public EvaluatorException()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluatorException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public EvaluatorException(string message) : base(message)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluatorException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public EvaluatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}