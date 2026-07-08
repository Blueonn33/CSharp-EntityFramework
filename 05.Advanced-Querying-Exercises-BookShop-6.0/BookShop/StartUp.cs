using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BookShop
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContext);

            //string command = Console.ReadLine()!;
            string result = CountCopiesByAuthor(dbContext);

            Console.WriteLine(result);
        }

        // -- 02
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command)
        {
            AgeRestriction? ageRestriction = null;

            if (command != null && Enum.GetValues<AgeRestriction>().Any(ev => ev.ToString().ToLowerInvariant() == command.ToLowerInvariant()))
            {
                ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            }

            if (!ageRestriction.HasValue)
            {
                return string.Empty;
            }

            IEnumerable<string> ageRestrictedBooks = dbContext.Books
                .AsNoTracking()
                .Where(b => b.AgeRestriction == ageRestriction.Value)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, ageRestrictedBooks);
        }

        // -- 06
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLowerInvariant())
                .ToArray();

            IEnumerable<string> bookTitlesFromCategories = dbContext.Books
                .AsNoTracking()
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitlesFromCategories);
        }

        // -- 08
        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            IEnumerable<string> authorFullNames = dbContext.Authors
                .AsNoTracking()
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => a.FirstName + " " + a.LastName)
                .ToArray();

            return string.Join(Environment.NewLine, authorFullNames);
        }

        // -- 12
        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            StringBuilder sb = new();
            var authorBookCopies = dbContext.Authors
                .AsNoTracking()
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalBookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBookCopies)
                .ToArray();

            foreach (var author in authorBookCopies)
            {
                sb
                    .AppendLine($"{author.FirstName} {author.LastName} - {author.TotalBookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        // -- 13
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            StringBuilder sb = new();

            var categoriesTotalProfit = dbContext.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                        .Select(cb => cb.Book)
                        .Sum(b => b.Price * b.Copies)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            foreach (var category in categoriesTotalProfit)
            {
                sb.AppendLine($"{category.Name} ${category.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        // -- 15
        public static void IncreasePrices(BookShopContext dbContext)
        {
            // I. Standard Approach
            IQueryable<Book> booksToUpdate = dbContext.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010);

            foreach (var book in booksToUpdate)
            {
                book.Price += 5;
            }

            dbContext.SaveChanges();
        }
    }
}