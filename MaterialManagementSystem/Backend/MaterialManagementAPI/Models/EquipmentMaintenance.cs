using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents equipment maintenance records in the material management system
    /// Maps to the EquipmentMaintenance table in the database
    /// </summary>
    [Table("EquipmentMaintenance")]
    public class EquipmentMaintenance
    {
        /// <summary>
        /// Primary key for the maintenance record
        /// </summary>
        [Key]
        public int MaintenanceID { get; set; }

        /// <summary>
        /// Foreign key reference to the equipment
        /// </summary>
        [Required]
        public int EquipmentID { get; set; }

        /// <summary>
        /// Date when the maintenance was performed
        /// </summary>
        [Required]
        public DateTime MaintenanceDate { get; set; }

        /// <summary>
        /// Cost of the maintenance (optional)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Description of the maintenance work performed (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Additional notes about the maintenance (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? Notes { get; set; }

        /// <summary>
        /// Navigation property to the equipment
        /// </summary>
        [ForeignKey("EquipmentID")]
        public virtual Equipment Equipment { get; set; } = null!;
    }
}

