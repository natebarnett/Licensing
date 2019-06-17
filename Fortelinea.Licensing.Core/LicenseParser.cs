using System;
using System.Globalization;
using System.Xml;
using NLog;

namespace Fortelinea.Licensing.Core
{
    public static class LicenseParser
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public static License LoadLicenseContent(string licenseContent)
        {
            if (string.IsNullOrEmpty(licenseContent)) throw new ArgumentNullException(nameof(licenseContent), "Must be a valid file path");

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(licenseContent);
            return Parse(xmlDocument);
        }

        public static License LoadLicenseFile(string licensePath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(licensePath);
            return Parse(xmlDocument);
        }

        public static XmlDocument OpenFile(string licenseFilename)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(licenseFilename);
            return xmlDocument;
        }

        /// <summary>
        ///     Parses a license XML file into a structure that is easy to pass around and validate
        /// </summary>
        /// <param name="doc">XmlDocument doc</param>
        /// <returns>License</returns>
        /// <exception cref="InvalidFormatException">Thrown when a piece of required info is not found in the license XML file</exception>
        public static License Parse(XmlDocument doc)
        {
            var license = new License();
            license.FileContents = doc; // Should this be cloned?
            if (license.Signature == null) throw new InvalidFormatException("Could not find the crypto-signature for the license");

            var id = doc.SelectSingleNode("/license/@id");
            if (id == null) throw new InvalidFormatException("Could not find id attribute in the license");
            license.UserId = new Guid(id.Value);

            var date = doc.SelectSingleNode("/license/@expiration");
            if (date == null) throw new InvalidFormatException("Could not find the expiration date in the license");
            license.ExpirationDate = DateTime.ParseExact(date.Value, "yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture);

            var licenseType = doc.SelectSingleNode("/license/@type");
            if (licenseType == null) throw new InvalidFormatException("Could not determine the license type");
            license.LicenseType = (LicenseType)Enum.Parse(typeof(LicenseType), licenseType.Value);

            var name = doc.SelectSingleNode("/license/name/text()");
            if (name == null) throw new InvalidFormatException("Could not find licensee's name in license");
            license.Name = name.Value;

            var licenseNode = doc.SelectSingleNode("/license");
            if (licenseNode?.Attributes == null) return license;

            foreach (XmlAttribute attr in licenseNode.Attributes)
            {
                if ((attr.Name == "type") || (attr.Name == "expiration") || (attr.Name == "id"))
                    continue;

                _log.Debug($"License has attribute: [{attr.Name}, {attr.Value}]");
            }

            return license;
        }
    }
}