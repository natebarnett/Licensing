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
                var validator = new EmbeddedLicenseValidator(publicKeyContents)
                                {
                                    RequireNetworkTimeCheck = false
                                };
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
                var validator = new EmbeddedLicenseValidator(publicKeyContents)
                                {
                                    RequireNetworkTimeCheck = false
                                };
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

        [Test]
        public async Task TamperedLicense_FailsCryptoSignature()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                var privateKeyContents = await File.ReadAllTextAsync(privateKeyPath);
                var generator = new LicenseGenerator(privateKeyContents);
                var licenseContent = generator.Generate("Acme Test Co.", Guid.NewGuid(),
                                                        DateTime.UtcNow.AddDays(1), new Dictionary<string, string>(),
                                                        LicenseType.Trial);

                // Tamper: replace the license holder name in the raw XML
                var tampered = licenseContent.Replace("<name>Acme Test Co.</name>",
                                                      "<name>Evil Hacker</name>");

                var publicKeyContents = await File.ReadAllTextAsync(publicKeyPath);
                var validator = new EmbeddedLicenseValidator(publicKeyContents) { RequireNetworkTimeCheck = false };

                Assert.ThrowsAsync<CryptoSignatureVerificationFailedException>(
                    () => validator.AssertValidLicenseAsync(tampered));
            }
            finally
            {
                try { File.Delete(publicKeyPath); } catch { /* Ignore */ }
                try { File.Delete(privateKeyPath); } catch { /* Ignore */ }
            }
        }

        [Test]
        public async Task FloatingLicenseType_ThrowsNotImplemented()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                var privateKeyContents = await File.ReadAllTextAsync(privateKeyPath);
                var generator = new LicenseGenerator(privateKeyContents);
                var licenseContent = generator.Generate("Acme Test Co.", Guid.NewGuid(),
                                                        DateTime.UtcNow.AddDays(1), new Dictionary<string, string>(),
                                                        LicenseType.Floating);

                var publicKeyContents = await File.ReadAllTextAsync(publicKeyPath);
                var validator = new EmbeddedLicenseValidator(publicKeyContents) { RequireNetworkTimeCheck = false };

                Assert.ThrowsAsync<NotImplementedException>(() => validator.AssertValidLicenseAsync(licenseContent));
            }
            finally
            {
                try { File.Delete(publicKeyPath); } catch { /* Ignore */ }
                try { File.Delete(privateKeyPath); } catch { /* Ignore */ }
            }
        }

        [Test]
        public async Task SubscriptionLicenseType_ThrowsNotImplemented()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                var privateKeyContents = await File.ReadAllTextAsync(privateKeyPath);
                var generator = new LicenseGenerator(privateKeyContents);
                var licenseContent = generator.Generate("Acme Test Co.", Guid.NewGuid(),
                                                        DateTime.UtcNow.AddDays(1), new Dictionary<string, string>(),
                                                        LicenseType.Subscription);

                var publicKeyContents = await File.ReadAllTextAsync(publicKeyPath);
                var validator = new EmbeddedLicenseValidator(publicKeyContents) { RequireNetworkTimeCheck = false };

                Assert.ThrowsAsync<NotImplementedException>(() => validator.AssertValidLicenseAsync(licenseContent));
            }
            finally
            {
                try { File.Delete(publicKeyPath); } catch { /* Ignore */ }
                try { File.Delete(privateKeyPath); } catch { /* Ignore */ }
            }
        }
    }
}