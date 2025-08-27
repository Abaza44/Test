using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Represents a material category in the material management system
    /// Maps to the MaterialCategories table in the database
    /// </summary>
    [Table("MaterialCategories")]
    public class MaterialCategory
    {
        /// <summary>
        /// Primary key for the material category
        /// </summary>
        [Key]
        public int CategoryID { get; set; }

        /// <summary>
        /// Category name (required, max 100 characters)
        /// Examples: حديد وصلب، أسمنت ومونة، الطوب والبلوك
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the category (max 200 characters)
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property for materials in this category
        /// One category can have multiple materials
        /// </summary>
        public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}

