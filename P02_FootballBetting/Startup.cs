using P02_FootballBetting.Data;

public class Startup
{
    static void Main(string[] args)
    {
        try
        {
            using FootballBettingContext dbContext = new();

            // Db only solution
            // First drop any existing database to ensure sync with latest changes
            dbContext.Database.EnsureDeleted();

            // Try to create the database from the latest Code First model
            dbContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}