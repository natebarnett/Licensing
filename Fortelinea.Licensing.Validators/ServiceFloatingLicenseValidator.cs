using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using Fortelinea.Licensing.Core;

namespace Fortelinea.Licensing.Validators
{
    // TODO: Make this serializable so that it can recover from a service crash
    //public class ServiceFloatingLicenseValidator : LicenseValidator
    //{
    //    private readonly Timer _cleanupLeasesTimer;

    //    private readonly ConcurrentDictionary<Guid, License> _currentLeases = new ConcurrentDictionary<Guid, License>();

    //    private readonly ILicenseGenerator _licenseGenerator;

    //    public ServiceFloatingLicenseValidator(string publicKey, ILicenseGenerator licenseGenerator, int keySize = 384, CspParameters cspParameters = null) : base(publicKey, keySize, cspParameters)
    //    {
    //        _licenseGenerator = licenseGenerator;
    //        _cleanupLeasesTimer = new Timer(100)
    //                              {
    //                                  AutoReset = false,
    //                                  Enabled = false
    //                              };
    //        _cleanupLeasesTimer.Elapsed += CleanupLeases;
    //    }

    //    public TimeSpan LeaseLength { get; set; } = TimeSpan.FromDays(1);

    //    public int MaxLicenses { get; set; } = 2;

    //    private void CleanupLeases(object sender, ElapsedEventArgs e)
    //    {
    //        var now = DateTime.UtcNow;
    //        var remove = _currentLeases.Where(i => i.Value.ExpirationDate > now);
    //        foreach (var item in remove) _currentLeases.TryRemove(item.Key, out _);
    //        if (!_currentLeases.Any()) return;

    //        var nextExpiration = _currentLeases.Values.Min(i => i.ExpirationDate);
    //        var timeLength = nextExpiration - now;
    //        _cleanupLeasesTimer.Interval = timeLength.TotalMilliseconds;
    //        _cleanupLeasesTimer.Start();
    //    }

    //    /// <param name="license"></param>
    //    /// <inheritdoc />
    //    protected override async Task<License> HandleUpdateableLicenseAsync(License license)
    //    {
    //        if (_currentLeases.Count >= MaxLicenses) throw new FloatingLicenseNotAvailableException();
    //        var updated = await _licenseGenerator.GetUpdatedLicenseAsync(license);
    //        _currentLeases[updated.UserId] = updated;

    //        // TODO: Update and start timer
    //        return updated;
    //    }

    //    public void ReleaseLicenseLease(License license)
    //    {
    //        _currentLeases.TryRemove(license.UserId, out _);

    //        // TODO: Update and start timer
    //    }

    //    /// <inheritdoc />
    //    protected override void ValidateLicenseType(License license)
    //    {
    //        switch (license.LicenseType)
    //        {
    //            case LicenseType.None: return;
    //            case LicenseType.Floating: return;
    //            case LicenseType.Personal: throw new NotImplementedException();
    //            case LicenseType.Standard:
    //                throw new NotImplementedException();
    //                ;
    //            case LicenseType.Subscription: return;
    //            case LicenseType.Trial: throw new NotImplementedException();
    //        }
    //    }
    //}
}