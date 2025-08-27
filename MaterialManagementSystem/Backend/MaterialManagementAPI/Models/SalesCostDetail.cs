using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents the cost breakdown for a sales item using FIFO methodology
    /// This table tracks which stock batches were used for each sale and their costs
    /// Maps to the SalesCostDetails table in the database
    /// </summary>
    [Table("SalesCostDetails")]
    public class SalesCostDetail
    {
        /// <summary>
        /// Primary key for the sales cost detail
        /// </summary>
        [Key]
        public int CostDetailID { get; set; }

        /// <summary>
        /// Foreign key reference to the sales invoice item
        /// </summary>
        [Required]
        public int SalesItemID { get; set; }

        /// <summary>
        /// Foreign key reference to the stock batch used for this sale
        /// </summary>
        [Required]
        public int BatchID { get; set; }

        /// <summary>
        /// Quantity taken from this specific batch for the sale
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityFromBatch { get; set; }

        /// <summary>
        /// Unit cost from this specific batch
        /// Used to calculate the cost of goods sold
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCostFromBatch { get; set; }

        /// <summary>
        /// Total cost from this batch (calculated as QuantityFromBatch * UnitCostFromBatch)
        /// This is a computed column in the database
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCostFromBatch { get; set; }

        /// <summary>
        /// Navigation property to the sales invoice item
        /// </summary>
        [ForeignKey("SalesItemID")]
        public virtual SalesInvoiceItem SalesItem { get; set; } = null!;

        /// <summary>
        /// Navigation property to the stock batch
        /// </summary>
        [ForeignKey("BatchID")]
        public virtual StockBatch Batch { get; set; } = null!;
    }
}

