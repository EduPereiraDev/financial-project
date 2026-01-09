using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountMember> AccountMembers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<BankConnection> BankConnections { get; set; }
    public DbSet<BankTransaction> BankTransactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<GoalContribution> GoalContributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        // Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).HasConversion<string>();

            entity.HasOne(e => e.Owner)
                .WithMany(u => u.OwnedAccounts)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AccountMember
        modelBuilder.Entity<AccountMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Role).HasConversion<string>();

            entity.HasOne(e => e.Account)
                .WithMany(a => a.Members)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.AccountMemberships)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.AccountId, e.UserId }).IsUnique();
        });

        // Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Type).HasConversion<string>();

            entity.HasOne(e => e.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.AccountId);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Type).HasConversion<string>();

            entity.HasOne(e => e.Account)
                .WithMany(a => a.Categories)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RecurringTransaction
        modelBuilder.Entity<RecurringTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Frequency).HasConversion<string>();

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(rt => rt.NextExecutionDate);
            entity.HasIndex(e => e.IsActive);
        });

        // Invitation
        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvitedEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Role).IsRequired();

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InvitedByUser)
                .WithMany()
                .HasForeignKey(e => e.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.InvitedEmail);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ExpiresAt);
        });

        // Alert
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.IsActive);
        });

        // Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.IsRead).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Alert)
                .WithMany()
                .HasForeignKey(e => e.AlertId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.CreatedAt);
        });

        // BankConnection
        modelBuilder.Entity<BankConnection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BankName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.BankCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.InstitutionId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ItemId).IsRequired().HasMaxLength(255);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ItemId);
            entity.HasIndex(e => e.Status);
        });

        // BankTransaction
        modelBuilder.Entity<BankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExternalId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Amount).HasPrecision(18, 2);

            entity.HasOne(e => e.BankConnection)
                .WithMany(bc => bc.BankTransactions)
                .HasForeignKey(e => e.BankConnectionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Transaction)
                .WithMany()
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.BankConnectionId);
            entity.HasIndex(e => e.ExternalId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Date);
        });

        // Budget
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period).HasConversion<string>();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => new { e.UserId, e.CategoryId, e.Month, e.Year }).IsUnique();
        });

        // Goal
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.TargetAmount).HasPrecision(18, 2);
            entity.Property(e => e.CurrentAmount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.ImageUrl).HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.TargetDate);
        });

        // GoalContribution
        modelBuilder.Entity<GoalContribution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasOne(e => e.Goal)
                .WithMany(g => g.Contributions)
                .HasForeignKey(e => e.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.GoalId);
            entity.HasIndex(e => e.ContributedAt);
        });
    }
}
