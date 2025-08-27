using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a salary payment record in the material management system
    /// Maps to the Salaries table in the database
    /// </summary>
    [Table("Salaries")]
    public class Salary
    {
        /// <summary>
        /// Primary key for the salary record
        /// </summary>
        [Key]
        public int SalaryID { get; set; }

        /// <summary>
        /// Foreign key reference to the employee
        /// </summary>
        [Required]
        public int EmployeeID { get; set; }

        /// <summary>
        /// Month and year for which this salary is paid
        /// Stored as the first day of the month (e.g., 2024-01-01 for January 2024)
        /// </summary>
        [Required]
        public DateTime SalaryMonth { get; set; }

        /// <summary>
        /// Base salary amount for the month
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? BaseSalary { get; set; }

        /// <summary>
        /// Overtime payment amount (default 0)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Overtime { get; set; } = 0;

        /// <summary>
        /// Bonus amount (default 0)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Bonus { get; set; } = 0;

        /// <summary>
        /// Deductions amount (default 0)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Deductions { get; set; } = 0;

        /// <summary>
        /// Net salary (calculated as BaseSalary + Overtime + Bonus - Deductions)
        /// This is a computed column in the database
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal NetSalary { get; set; }

        /// <summary>
        /// Date when the salary was actually paid (optional)
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Additional notes about the salary payment (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the employee
        /// </summary>
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; } = null!;
    }
}

