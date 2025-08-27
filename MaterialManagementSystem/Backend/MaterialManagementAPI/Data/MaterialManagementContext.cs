using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MaterialManagementAPI.Models;

namespace MaterialManagementAPI.Data
{
    /// <summary>
    /// Entity Framework DbContext for the Material Management System
    /// Inherits from IdentityDbContext to include ASP.NET Core Identity tables
    /// </summary>
    public class MaterialManagementContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions
        /// </summary>
        /// <param name="options">Database context options</param>
        public MaterialManagementContext(DbContextOptions<MaterialManagementContext> options)
            : base(options)
        {
        }

        // ========== Core Business Entities ==========

        /// <summary>
        /// DbSet for Clients table
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// DbSet for ClientAddresses table
        /// </summary>
        public DbSet<ClientAddress> ClientAddresses { get; set; }

        /// <summary>
        /// DbSet for Suppliers table
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// DbSet for MaterialCategories table
        /// </summary>
        public DbSet<MaterialCategory> MaterialCategories { get; set; }

        /// <summary>
        /// DbSet for Materials table
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// DbSet for StockBatches table (FIFO implementation)
        /// </summary>
        public DbSet<StockBatch> StockBatches { get; set; }

        /// <summary>
        /// DbSet for StockMovements table
        /// </summary>
        public DbSet<StockMovement> StockMovements { get; set; }

        // ========== Sales Management ==========

