using System;

namespace DemoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var repo = new ProductRepository();
            var menu = new AppMenu(repo);
            menu.Run();
        }
    }
}