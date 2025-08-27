using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents equipment in the material management system
    /// Maps to the Equipments table in the database
    /// </summary>
    [Table("Equipments")]
    public class Equipment
    {
        /// <summary>
        /// Primary key for the equipment
        /// </summary>
        [Key]
        public int EquipmentID { get; set; }

        /// <summary>
        /// Equipment name (required, max 100 characters)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Equipment serial number (optional, max 50 characters)
        /// </summary>
        [StringLength(50)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Date when the equipment was purchased (optional)
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Original purchase price of the equipment (optional)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Current value of the equipment (optional)
        /// May be different from purchase price due to depreciation
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CurrentValue { get; set; }

        /// <summary>
        /// Current status of the equipment (max 20 characters, default "Active")
        /// Values: Active, InMaintenance, Retired
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Current location of the equipment (optional, max 100 characters)
        /// </summary>
        [StringLength(100)]
        public string? Location { get; set; }

        /// <summary>
        /// Description of the equipment (optional, max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Path to the equipment image file (optional, max 300 characters)
        /// </summary>
        [StringLength(300)]
        public string? ImagePath { get; set; }

        /// <summary>
        /// Navigation property for maintenance records
        /// One equipment can have multiple maintenance records
        /// </summary>
        public virtual ICollection<EquipmentMaintenance> MaintenanceRecords { get; set; } = new List<EquipmentMaintenance>();
    }
}

