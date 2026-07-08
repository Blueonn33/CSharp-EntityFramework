using BookShop.Models.Enums;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            DbInitializer.ResetDatabase(dbContext);

            string command = Console.ReadLine()!;
            string result = GetBooksByAgeRestriction(dbContext, command);

            Console.WriteLine(result);
        }

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
                .Where(b => b.AgeRestriction == ageRestriction.Value)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, ageRestrictedBooks);
        }

        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLowerInvariant())
                .ToArray();

            IEnumerable<string> bookTitlesFromCategories = dbContext.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitlesFromCategories);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            IEnumerable<string> authorFullNames = dbContext.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => a.FirstName + " " + a.LastName)
                .ToArray();

            return string.Join(Environment.NewLine, authorFullNames);
        }
    }
}