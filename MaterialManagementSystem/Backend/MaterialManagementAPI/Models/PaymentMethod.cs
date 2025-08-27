using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a payment method in the material management system
    /// Maps to the PaymentMethods table in the database
    /// </summary>
    [Table("PaymentMethods")]
    public class PaymentMethod
    {
        /// <summary>
        /// Primary key for the payment method
        /// </summary>
        [Key]
        public int MethodID { get; set; }

        /// <summary>
        /// Payment method name (required, max 50 characters)
        /// Examples: نقد، شيك، تحويل بنكي، بطاقة ائتمان، فودافون كاش
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MethodName { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for collections using this payment method
        /// One payment method can be used in multiple collections
        /// </summary>
        public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

        /// <summary>
        /// Navigation property for payments using this payment method
        /// One payment method can be used in multiple payments
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        /// <summary>
        /// Navigation property for expenses using this payment method
        /// One payment method can be used in multiple expenses
        /// </summary>
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}

