using Botticelli.Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Data.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Adds auth database model for Botticelli Botticelli.Auth
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static ModelBuilder AddAuthenticationModels(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotUser>()
                    .HasKey(u => u.UserId);
        
        modelBuilder.Entity<BotUserRole>()
                    .HasMany<BotUser>()
                    .WithOne(r => r.Role)
                    .HasPrincipalKey(r => r.Id);
        
        modelBuilder.Entity<BotUserRole>()
                    .HasIndex(i => i.RoleName)
                    .IsUnique();
        
        return modelBuilder;
    }
}