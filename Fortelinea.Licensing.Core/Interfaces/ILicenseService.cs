using System.Threading.Tasks;

namespace Fortelinea.Licensing.Core
{
    public interface ILicenseService
    {
        Task<License> GetLicenseAsync(License license);
    }
}