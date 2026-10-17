using LSMS.Domain.Entity;
using LSMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence
{
    public class LSMSDbContext:IdentityDbContext<AppUser>
    {
        public LSMSDbContext(DbContextOptions<LSMSDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(LSMSDbContext).Assembly);
        }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Supplier> Suppliers => Set<Supplier>();

        public DbSet<Purchase> Purchases => Set<Purchase>();

        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

        public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();

        public DbSet<Sale> Sales => Set<Sale>();

        public DbSet<SaleItem> SaleItems => Set<SaleItem>();

        public DbSet<Return> Returns => Set<Return>();

        public DbSet<StockMovement> StockMovements => Set<StockMovement>();

        public DbSet<CashMovement> CashMovements => Set<CashMovement>();

        public DbSet<Expense> Expenses => Set<Expense>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    }
}
