using System;
using System.Security.Cryptography;
using System.Xml;

namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     The .Net Standard library implementations of these throw an exception (instead of fixing a circular reference)
    ///     This implementation came from the issue(https://github.com/dotnet/core/issues/874)
    ///     Ifthe issue gets fixed in .NET Standard libraries, this class should be removed
    /// </summary>
    internal static class RSAKeyExtensions
    {
        public static void FromXmlString(this RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    switch (node.Name)
                    {
                        case "Modulus":
                            parameters.Modulus = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "Exponent":
                            parameters.Exponent = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "P":
                            parameters.P = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "Q":
                            parameters.Q = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "DP":
                            parameters.DP = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "DQ":
                            parameters.DQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "InverseQ":
                            parameters.InverseQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                        case "D":
                            parameters.D = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
                            break;
                    }
            else
                throw new Exception("Invalid XML RSA key.");

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlString(this RSA rsa, bool includePrivateParameters)
        {
            var parameters = rsa.ExportParameters(includePrivateParameters);
            return
                $"<RSAKeyValue><Modulus>{(parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null)}</Modulus><Exponent>{(parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null)}</Exponent><P>{(parameters.P != null ? Convert.ToBase64String(parameters.P) : null)}</P><Q>{(parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null)}</Q><DP>{(parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null)}</DP><DQ>{(parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null)}</DQ><InverseQ>{(parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null)}</InverseQ><D>{(parameters.D != null ? Convert.ToBase64String(parameters.D) : null)}</D></RSAKeyValue>";
        }
    }
}