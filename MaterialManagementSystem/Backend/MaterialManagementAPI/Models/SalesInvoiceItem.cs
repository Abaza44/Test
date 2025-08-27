using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents an item within a sales invoice
    /// Maps to the SalesInvoiceItems table in the database
    /// </summary>
    [Table("SalesInvoiceItems")]
    public class SalesInvoiceItem
    {
        /// <summary>
        /// Primary key for the sales invoice item
        /// </summary>
        [Key]
        public int ItemID { get; set; }

        /// <summary>
        /// Foreign key reference to the sales invoice
        /// </summary>
        [Required]
        public int InvoiceID { get; set; }

        /// <summary>
        /// Foreign key reference to the material being sold
        /// </summary>
        [Required]
        public int MaterialID { get; set; }

        /// <summary>
        /// Quantity of the material being sold
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit selling price for this item
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Line total (calculated as Quantity * UnitPrice)
        /// This is a computed column in the database
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LineTotal { get; set; }

        /// <summary>
        /// Navigation property to the sales invoice
        /// </summary>
        [ForeignKey("InvoiceID")]
        public virtual SalesInvoice Invoice { get; set; } = null!;

        /// <summary>
        /// Navigation property to the material
        /// </summary>
        [ForeignKey("MaterialID")]
        public virtual Material Material { get; set; } = null!;

        /// <summary>
        /// Navigation property for sales cost details (FIFO breakdown)
        /// One sales item can have multiple cost details from different batches
        /// </summary>
        public virtual ICollection<SalesCostDetail> CostDetails { get; set; } = new List<SalesCostDetail>();
    }
}

