using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure.Data;

public class MarketplaceContextSeed
{
    public static async Task SeedAsync(MarketplaceDbContext context, ILogger<MarketplaceContextSeed> logger)
    {
        try
        {
            // Check if an Admin already exists to prevent duplicate seeding
            if (!await context.Users.AnyAsync(u => u.Role == "Admin"))
            {
                var adminUser = new User
                {
                    Username = "SystemAdmin",
                    Email = "admin@marketplace.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperSecretAdminPassword123!"),
                    Role = "Admin"
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();

                logger.LogInformation("Successfully seeded the default Admin user.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
