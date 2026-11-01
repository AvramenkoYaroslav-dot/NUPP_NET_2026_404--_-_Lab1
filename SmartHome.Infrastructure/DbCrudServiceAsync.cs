using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Common;
using SmartHome.Infrastructure.Models;

namespace SmartHome.Infrastructure
{
    public class DbCrudServiceAsync : ICrudServiceAsync<DeviceModel>
    {
        private readonly IRepository<DeviceModel> _repository;

        public DbCrudServiceAsync(IRepository<DeviceModel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(DeviceModel element)
        {
            if (element == null) return false;
            await _repository.AddAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<DeviceModel> ReadAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DeviceModel>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<DeviceModel>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(DeviceModel element)
        {
            if (element == null) return false;
            await _repository.Update(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(DeviceModel element)
        {
            if (element == null) return false;
            await _repository.Delete(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            await _repository.SaveChangesAsync();
            return true;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return ReadAllAsync().GetAwaiter().GetResult().GetEnumerator();
        }

        IEnumerator<DeviceModel> IEnumerable<DeviceModel>.GetEnumerator()
        {
            return ReadAllAsync().GetAwaiter().GetResult().GetEnumerator();
        }
    }
}