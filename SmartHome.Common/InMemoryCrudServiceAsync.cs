using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Common
{
    public class InMemoryCrudServiceAsync<T> : ICrudServiceAsync<T> where T : Device
    {
        private readonly ConcurrentDictionary<Guid, T> _items = new ConcurrentDictionary<Guid, T>();
        private readonly string _filePath;
        private readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);

        public InMemoryCrudServiceAsync(string filePath)
        {
            _filePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            if (element == null) return Task.FromResult(false);
            bool added = _items.TryAdd(element.Id, element);
            return Task.FromResult(added);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _items.TryGetValue(id, out T item);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_items.Values.ToList());
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page < 1 || amount < 1)
                return Task.FromResult(Enumerable.Empty<T>());

            var pagedItems = _items.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();

            return Task.FromResult<IEnumerable<T>>(pagedItems);
        }

        public Task<bool> UpdateAsync(T element)
        {
            if (element == null || !_items.ContainsKey(element.Id))
                return Task.FromResult(false);

            _items[element.Id] = element;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(T element)
        {
            if (element == null) return Task.FromResult(false);
            bool removed = _items.TryRemove(element.Id, out _);
            return Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_items.Values, options);
                await File.WriteAllTextAsync(_filePath, json);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}