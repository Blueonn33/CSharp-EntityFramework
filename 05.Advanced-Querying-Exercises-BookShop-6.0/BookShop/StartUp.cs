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
    }
}


