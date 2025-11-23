using System;

namespace DemoApp
{
    // Маленька DTO для читання з консолі і валідації
    public class ProductDto
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}