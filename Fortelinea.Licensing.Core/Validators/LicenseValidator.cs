using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml;
using NLog;
using Yort.Ntp;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     Base license validator.
    /// </summary>
    public abstract class LicenseValidator : ILicenseValidator
    {
#if !NET10_0_OR_GREATER
        private readonly CspParameters _cspParameters;

        private readonly int _keySize;
#endif

        protected readonly ILogger _log = LogManager.GetCurrentClassLogger();

        private readonly NtpClient _ntpClient = new NtpClient("time.nist.gov");

        private readonly string _publicKey;

        /// <summary>
        ///     Creates a license validator with specified public key.
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <param name="keySize"></param>
#if NET10_0_OR_GREATER
        protected LicenseValidator(string publicKey, int keySize = 4096)
        {
            _publicKey = publicKey;
            // keySize accepted for call-site source compatibility; key size is determined by imported key material on net10
        }
#else
        /// <param name="cspParameters">null by default</param>
        protected LicenseValidator(string publicKey, int keySize = 4096, CspParameters cspParameters = null)
        {
            _publicKey = publicKey;
            _keySize = keySize;
            _cspParameters = cspParameters;
        }
#endif

        public bool RequireNetworkTimeCheck { get; set; } = true;

        #region ILicenseValidator

        /// <summary>
        ///     Validates loaded license
        /// </summary>
        public virtual async Task AssertValidLicenseAsync(string xmlLicenseContents)
        {
            var existingLicense = LicenseParser.LoadLicenseContent(xmlLicenseContents);
            var updatedLicense = await HandleUpdateableLicenseAsync(existingLicense);

            ValidateLicenseType(updatedLicense);
            if (!IsValidLicenseCryptoSignature(updatedLicense)) throw new CryptoSignatureVerificationFailedException();
            if (await IsLicenseExpiredAsync(updatedLicense)) throw new LicenseExpiredException($"License expired {updatedLicense.ExpirationDate}");
        }

        #endregion

        private async Task<DateTime> GetCurrentDateTimeAsync()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                if (RequireNetworkTimeCheck) throw new NetworkUnavailableException();
                return DateTime.UtcNow;
            }

            try
            {
                var currentTime = await _ntpClient.RequestTimeAsync();
                return currentTime.NtpTime;
            }
            catch
            {
                if (RequireNetworkTimeCheck) throw;
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        ///     If the license is a subscription or floating license, this function should fetch the new license, as appropriate.
        /// </summary>
        /// <param name="license"></param>
        /// <returns>Returns most recent license</returns>
        protected abstract Task<License> HandleUpdateableLicenseAsync(License license);

        private async Task<bool> IsLicenseExpiredAsync(License license)
        {
            var now = await GetCurrentDateTimeAsync();
            return now > license.ExpirationDate;
        }

        protected bool IsValidLicenseCryptoSignature(License clientLicense)
        {
#if NET10_0_OR_GREATER
            using var rsa = RSA.Create();
            rsa.FromXmlString(_publicKey);
            var signedXml = new SignedXml(clientLicense.FileContents);
            signedXml.LoadXml(clientLicense.Signature);
            return signedXml.CheckSignature(rsa);
#else
            using (var rsa = _cspParameters == null ? new RSACryptoServiceProvider(_keySize) : new RSACryptoServiceProvider(_keySize, _cspParameters))
            {
                RSAKeyExtensions.FromXmlString(rsa, _publicKey);
                var signedXml = new SignedXml(clientLicense.FileContents);
                signedXml.LoadXml(clientLicense.Signature);
                return signedXml.CheckSignature(rsa);
            }
#endif
        }

        protected abstract void ValidateLicenseType(License license);
    }
}