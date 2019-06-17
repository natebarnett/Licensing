using System.Threading.Tasks;

namespace Fortelinea.Licensing.Core
{
    public interface ILicenseGenerator
    {
        Task<License> GetUpdatedLicenseAsync(License currentLicense);
    }
}