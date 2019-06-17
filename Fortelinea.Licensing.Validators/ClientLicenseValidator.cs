using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Fortelinea.Licensing.Core;
using NLog;

namespace Fortelinea.Licensing.Validators
{
    //public class ClientLicenseValidator : ILicenseValidator
    //{
    //    private readonly string _licensePath;

    //    protected readonly ILogger _log = LogManager.GetCurrentClassLogger();

    //    private readonly Timer _nextLeaseTimer;

    //    private readonly ILicenseService _service;

    //    private string _inMemoryLicense;

    //    public ClientLicenseValidator(ILicenseService service, string licensePath)
    //    {
    //        _service = service;
    //        _licensePath = licensePath;
    //    }

    //    protected string License
    //    {
    //        get => _inMemoryLicense ?? File.ReadAllText(_licensePath);
    //        set
    //        {
    //            try
    //            {
    //                File.WriteAllText(_licensePath, value);
    //            }
    //            catch (Exception e)
    //            {
    //                _inMemoryLicense = value;
    //                _log.Warn(e, "Could not write new license value, using in memory model instead");
    //            }
    //        }
    //    }

    //    #region ILicenseValidator

    //    public virtual async Task AssertValidLicenseAsync(string xmlLicenseContents)
    //    {
    //        var currentLicense = LicenseParser.LoadLicenseContent(xmlLicenseContents);
    //        var newLicense = await _service.GetLicenseAsync(currentLicense);

    //        // TODO: Overwrite existing file
    //        // TODO: Setup a timer for revalidation
    //    }

    //    #endregion
    //}
}