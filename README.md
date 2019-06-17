# XML File Licensing
Public/private key XML license generation and validation library

## History
Taking strong queues from Rhino Licensing (https://github.com/hibernating-rhinos/rhino-licensing), this library should be interoperable with licenses from that project. It is a complete rewrite, targetting .NET standard, but does use some of the same enums and algorithms.

##Usage
The core library contains tools for creating public/private key pairs, creating client licenses and base classes for validation.
The validators library contains implementations of the core validator for validation within the client app, floating licenses (TODO), and subscriptions (TODO)

For the most basic validation (embedded), setup a public/private key pair. Keep the private key safe. Ship the public key and a client license. Use the embedded validator at startup to validate the license using the public key.

```
var publicKey = File.ReadAllText(publicKeyPath);
var licenseContents = File.ReadAllText(licensePath);
var licenseValidator = new EmbeddedLicenseValidator(publicKey);
await licenseValidator.AssertValidLicenseAsync(licenseContents);
```

## Roadmap
- Make the license file serialize to XML
- Finish implementing floating and subscription license validators
- Cleanup interface: filename, file contents and license model should either be used interchangeably or settle on a format