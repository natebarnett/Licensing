using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fortelinea.Licensing.Core;
using Fortelinea.Licensing.Validators;
using NUnit.Framework;

namespace Fortelinea.Licensing.Test
{
    public class LicenseValidatorTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public async Task TestExpiredLicenseFailsValidationAsync()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                var privateKeyContents = await File.ReadAllTextAsync(privateKeyPath);
                var licenseGenerator = new LicenseGenerator(privateKeyContents);

                const string licenseName = "Acme Test Co."; // Could be customer or single client, etc
                var id = Guid.NewGuid();
                var now = DateTime.UtcNow;
                var expirationDate = now.AddDays(-1);
                var extraAttributes = new Dictionary<string, string>();
                var clientLicenseContent = licenseGenerator.Generate(licenseName, id, expirationDate, extraAttributes, LicenseType.Trial);

                var publicKeyContents = await File.ReadAllTextAsync(publicKeyPath);
                var validator = new EmbeddedLicenseValidator(publicKeyContents);
                Assert.ThrowsAsync<LicenseExpiredException>(() => validator.AssertValidLicenseAsync(clientLicenseContent));
            }
            finally
            {
                try { File.Delete(publicKeyPath); }
                catch
                {
                    /* Ignore */
                }

                try { File.Delete(privateKeyPath); }
                catch
                {
                    /* Ignore */
                }
            }
        }

        [Test]
        public async Task TestValidLicensePassesValidationAsync()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                var privateKeyContents = await File.ReadAllTextAsync(privateKeyPath);
                var licenseGenerator = new LicenseGenerator(privateKeyContents);

                const string licenseName = "Acme Test Co."; // Could be customer or single client, etc
                var id = Guid.NewGuid();
                var now = DateTime.UtcNow;
                var expirationDate = now.AddDays(1);
                var extraAttributes = new Dictionary<string, string>();
                var clientLicenseContent = licenseGenerator.Generate(licenseName, id, expirationDate, extraAttributes, LicenseType.Trial);

                var publicKeyContents = await File.ReadAllTextAsync(publicKeyPath);
                var validator = new EmbeddedLicenseValidator(publicKeyContents);
                Assert.DoesNotThrowAsync(() => validator.AssertValidLicenseAsync(clientLicenseContent));
            }
            finally
            {
                try { File.Delete(publicKeyPath); }
                catch
                {
                    /* Ignore */
                }

                try { File.Delete(privateKeyPath); }
                catch
                {
                    /* Ignore */
                }
            }
        }
    }
}