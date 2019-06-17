using System;
using System.Collections.Generic;
using System.Xml;

namespace Fortelinea.Licensing.Core
{
    public class License
    {
        private XmlDocument _fileContents;

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        /// <summary>
        ///     Gets the expiration date of the license
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        ///     Gets the cryptography signature of the license file contents
        /// </summary>
        public XmlDocument FileContents
        {
            get => _fileContents;
            set
            {
                _fileContents = value;

                if (_fileContents == null)
                {
                    Signature = null;
                }
                else
                {
                    var nsMgr = new XmlNamespaceManager(_fileContents.NameTable);
                    nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
                    Signature = (XmlElement)_fileContents.SelectSingleNode("//sig:Signature", nsMgr);
                }
            }
        }

        /// <summary>
        ///     Gets the Type of the license
        /// </summary>
        public LicenseType LicenseType { get; set; }

        /// <summary>
        ///     Gets the name of the license holder
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the cryptography signature of the license file contents
        /// </summary>
        public XmlElement Signature { get; private set; }

        /// <summary>
        ///     Gets the Id of the license holder
        /// </summary>
        public Guid UserId { get; set; }
    }
}