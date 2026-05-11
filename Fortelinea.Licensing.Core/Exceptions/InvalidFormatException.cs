using System;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Thrown when suitable license is not found.
    /// </summary>
    public class InvalidFormatException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="InvalidFormatException" />.
        /// </summary>
        public InvalidFormatException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="InvalidFormatException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public InvalidFormatException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="InvalidFormatException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public InvalidFormatException(string message, Exception inner) : base(message, inner) { }

    }
}