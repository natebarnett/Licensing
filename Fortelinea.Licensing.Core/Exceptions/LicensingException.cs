using System;
using System.Runtime.Serialization;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Base class for all licensing exceptions.
    /// </summary>
    [Serializable]
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

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">streaming context</param>
        protected LicensingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}