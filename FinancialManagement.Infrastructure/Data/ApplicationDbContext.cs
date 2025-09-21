using Microsoft.EntityFrameworkCore;
using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalEntryLine> JournalEntryLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("User");
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            // ChartOfAccount Configuration
            modelBuilder.Entity<ChartOfAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AccountName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AccountType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
                entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
                entity.HasIndex(e => e.AccountCode).IsUnique();

                // Self-referencing relationship
                entity.HasOne(e => e.ParentAccount)
                      .WithMany(e => e.ChildAccounts)
                      .HasForeignKey(e => e.ParentAccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // JournalEntry Configuration
            modelBuilder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JournalNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Reference).HasMaxLength(100);
                entity.Property(e => e.TotalDebit).HasPrecision(18, 2);
                entity.Property(e => e.TotalCredit).HasPrecision(18, 2);
                entity.HasIndex(e => e.JournalNumber).IsUnique();

                // Relationship with User
                entity.HasOne(e => e.User)
                      .WithMany(e => e.JournalEntries)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // JournalEntryLine Configuration
            modelBuilder.Entity<JournalEntryLine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Reference).HasMaxLength(100);
                entity.Property(e => e.DebitAmount).HasPrecision(18, 2);
                entity.Property(e => e.CreditAmount).HasPrecision(18, 2);

                // Relationship with JournalEntry
                entity.HasOne(e => e.JournalEntry)
                      .WithMany(e => e.JournalEntryLines)
                      .HasForeignKey(e => e.JournalEntryId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship with ChartOfAccount
                entity.HasOne(e => e.Account)
                      .WithMany(e => e.JournalEntryLines)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Use static date to avoid migration issues
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed default admin user (password will be hashed in service)
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@financialsystem.com",
                PasswordHash = "$2a$11$5Zn5qh.cJ5Z5Zn5qh.cJ5uOEbF8N8vGxOzJkP3bwqzYyYxzZyZzZy", // bcrypt hash for "admin123"
                FirstName = "System",
                LastName = "Admin",
                Role = "Admin",
                CreatedBy = "System",
                CreatedDate = seedDate
            });

            // Seed basic chart of accounts
            modelBuilder.Entity<ChartOfAccount>().HasData(
                new ChartOfAccount
                {
                    Id = 1,
                    AccountCode = "1000",
                    AccountName = "Assets",
                    AccountType = "Asset",
                    Description = "All company assets",
                    IsParent = true,
                    Level = 1,
                    CreatedBy = "System",
                    CreatedDate = seedDate
                },
                new ChartOfAccount
                {
                    Id = 2,
                    AccountCode = "2000",
                    AccountName = "Liabilities",
                    AccountType = "Liability",
                    Description = "All company liabilities",
                    IsParent = true,
                    Level = 1,
                    CreatedBy = "System",
                    CreatedDate = seedDate
                },
                new ChartOfAccount
                {
                    Id = 3,
                    AccountCode = "3000",
                    AccountName = "Equity",
                    AccountType = "Equity",
                    Description = "Owner's equity",
                    IsParent = true,
                    Level = 1,
                    CreatedBy = "System",
                    CreatedDate = seedDate
                },
                new ChartOfAccount
                {
                    Id = 4,
                    AccountCode = "4000",
                    AccountName = "Revenue",
                    AccountType = "Revenue",
                    Description = "Company revenues",
                    IsParent = true,
                    Level = 1,
                    CreatedBy = "System",
                    CreatedDate = seedDate
                },
                new ChartOfAccount
                {
                    Id = 5,
                    AccountCode = "5000",
                    AccountName = "Expenses",
                    AccountType = "Expense",
                    Description = "Company expenses",
                    IsParent = true,
                    Level = 1,
                    CreatedBy = "System",
                    CreatedDate = seedDate
                }
            );
        }
    }
}