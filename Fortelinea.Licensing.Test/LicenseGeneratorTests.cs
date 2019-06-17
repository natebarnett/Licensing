using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fortelinea.Licensing.Core;
using NUnit.Framework;

namespace Fortelinea.Licensing.Test
{
    public class LicenseGeneratorTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public async Task TestGenerateKeyAsync()
        {
            var publicKeyPath = Path.GetTempFileName();
            var privateKeyPath = Path.GetTempFileName();

            try
            {
                await LicenseGenerator.GenerateKeyAsync(publicKeyPath, privateKeyPath);

                // GetTempFileName creates a 0 byte file, so test that we actually added content
                Assert.That(new FileInfo(publicKeyPath).Length > 0);
                Assert.That(new FileInfo(privateKeyPath).Length > 0);
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
        public async Task TestGeneratingClientLicenseAsync()
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

                var clientLicense = LicenseParser.LoadLicenseContent(clientLicenseContent);

                // Ensure that what we packed into the license can be read
                Assert.AreEqual(licenseName, clientLicense.Name);
                Assert.AreEqual(id, clientLicense.UserId);
                Assert.AreEqual(TimeSpan.FromDays(1), clientLicense.ExpirationDate - now);
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