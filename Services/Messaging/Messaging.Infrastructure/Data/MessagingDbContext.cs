using Messaging.Core.Entities.MessageAggregate;
using Messaging.Core.Entities.UserAggregate;
using Messaging.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging.Infrastructure.Data
{
    public class MessagingDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<BlockedPeople> BlockedUsers { get; set; }
        public DbSet<User> Users { get; set; }

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(b => b.SenderId)
                 .IsRequired();

                entity.Property(b => b.ReceiverId)
                 .IsRequired();

                entity.Property(b => b.MessageText)
                 .IsRequired();

            });

            modelBuilder.Entity<BlockedPeople>(entity =>
            {
                entity.HasKey(b => b.BlockingUserId);

                entity.Property(b => b.BlockingUserId)
                 .IsRequired();

                entity.Property(b => b.BlockedUserId)
                 .IsRequired();

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                 .ValueGeneratedNever()
                 .IsRequired();

                entity.Property(e => e.UserName);

                entity.HasMany(e => e.UsersBlockedByUser)
                    .WithOne(e => e.BlockingUser)
                    .HasForeignKey(e => e.BlockingUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.UsersBlockUser)
                    .WithOne(e => e.BlockedUser)
                    .HasForeignKey(e => e.BlockedUserId)
                    .HasForeignKey(e => e.BlockingUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync();

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
