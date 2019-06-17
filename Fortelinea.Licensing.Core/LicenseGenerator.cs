﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     LicenseGenerator generates and signs license.
    /// </summary>
    public class LicenseGenerator
    {
        private readonly string _privateKey;

        /// <summary>
        ///     Creates a new instance of <seealso cref="LicenseGenerator" />.
        /// </summary>
        /// <param name="privateKey">private key of the product</param>
        public LicenseGenerator(string privateKey) { _privateKey = privateKey; }

        private static XmlDocument CreateDocument(Guid id, string name, DateTime expirationDate, IDictionary<string, string> attributes, LicenseType licenseType)
        {
            var doc = new XmlDocument();
            var license = doc.CreateElement("license");
            doc.AppendChild(license);
            var idAttr = doc.CreateAttribute("id");
            license.Attributes.Append(idAttr);
            idAttr.Value = id.ToString();

            var expirationDateAttr = doc.CreateAttribute("expiration");
            license.Attributes.Append(expirationDateAttr);
            expirationDateAttr.Value = expirationDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture);

            var licenseAttr = doc.CreateAttribute("type");
            license.Attributes.Append(licenseAttr);
            licenseAttr.Value = licenseType.ToString();

            var nameEl = doc.CreateElement("name");
            license.AppendChild(nameEl);
            nameEl.InnerText = name;

            foreach (var attribute in attributes)
            {
                var attrib = doc.CreateAttribute(attribute.Key);
                attrib.Value = attribute.Value;
                license.Attributes.Append(attrib);
            }

            return doc;
        }

        /// <summary>
        ///     Generates a new license
        /// </summary>
        /// <param name="name">name of the license holder</param>
        /// <param name="id">Id of the license holder</param>
        /// <param name="expirationDate">expiry date</param>
        /// <param name="licenseType">type of the license</param>
        /// <returns></returns>
        public string Generate(string name, Guid id, DateTime expirationDate, LicenseType licenseType) { return Generate(name, id, expirationDate, new Dictionary<string, string>(), licenseType); }

        /// <summary>
        ///     Generates a new license
        /// </summary>
        /// <param name="name">name of the license holder</param>
        /// <param name="id">Id of the license holder</param>
        /// <param name="expirationDate">expiry date</param>
        /// <param name="licenseType">type of the license</param>
        /// <param name="attributes">extra information stored as key/value in the license file</param>
        /// <returns></returns>
        public string Generate(string name, Guid id, DateTime expirationDate, IDictionary<string, string> attributes, LicenseType licenseType)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                RSAKeyExtensions.FromXmlString(rsa, _privateKey);
                var doc = CreateDocument(id, name, expirationDate, attributes, licenseType);

                var signature = GetXmlDigitalSignature(doc, rsa);
                doc.FirstChild.AppendChild(doc.ImportNode(signature, true));

                var ms = new MemoryStream();
                var writer = XmlWriter.Create(ms,
                                              new XmlWriterSettings
                                              {
                                                  Indent = true,
                                                  Encoding = Encoding.UTF8
                                              });
                doc.Save(writer);
                ms.Position = 0;
                return new StreamReader(ms).ReadToEnd();
            }
        }

        /// <summary>
        ///     Generates a new floating license.
        /// </summary>
        /// <param name="name">Name of the license holder</param>
        /// <param name="publicKey">public key of the license server</param>
        /// <returns>license content</returns>
        public string GenerateFloatingLicense(string name, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                RSAKeyExtensions.FromXmlString(rsa, _privateKey);
                var doc = new XmlDocument();
                var license = doc.CreateElement("floating-license");
                doc.AppendChild(license);

                var publicKeyEl = doc.CreateElement("license-server-public-key");
                license.AppendChild(publicKeyEl);
                publicKeyEl.InnerText = publicKey;

                var nameEl = doc.CreateElement("name");
                license.AppendChild(nameEl);
                nameEl.InnerText = name;

                var signature = GetXmlDigitalSignature(doc, rsa);
                doc.FirstChild.AppendChild(doc.ImportNode(signature, true));

                var ms = new MemoryStream();
                var writer = XmlWriter.Create(ms,
                                              new XmlWriterSettings
                                              {
                                                  Indent = true,
                                                  Encoding = Encoding.UTF8
                                              });
                doc.Save(writer);
                ms.Position = 0;
                return new StreamReader(ms).ReadToEnd();
            }
        }

        /// <summary>
        ///     Generates a public/private key pair that can then be used to create client licenses.
        ///     The public key gets sent with the license and the private is used for the crypto-signing
        /// </summary>
        public static Task GenerateKeyAsync(string publicKeyPath, string privateKeyPath, int keySize = 4096, CspParameters cspParameters = null)
        {
            var rsa = cspParameters == null ? new RSACryptoServiceProvider(keySize) : new RSACryptoServiceProvider(keySize, cspParameters);
            RSAKeyExtensions.ToXmlString(rsa, false);
            File.WriteAllText(publicKeyPath, RSAKeyExtensions.ToXmlString(rsa, false));
            File.WriteAllText(privateKeyPath, RSAKeyExtensions.ToXmlString(rsa, true));

            return Task.CompletedTask;
        }

        private static XmlElement GetXmlDigitalSignature(XmlDocument x, AsymmetricAlgorithm key)
        {
            var signedXml = new SignedXml(x)
                            {
                                SigningKey = key
                            };
            var reference = new Reference
                            {
                                Uri = ""
                            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            return signedXml.GetXml();
        }
    }
}