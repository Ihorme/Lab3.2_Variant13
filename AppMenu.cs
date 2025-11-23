using System;
using System.Linq;
using Models;

namespace DemoApp
{
    public class AppMenu
    {
        private readonly ProductRepository _repo;
        private readonly TreeDemo _treeDemo;

        public AppMenu(ProductRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _treeDemo = new TreeDemo();
            // Початкові дані (щоб користувач одразу мав з чим працювати)
            SeedSampleData();
        }

        private void SeedSampleData()
        {
            if (!_repo.GetAll().Any())
            {
                _repo.Add(new Product("Milk", "P001", new DateTime(2025, 10, 1), new DateTime(2025, 10, 28)));
                _repo.Add(new Product("Cheese", "P002", new DateTime(2025, 8, 15), new DateTime(2026, 2, 15)));
                _repo.Add(new Product("Bread", "P003", new DateTime(2025, 10, 20), new DateTime(2025, 10, 25)));
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Меню операцій з товарами (варіант 13) ===");
                Console.WriteLine("1. Показати всі товари (List)");
                Console.WriteLine("2. Додати товар");
                Console.WriteLine("3. Редагувати товар (за кодом)");
                Console.WriteLine("4. Видалити товар (за кодом)");
                Console.WriteLine("5. Пошук товару (за кодом або назвою)");
                Console.WriteLine("6. Показати масив Product[]");
                Console.WriteLine("7. Показати ArrayList");
                Console.WriteLine("8. Сортування (за назвою або за терміном придатності)");
                Console.WriteLine("9. BinaryTree: показати, додати, перевірити Contains (preorder)");
                Console.WriteLine("0. Вийти");
                Console.Write("Оберіть опцію: ");

                var key = Console.ReadLine();
                Console.WriteLine();

                switch (key)
                {
                    case "1":
                        ShowAll();
                        break;
                    case "2":
                        AddProduct();
                        break;
                    case "3":
                        EditProduct();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        SearchProduct();
                        break;
                    case "6":
                        ShowArrayDemo();
                        break;
                    case "7":
                        ShowArrayListDemo();
                        break;
                    case "8":
                        SortMenu();
                        break;
                    case "9":
                        TreeMenu();
                        break;
                    case "0":
                        Console.WriteLine("Вихід. До побачення!");
                        return;
                    default:
                        Console.WriteLine("Невірна опція. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private void ShowAll()
        {
            Console.WriteLine("Список товарів (List):");
            var all = _repo.GetAll();
            if (!all.Any()) Console.WriteLine("Список порожній.");
            else foreach (var p in all) Console.WriteLine(p);
        }

        private void AddProduct()
        {
            Console.WriteLine("=== Додавання товару ===");
            var dto = InputHelper.ReadProductDtoFromConsole();
            if (dto == null)
            {
                Console.WriteLine("Скасовано додавання.");
                return;
            }
            if (_repo.Exists(dto.Code))
            {
                Console.WriteLine($"Товар з кодом {dto.Code} вже існує. Спробуйте оновити його замість додавання.");
                return;
            }
            var p = new Product(dto.Name, dto.Code, dto.ProductionDate, dto.ExpiryDate);
            _repo.Add(p);
            Console.WriteLine("Товар додано успішно.");
        }

        private void EditProduct()
        {
            Console.Write("Введіть код товару для редагування: ");
            var code = Console.ReadLine()?.Trim() ?? "";
            var existing = _repo.FindByCode(code);
            if (existing == null)
            {
                Console.WriteLine("Товар не знайдено.");
                return;
            }
            Console.WriteLine("Поточний товар: " + existing);
            Console.WriteLine("Введіть нові значення (порожній ввід => зберігається старе):");

            var dto = InputHelper.ReadProductDtoForEdit(existing);
            if (dto == null)
            {
                Console.WriteLine("Скасовано редагування.");
                return;
            }

            existing.Name = dto.Name;
            existing.ProductionDate = dto.ProductionDate;
            existing.ExpiryDate = dto.ExpiryDate;
            // Код ми не змінюємо (ідентифікатор)
            Console.WriteLine("Редагування збережено.");
        }

        private void DeleteProduct()
        {
            Console.Write("Введіть код товару для видалення: ");
            var code = Console.ReadLine()?.Trim() ?? "";
            if (!_repo.Exists(code))
            {
                Console.WriteLine("Товар не знайдено.");
                return;
            }
            _repo.Remove(code);
            Console.WriteLine("Товар видалено.");
        }

        private void SearchProduct()
        {
            Console.WriteLine("Пошук: 1 - за кодом, 2 - за назвою (підрядок)");
            var ch = Console.ReadLine();
            if (ch == "1")
            {
                Console.Write("Введіть код: ");
                var code = Console.ReadLine()?.Trim() ?? "";
                var p = _repo.FindByCode(code);
                Console.WriteLine(p != null ? p.ToString() : "Не знайдено.");
            }
            else if (ch == "2")
            {
                Console.Write("Введіть частину назви: ");
                var q = Console.ReadLine() ?? "";
                var found = _repo.FindByNameContains(q);
                if (!found.Any()) Console.WriteLine("Не знайдено.");
                else foreach (var p in found) Console.WriteLine(p);
            }
            else Console.WriteLine("Невірна опція.");
        }

        private void ShowArrayDemo()
        {
            Console.WriteLine("Демонстрація Product[] (масив):");
            var arr = _repo.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"[{i}] {arr[i]}");
            }
            Console.WriteLine("Щоб додати в масив, потрібно створити новий масив — ця операція не робиться автоматично в меню.");
        }

        private void ShowArrayListDemo()
        {
            Console.WriteLine("Демонстрація ArrayList (non-generic):");
            var al = _repo.ToArrayList();
            for (int i = 0; i < al.Count; i++)
            {
                var o = al[i];
                Console.WriteLine($"[{i}] {o}");
            }
        }

        private void SortMenu()
        {
            Console.WriteLine("Сортування: 1 - за назвою (IComparable), 2 - за терміном придатності (IComparer)");
            var ch = Console.ReadLine();
            if (ch == "1")
            {
                _repo.SortByName();
                Console.WriteLine("Виконано сортування за назвою.");
            }
            else if (ch == "2")
            {
                _repo.SortByExpiry();
                Console.WriteLine("Виконано сортування за терміном придатності.");
            }
            else Console.WriteLine("Невірна опція.");
        }

        private void TreeMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== BinaryTree Menu (preorder) ===");
                Console.WriteLine("1. Показати дерево (preorder)");
                Console.WriteLine("2. Додати товар у дерево");
                Console.WriteLine("3. Перевірити Contains (за терміном придатності/коду)");
                Console.WriteLine("0. Повернутись в головне меню");
                Console.Write("Оберіть опцію: ");
                var ch = Console.ReadLine();
                if (ch == "1")
                {
                    _treeDemo.ShowTree();
                }
                else if (ch == "2")
                {
                    var dto = InputHelper.ReadProductDtoFromConsole();
                    if (dto != null)
                    {
                        var p = new Product(dto.Name, dto.Code, dto.ProductionDate, dto.ExpiryDate);
                        _treeDemo.Add(p);
                        Console.WriteLine("Додано в дерево.");
                    }
                }
                else if (ch == "3")
                {
                    Console.Write("Введіть код товару для перевірки (tree.Contains порівнює залежно від реалізації): ");
                    var code = Console.ReadLine() ?? "";
                    var found = _treeDemo.ContainsByCode(code);
                    Console.WriteLine(found ? "У дереві є елемент з таким кодом/терміном." : "Не знайдено в дереві.");
                }
                else if (ch == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Невірна опція.");
                }
            }
        }
    }
}