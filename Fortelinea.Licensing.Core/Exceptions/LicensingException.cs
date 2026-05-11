using System;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Base class for all licensing exceptions.
    /// </summary>
    public class LicensingException : Exception
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        protected LicensingException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        protected LicensingException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        protected LicensingException(string message, Exception inner) : base(message, inner) { }

    }
}