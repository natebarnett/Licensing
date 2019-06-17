using System;
using System.Runtime.Serialization;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Thrown when license is found but is past it's expiration date
    /// </summary>
    public class LicenseExpiredException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        public LicenseExpiredException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public LicenseExpiredException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public LicenseExpiredException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">streaming context</param>
        public LicenseExpiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}