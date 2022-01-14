// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Ch10EFCoreDemo;
using static System.Console;

Console.WriteLine("Hello, World!");

Console.WriteLine($"Using {ProjectConstants.DatabaseProvider} database provider.");

//QueryingCategories();
FilteredIncludes();


static void QueryingCategories()
{
    using (Northwind db = new())
    {
        Console.WriteLine("Categories and how many products they have:");
        // a query to get all categories and their related products
        IQueryable<Category>? categories = db.Categories?
        .Include(c => c.Products);
        if (categories is null)
        {
            Console.WriteLine("No categories found.");
            return;
        }
        // execute query and enumerate results
        foreach (Category c in categories)
        {
            Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }
    }
}

static void FilteredIncludes()
{
    using (Northwind db = new())
    {
        Write("Enter a minimum for units in stock: ");
        string unitsInStock = ReadLine() ?? "10";
        int stock = int.Parse(unitsInStock);
        IQueryable<Category>? categories = db.Categories?
        .Include(c => c.Products.Where(p => p.Stock >= stock));
        if (categories is null)
        {
            WriteLine("No categories found.");
            return;
        }
        foreach (Category c in categories)
        {
            Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of { stock} units in stock.");
            foreach (Product p in c.Products)
            {
                Console.WriteLine($" {p.ProductName} has {p.Stock} units in stock.");
            }
        }
    }
}