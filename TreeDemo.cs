using System;
using System.Linq;
using Models;
using DataStructures;
using DataStructures.Comparers;

namespace DemoApp
{
    // Проста демонстрація BinaryTree<Product>
    public class TreeDemo
    {
        private readonly BinaryTree<Product> _tree;

        public TreeDemo()
        {
            // Будуємо дерево з порівнячем за ExpiryDate
            _tree = new BinaryTree<Product>(new ProductExpiryComparer());

            // Початкові елементи
            var initial = new[]
            {
                new Product("Apple", "A01", new DateTime(2025,9,1), new DateTime(2025,10,24)),
                new Product("Banana", "B01", new DateTime(2025,10,5), new DateTime(2025,10,22)),
                new Product("Cherry", "C01", new DateTime(2025,8,1), new DateTime(2026,1,1)),
                new Product("Date", "D01", new DateTime(2025,9,20), new DateTime(2025,10,30))
            };
            foreach (var p in initial) _tree.Add(p);
        }

        public void ShowTree()
        {
            Console.WriteLine("Перебір дерева (preorder):");
            var any = false;
            foreach (var p in _tree)
            {
                any = true;
                Console.WriteLine(p);
            }
            if (!any) Console.WriteLine("Дерево порожнє.");
        }

        public void Add(Product p) => _tree.Add(p);

        // Точка: BinaryTree порівнює за ExpiryDate — для перевірки Contains створюємо тимчасовий Product з тим же ExpiryDate
        public bool ContainsByCode(string code)
        {
            // Маємо тільки код; спробуємо знайти елемент в preorder який має такий код
            return _tree.Any(node => string.Equals(node.Code, code, StringComparison.OrdinalIgnoreCase));
        }
    }

    // Додаємо extension Any для BinaryTree (коли IEnumerable<T> доступний — можна використовувати LINQ)
    internal static class TreeExtensions
    {
        public static bool Any<T>(this BinaryTree<T> tree, Func<T, bool> predicate) where T : class
        {
            foreach (var v in tree)
            {
                if (predicate(v)) return true;
            }
            return false;
        }
    }
}
