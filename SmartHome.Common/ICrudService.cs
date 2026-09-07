using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SmartHome.Common
{
    // Інтерфейс CRUD з методики
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);

        // Додаткове завдання
        void Save(string filePath);
        void Load(string filePath);
    }

    public class InMemoryCrudService<T> : ICrudService<T>
    {
        private readonly List<T> _items = new List<T>();

        public void Create(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            _items.Add(element);
        }

        public T Read(Guid id)
        {
            return _items.FirstOrDefault(item =>
            {
                var prop = typeof(T).GetProperty("Id");
                if (prop != null)
                {
                    var val = (Guid)prop.GetValue(item);
                    return val == id;
                }
                return false;
            });
        }

        public IEnumerable<T> ReadAll() => _items;

        public void Update(T element)
        {
            if (element == null) return;
            var prop = typeof(T).GetProperty("Id");
            if (prop == null) return;

            Guid id = (Guid)prop.GetValue(element);
            var existingIndex = _items.FindIndex(item => (Guid)prop.GetValue(item) == id);

            if (existingIndex != -1)
            {
                _items[existingIndex] = element;
            }
        }

        public void Remove(T element) => _items.Remove(element);

        // Додаткове завдання: Збереження та завантаження з файлу
        public void Save(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;
            string json = File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<T>>(json);
            if (items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
    }
}