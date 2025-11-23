using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataStructures.Comparers;
using DataStructures;

namespace DemoApp
{
    // Проста репозиторна оболонка над List<Product>
    public class ProductRepository
    {
        private readonly List<Product> _items = new();

        public IEnumerable<Product> GetAll() => _items;

        public void Add(Product p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            _items.Add(p);
        }

        public bool Exists(string code) => _items.Any(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase));

        public Product? FindByCode(string code) => _items.FirstOrDefault(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Product> FindByNameContains(string part) =>
            _items.Where(x => x.Name.IndexOf(part ?? "", StringComparison.CurrentCultureIgnoreCase) >= 0);

        public void Remove(string code)
        {
            var p = FindByCode(code);
            if (p != null) _items.Remove(p);
        }

        public Product[] ToArray() => _items.ToArray();

        public ArrayList ToArrayList()
        {
            var al = new ArrayList();
            al.AddRange(_items);
            return al;
        }

        public void SortByName()
        {
            _items.Sort(); // Використає IComparable<Product> реалізацію в Product
        }

        public void SortByExpiry()
        {
            _items.Sort(new ProductExpiryComparer());
        }
    }
}
