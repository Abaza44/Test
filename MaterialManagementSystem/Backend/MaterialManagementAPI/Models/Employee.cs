using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an employee in the material management system
    /// Maps to the Employees table in the database
    /// </summary>
    [Table("Employees")]
    public class Employee
    {
        /// <summary>
        /// Primary key for the employee
        /// </summary>
        [Key]
        public int EmployeeID { get; set; }

        /// <summary>
        /// Employee's full name (required, max 100 characters)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Employee's job title (optional, max 100 characters)
        /// </summary>
        [StringLength(100)]
        public string? JobTitle { get; set; }

        /// <summary>
        /// Employee's phone number (optional, max 20 characters)
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Date when the employee was hired (optional)
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Employee's monthly salary amount (optional)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MonthlySalary { get; set; }

        /// <summary>
        /// Indicates whether the employee is active in the system
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property for salary records
        /// One employee can have multiple salary records
        /// </summary>
        public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
    }
}

