using System;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Thrown when suitable license is not found.
    /// </summary>
    public class LicenseNotFoundException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        public LicenseNotFoundException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public LicenseNotFoundException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public LicenseNotFoundException(string message, Exception inner) : base(message, inner) { }

    }
}