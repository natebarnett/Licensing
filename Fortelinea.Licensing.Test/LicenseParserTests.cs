using System;
using System.IO;
using Fortelinea.Licensing.Core;
using NUnit.Framework;

namespace Fortelinea.Licensing.Test
{
    public class LicenseParserTests
    {
        // Minimal valid signature element — satisfies the namespace check without needing real crypto
        private const string SigElement = "<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"/>";
        private const string ValidId = "00000000-0000-0000-0000-000000000000";
        private const string ValidExpiry = "2099-01-01T00:00:00.0000000";

        [Test]
        public void LoadLicenseContent_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => LicenseParser.LoadLicenseContent(null));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => LicenseParser.LoadLicenseContent(""));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnMissingSignature()
        {
            var xml = $"<license id=\"{ValidId}\" expiration=\"{ValidExpiry}\" type=\"Trial\"><name>Test</name></license>";
            Assert.Throws<InvalidFormatException>(() => LicenseParser.LoadLicenseContent(xml));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnMissingId()
        {
            var xml = $"<license expiration=\"{ValidExpiry}\" type=\"Trial\"><name>Test</name>{SigElement}</license>";
            Assert.Throws<InvalidFormatException>(() => LicenseParser.LoadLicenseContent(xml));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnMissingExpiration()
        {
            var xml = $"<license id=\"{ValidId}\" type=\"Trial\"><name>Test</name>{SigElement}</license>";
            Assert.Throws<InvalidFormatException>(() => LicenseParser.LoadLicenseContent(xml));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnMissingType()
        {
            var xml = $"<license id=\"{ValidId}\" expiration=\"{ValidExpiry}\"><name>Test</name>{SigElement}</license>";
            Assert.Throws<InvalidFormatException>(() => LicenseParser.LoadLicenseContent(xml));
        }

        [Test]
        public void LoadLicenseContent_ThrowsOnMissingName()
        {
            var xml = $"<license id=\"{ValidId}\" expiration=\"{ValidExpiry}\" type=\"Trial\">{SigElement}</license>";
            Assert.Throws<InvalidFormatException>(() => LicenseParser.LoadLicenseContent(xml));
        }

        [Test]
        public void LoadLicenseFile_ThrowsOnMissingFile()
        {
            var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");
            Assert.Throws<LicenseFileNotFoundException>(() => LicenseParser.LoadLicenseFile(missingPath));
        }
    }
}
