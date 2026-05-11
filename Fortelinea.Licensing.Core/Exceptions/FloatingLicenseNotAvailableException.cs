using System;

namespace Fortelinea.Licensing.Core
{
    public class FloatingLicenseNotAvailableException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="FloatingLicenseNotAvailableException" />.
        /// </summary>
        public FloatingLicenseNotAvailableException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="FloatingLicenseNotAvailableException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public FloatingLicenseNotAvailableException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="FloatingLicenseNotAvailableException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public FloatingLicenseNotAvailableException(string message, Exception inner) : base(message, inner) { }

    }
}