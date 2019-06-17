using System;
using System.Threading.Tasks;
using Fortelinea.Licensing.Core;

namespace Fortelinea.Licensing.Validators
{
    /// <summary>
    ///     License validator validates a public and private key pair located on local/mapped drives
    /// </summary>
    public class EmbeddedLicenseValidator : LicenseValidator
    {
        /// <summary>
        ///     Creates a new instance of <seealso cref="EmbeddedLicenseValidator" />.
        /// </summary>
        /// <param name="publicKey">public key</param>
        public EmbeddedLicenseValidator(string publicKey) : base(publicKey) { }

        /// <param name="license"></param>
        /// <inheritdoc />
        protected override Task<License> HandleUpdateableLicenseAsync(License license) { return Task.FromResult(license); }

        /// <inheritdoc />
        protected override void ValidateLicenseType(License license)
        {
            switch (license.LicenseType)
            {
                case LicenseType.None: return;
                case LicenseType.Floating: throw new NotImplementedException();
                case LicenseType.Personal: return;
                case LicenseType.Standard: return;
                case LicenseType.Subscription: throw new NotImplementedException();
                case LicenseType.Trial: return;
            }
        }
    }
}