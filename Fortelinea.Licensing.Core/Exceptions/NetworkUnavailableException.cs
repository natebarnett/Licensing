using System;
using System.Runtime.Serialization;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Thrown when suitable license is not found.
    /// </summary>
    [Serializable]
    public class NetworkUnavailableException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        public NetworkUnavailableException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public NetworkUnavailableException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public NetworkUnavailableException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseNotFoundException" />.
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">steaming context</param>
        protected NetworkUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}