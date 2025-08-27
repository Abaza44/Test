using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an expense in the material management system
    /// Maps to the Expenses table in the database
    /// </summary>
    [Table("Expenses")]
    public class Expense
    {
        /// <summary>
        /// Primary key for the expense
        /// </summary>
        [Key]
        public int ExpenseID { get; set; }

        /// <summary>
        /// Unique expense number (required, max 20 characters)
        /// Used for expense identification and reference
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ExpenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the expense type
        /// </summary>
        [Required]
        public int TypeID { get; set; }

        /// <summary>
        /// Date when the expense occurred
        /// </summary>
        public DateTime ExpenseDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Amount of the expense
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Foreign key reference to the payment method used (optional)
        /// </summary>
        public int? PaymentMethodID { get; set; }

        /// <summary>
        /// Description of the expense (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Additional notes about the expense (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the expense type
        /// </summary>
        [ForeignKey("TypeID")]
        public virtual ExpenseType Type { get; set; } = null!;

        /// <summary>
        /// Navigation property to the payment method
        /// </summary>
        [ForeignKey("PaymentMethodID")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}

