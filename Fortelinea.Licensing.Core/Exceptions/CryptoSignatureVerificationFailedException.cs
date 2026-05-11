using System;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Thrown when the cryptography signature does not match, indicating tampering or formatting problem
    /// </summary>
    public class CryptoSignatureVerificationFailedException : LicensingException
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        public CryptoSignatureVerificationFailedException() { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        public CryptoSignatureVerificationFailedException(string message) : base(message) { }

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicensingException" />.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public CryptoSignatureVerificationFailedException(string message, Exception inner) : base(message, inner) { }

    }
}