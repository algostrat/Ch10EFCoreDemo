using Microsoft.EntityFrameworkCore; // DbContext, DbContextOptionsBuilder
using static System.Console;
namespace Ch10EFCoreDemo;

// this manages the connection to the database
public class Northwind : DbContext
{

    // These properties map to tables in the database
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }

    protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
    {
        if (ProjectConstants.DatabaseProvider == "SQLite")
        {
            string path = Path.Combine(
            Environment.CurrentDirectory, "Northwind.db");
            WriteLine($"Using {path} database file.");
            optionsBuilder.UseSqlite($"Filename={path}");
        }
        else
        {
            string connection = "Data Source=.;" +
            "Initial Catalog=Northwind;" +
            "Integrated Security=true;" +
            "MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connection);
        }
    }

    // The DbContext-derived class can optionally have an overridden method named
    // OnModelCreating. This is where you can write Fluent API statements as an alternative to
    // decorating your entity classes with attributes.
    protected override void OnModelCreating(
ModelBuilder modelBuilder)
    {
        // example of using Fluent API instead of attributes
        // to limit the length of a category name to 15
        modelBuilder.Entity<Category>()
        .Property(category => category.CategoryName)
        .IsRequired() // NOT NULL
        .HasMaxLength(15);
        if (ProjectConstants.DatabaseProvider == "SQLite")
        {
            // added to "fix" the lack of decimal support in SQLite
            modelBuilder.Entity<Product>()
            .Property(product => product.Cost)
            .HasConversion<double>();
        }
    }


}