        /// <summary>
        /// DbSet for InvoiceStatuses table
        /// </summary>
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }

        /// <summary>
        /// DbSet for SalesInvoices table
        /// </summary>
        public DbSet<SalesInvoice> SalesInvoices { get; set; }

        /// <summary>
        /// DbSet for SalesInvoiceItems table
        /// </summary>
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }

        /// <summary>
        /// DbSet for SalesCostDetails table (FIFO cost tracking)
        /// </summary>
        public DbSet<SalesCostDetail> SalesCostDetails { get; set; }

        // ========== Purchase Management ==========

        /// <summary>
        /// DbSet for PurchaseInvoices table
        /// </summary>
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

        /// <summary>
        /// DbSet for PurchaseInvoiceItems table
        /// </summary>
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }

        // ========== Payments and Collections ==========

        /// <summary>
        /// DbSet for PaymentMethods table
        /// </summary>
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        /// <summary>
        /// DbSet for Collections table (payments from clients)
        /// </summary>
        public DbSet<Collection> Collections { get; set; }

        /// <summary>
        /// DbSet for Payments table (payments to suppliers)
        /// </summary>
        public DbSet<Payment> Payments { get; set; }

        // ========== Expenses Management ==========

        /// <summary>
        /// DbSet for ExpenseTypes table
        /// </summary>
        public DbSet<ExpenseType> ExpenseTypes { get; set; }

        /// <summary>
        /// DbSet for Expenses table
        /// </summary>
        public DbSet<Expense> Expenses { get; set; }

        // ========== Employee Management ==========

        /// <summary>
        /// DbSet for Employees table
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// DbSet for Salaries table
        /// </summary>
        public DbSet<Salary> Salaries { get; set; }

        // ========== Equipment Management ==========

        /// <summary>
        /// DbSet for Equipments table
        /// </summary>
        public DbSet<Equipment> Equipments { get; set; }

        /// <summary>
        /// DbSet for EquipmentMaintenance table
        /// </summary>
        public DbSet<EquipmentMaintenance> EquipmentMaintenance { get; set; }

        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base method to configure Identity tables
            base.OnModelCreating(modelBuilder);

            // ========== Configure Unique Constraints ==========

            // Ensure MaterialCode is unique
            modelBuilder.Entity<Material>()
                .HasIndex(m => m.MaterialCode)
                .IsUnique();

            // Ensure Client phone is unique
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            // Ensure invoice numbers are unique
            modelBuilder.Entity<SalesInvoice>()
                .HasIndex(si => si.InvoiceNumber)
                .IsUnique();

            modelBuilder.Entity<PurchaseInvoice>()
                .HasIndex(pi => pi.PurchaseNumber)
                .IsUnique();

            modelBuilder.Entity<Collection>()
                .HasIndex(c => c.CollectionNumber)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentNumber)
                .IsUnique();

            modelBuilder.Entity<Expense>()
                .HasIndex(e => e.ExpenseNumber)
                .IsUnique();

            // ========== Configure Cascade Delete Behaviors ==========

            // ClientAddresses should be deleted when Client is deleted
            modelBuilder.Entity<ClientAddress>()
                .HasOne(ca => ca.Client)
                .WithMany(c => c.Addresses)
                .HasForeignKey(ca => ca.ClientID)
                .OnDelete(DeleteBehavior.Cascade);

            // SalesInvoiceItems should be deleted when SalesInvoice is deleted
            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(sii => sii.Invoice)
                .WithMany(si => si.Items)
                .HasForeignKey(sii => sii.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            // SalesCostDetails should be deleted when SalesInvoiceItem is deleted
            modelBuilder.Entity<SalesCostDetail>()
                .HasOne(scd => scd.SalesItem)
                .WithMany(sii => sii.CostDetails)
                .HasForeignKey(scd => scd.SalesItemID)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseInvoiceItems should be deleted when PurchaseInvoice is deleted
            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(pii => pii.Purchase)
                .WithMany(pi => pi.Items)
                .HasForeignKey(pii => pii.PurchaseID)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== Configure Computed Columns ==========
            // Note: Computed columns are handled by database, but we can configure them here if needed

            // ========== Seed Initial Data ==========
            SeedInitialData(modelBuilder);
        }

        /// <summary>
        /// Seed initial lookup data into the database
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            // Seed Invoice Statuses
            modelBuilder.Entity<InvoiceStatus>().HasData(
                new InvoiceStatus { StatusID = 1, StatusName = "مسودة" },
                new InvoiceStatus { StatusID = 2, StatusName = "مؤكدة" },
                new InvoiceStatus { StatusID = 3, StatusName = "مدفوعة جزئياً" },
                new InvoiceStatus { StatusID = 4, StatusName = "مدفوعة كاملة" },
                new InvoiceStatus { StatusID = 5, StatusName = "ملغية" }
            );

            // Seed Payment Methods
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { MethodID = 1, MethodName = "نقد" },
                new PaymentMethod { MethodID = 2, MethodName = "شيك" },
                new PaymentMethod { MethodID = 3, MethodName = "تحويل بنكي" },
                new PaymentMethod { MethodID = 4, MethodName = "بطاقة ائتمان" },
                new PaymentMethod { MethodID = 5, MethodName = "فودافون كاش" }
            );

            // Seed Material Categories
            modelBuilder.Entity<MaterialCategory>().HasData(
                new MaterialCategory { CategoryID = 1, CategoryName = "حديد وصلب", Description = "حديد التسليح والصلب" },
                new MaterialCategory { CategoryID = 2, CategoryName = "أسمنت ومونة", Description = "الأسمنت والمونة الجاهزة" },
                new MaterialCategory { CategoryID = 3, CategoryName = "الطوب والبلوك", Description = "طوب أحمر وطوب أبيض وبلوك" },
                new MaterialCategory { CategoryID = 4, CategoryName = "رمل وزلط", Description = "الرمل والزلط والركام" },
                new MaterialCategory { CategoryID = 5, CategoryName = "دهانات", Description = "البويات والدهانات" },
                new MaterialCategory { CategoryID = 6, CategoryName = "سباكة", Description = "مواسير وخلاطات وأدوات السباكة" },
                new MaterialCategory { CategoryID = 7, CategoryName = "كهرباء", Description = "أسلاك ومفاتيح وأدوات كهربائية" },
                new MaterialCategory { CategoryID = 8, CategoryName = "أدوات", Description = "أدوات البناء والمعدات الصغيرة" }
            );

            // Seed Expense Types
            modelBuilder.Entity<ExpenseType>().HasData(
                new ExpenseType { TypeID = 1, TypeName = "كهرباء", Description = "فواتير الكهرباء" },
                new ExpenseType { TypeID = 2, TypeName = "مياه", Description = "فواتير المياه" },
                new ExpenseType { TypeID = 3, TypeName = "إيجار", Description = "إيجار المخزن أو المكتب" },
                new ExpenseType { TypeID = 4, TypeName = "مواصلات", Description = "مصاريف النقل والتوصيل" },
                new ExpenseType { TypeID = 5, TypeName = "صيانة", Description = "صيانة المعدات والمكان" },
                new ExpenseType { TypeID = 6, TypeName = "اتصالات", Description = "فواتير التليفون والإنترنت" },
                new ExpenseType { TypeID = 7, TypeName = "وقود", Description = "بنزين وسولار للمعدات" },
                new ExpenseType { TypeID = 8, TypeName = "أخرى", Description = "مصروفات متنوعة" }
            );
        }
    }
}

