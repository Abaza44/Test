using Microsoft.AspNetCore.Identity;

namespace MaterialManagementAPI.Models
{
    /// <summary>
    /// Custom user model that extends IdentityUser to include additional properties
    /// for the Material Management System users
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Full name of the user (Arabic name support)
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Date when the user account was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the user account is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Last login date for tracking user activity
        /// </summary>
        public DateTime? LastLoginDate { get; set; }
    }
}

