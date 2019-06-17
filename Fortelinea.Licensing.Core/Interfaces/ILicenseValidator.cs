using System.Threading.Tasks;

namespace Fortelinea.Licensing.Core
{
    public interface ILicenseValidator
    {
        Task AssertValidLicenseAsync(string xmlLicenseContents);
    }
}