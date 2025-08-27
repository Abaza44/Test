using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an expense type in the material management system
    /// Maps to the ExpenseTypes table in the database
    /// </summary>
    [Table("ExpenseTypes")]
    public class ExpenseType
    {
        /// <summary>
        /// Primary key for the expense type
        /// </summary>
        [Key]
        public int TypeID { get; set; }

        /// <summary>
        /// Expense type name (required, max 100 characters)
        /// Examples: كهرباء، مياه، إيجار، مواصلات، صيانة، اتصالات، وقود، أخرى
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the expense type (max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property for expenses of this type
        /// One expense type can have multiple expenses
        /// </summary>
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}

