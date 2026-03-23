using MyofficeApi.Models;

namespace MyofficeApi.Services
{
    public interface IMyofficeAcpdService
    {
        Task<IEnumerable<MyOfficeAcpd>> GetAllAsync();
        Task<MyOfficeAcpd?> GetByIdAsync(string id);
        Task<string> CreateAsync(MyOfficeAcpd entity);
        Task<bool> UpdateAsync(string id, MyOfficeAcpd entity);
        Task<bool> DeleteAsync(string id);
    }
}