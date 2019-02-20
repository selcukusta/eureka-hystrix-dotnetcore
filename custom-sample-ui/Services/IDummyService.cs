using System.Threading.Tasks;

namespace custom_sample_ui.Services
{
    public interface IDummyService
    {
        Task<string> GetAddress();
        Task<string> GetAddressFallback();
    }
}
