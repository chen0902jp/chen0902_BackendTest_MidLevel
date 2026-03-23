using MyofficeApi.Models;
using MyofficeApi.Repositories;

namespace MyofficeApi.Services
{
    public class MyofficeAcpdService : IMyofficeAcpdService
    {
        private readonly IMyofficeAcpdRepository _repository;

        public MyofficeAcpdService(IMyofficeAcpdRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MyOfficeAcpd>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<MyOfficeAcpd?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public async Task<string> CreateAsync(MyOfficeAcpd entity) => await _repository.CreateAsync(entity);

        public async Task<bool> UpdateAsync(string id, MyOfficeAcpd entity) => await _repository.UpdateAsync(id, entity);

        public async Task<bool> DeleteAsync(string id) => await _repository.DeleteAsync(id);
    }
}