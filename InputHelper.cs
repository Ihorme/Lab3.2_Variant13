using System;
using Models;

namespace DemoApp
{
    public static class InputHelper
    {
        // Читати новий товар із перевіркою
        public static ProductDto? ReadProductDtoFromConsole()
        {
            Console.Write("Назва (або пусто для скасування): ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return null;

            Console.Write("Код (унікальний) : ");
            var code = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(code)) return null;

            var prodDate = ReadDateOrDefault("Дата виготовлення (рррр-mm-dd): ");
            var expDate = ReadDateOrDefault("Термін придатності (рррр-mm-dd): ");

            if (expDate < prodDate)
            {
                Console.WriteLine("Помилка: термін придатності раніше дати виготовлення. Скасовано.");
                return null;
            }

            return new ProductDto
            {
                Name = name.Trim(),
                Code = code.Trim(),
                ProductionDate = prodDate,
                ExpiryDate = expDate
            };
        }

        public static ProductDto? ReadProductDtoForEdit(Models.Product existing)
        {
            Console.Write($"Назва ({existing.Name}): ");
            var name = Console.ReadLine();
            var finalName = string.IsNullOrWhiteSpace(name) ? existing.Name : name.Trim();

            var prodDate = ReadDateOrDefault($"Дата виготовлення ({existing.ProductionDate:yyyy-MM-dd}): ", existing.ProductionDate);
            var expDate = ReadDateOrDefault($"Термін придатності ({existing.ExpiryDate:yyyy-MM-dd}): ", existing.ExpiryDate);

            if (expDate < prodDate)
            {
                Console.WriteLine("Помилка: термін придатності раніше дати виготовлення. Скасовано.");
                return null;
            }

            return new ProductDto
            {
                Name = finalName,
                Code = existing.Code,
                ProductionDate = prodDate,
                ExpiryDate = expDate
            };
        }

        private static DateTime ReadDateOrDefault(string prompt, DateTime? defaultValue = null)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s))
                {
                    if (defaultValue.HasValue) return defaultValue.Value;
                    Console.WriteLine("Це поле обов'язкове. Введіть дату у форматі yyyy-mm-dd.");
                    continue;
                }
                if (DateTime.TryParse(s.Trim(), out var d)) return d.Date;
                Console.WriteLine("Неправильний формат дати. Спробуйте ще раз.");
            }
        }
    }
}
