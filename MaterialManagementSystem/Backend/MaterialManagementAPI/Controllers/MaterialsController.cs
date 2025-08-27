using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaterialManagementAPI.Data;
using MaterialManagementAPI.Models;
using MaterialManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace MaterialManagementAPI.Controllers
{
    /// <summary>
    /// API Controller for managing materials in the material management system
    /// Provides CRUD operations and inventory-related functionality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class MaterialsController : ControllerBase
    {
        private readonly MaterialManagementContext _context;
        private readonly IFIFOInventoryService _inventoryService;
        private readonly ILogger<MaterialsController> _logger;

        /// <summary>
        /// Constructor for MaterialsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="inventoryService">FIFO inventory service</param>
        /// <param name="logger">Logger instance</param>
        public MaterialsController(MaterialManagementContext context, IFIFOInventoryService inventoryService, ILogger<MaterialsController> logger)
        {
            _context = context;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all active materials with their categories
        /// </summary>
        /// <returns>List of materials</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        {
            try
            {
                _logger.LogInformation("Fetching all active materials");

                var materials = await _context.Materials
                    .Include(m => m.Category)
                    .Where(m => m.IsActive)
                    .OrderBy(m => m.MaterialCode)
                    .ToListAsync();

                return Ok(materials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching materials");
                return StatusCode(500, "An error occurred while fetching materials");
            }
        }

        /// <summary>
        /// Gets a specific material by ID with its category and stock information
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Material details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            try
            {
                _logger.LogInformation("Fetching material with ID: {MaterialId}", id);

                var material = await _context.Materials
                    .Include(m => m.Category)
                    .FirstOrDefaultAsync(m => m.MaterialID == id);

                if (material == null)
                {
                    _logger.LogWarning("Material with ID: {MaterialId} not found", id);
                    return NotFound($"Material with ID {id} not found");
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching material with ID: {MaterialId}", id);
                return StatusCode(500, "An error occurred while fetching the material");
            }
        }

        /// <summary>
        /// Creates a new material
        /// </summary>
        /// <param name="material">Material data</param>
        /// <returns>Created material</returns>
        [HttpPost]
        [Authorize(Roles = "Manager,Sales")] // Only managers and sales employees can create materials
        public async Task<ActionResult<Material>> CreateMaterial(Material material)
        {
            try
            {
                _logger.LogInformation("Creating new material with code: {MaterialCode}", material.MaterialCode);

                // Validate that material code is unique
                var existingMaterial = await _context.Materials
                    .FirstOrDefaultAsync(m => m.MaterialCode == material.MaterialCode);

                if (existingMaterial != null)
                {
                    return BadRequest($"Material with code '{material.MaterialCode}' already exists");
                }

                // Validate category exists if provided
                if (material.CategoryID.HasValue)
                {
                    var categoryExists = await _context.MaterialCategories
                        .AnyAsync(mc => mc.CategoryID == material.CategoryID.Value);

                    if (!categoryExists)
                    {
                        return BadRequest($"Category with ID {material.CategoryID} does not exist");
                    }
                }

                // Set default values
                material.CurrentStock = 0; // Will be updated when stock is added
                material.IsActive = true;

                _context.Materials.Add(material);
                await _context.SaveChangesAsync();

                // Fetch the created material with its category
                var createdMaterial = await _context.Materials
                    .Include(m => m.Category)
                    .FirstOrDefaultAsync(m => m.MaterialID == material.MaterialID);

                _logger.LogInformation("Successfully created material with ID: {MaterialId}", material.MaterialID);

                return CreatedAtAction(nameof(GetMaterial), new { id = material.MaterialID }, createdMaterial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating material");
                return StatusCode(500, "An error occurred while creating the material");
            }
        }

        /// <summary>
        /// Updates an existing material
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <param name="material">Updated material data</param>
        /// <returns>Updated material</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Sales")] // Only managers and sales employees can update materials
        public async Task<IActionResult> UpdateMaterial(int id, Material material)
        {
            try
            {
                if (id != material.MaterialID)
                {
                    return BadRequest("Material ID mismatch");
                }

                _logger.LogInformation("Updating material with ID: {MaterialId}", id);

                var existingMaterial = await _context.Materials.FindAsync(id);
                if (existingMaterial == null)
                {
                    return NotFound($"Material with ID {id} not found");
                }

                // Check if material code is unique (excluding current material)
                var duplicateCode = await _context.Materials
                    .AnyAsync(m => m.MaterialCode == material.MaterialCode && m.MaterialID != id);

                if (duplicateCode)
                {
                    return BadRequest($"Material with code '{material.MaterialCode}' already exists");
                }

                // Validate category exists if provided
                if (material.CategoryID.HasValue)
                {
                    var categoryExists = await _context.MaterialCategories
                        .AnyAsync(mc => mc.CategoryID == material.CategoryID.Value);

                    if (!categoryExists)
                    {
                        return BadRequest($"Category with ID {material.CategoryID} does not exist");
                    }
                }

                // Update properties (preserve CurrentStock as it's managed by inventory service)
                existingMaterial.MaterialCode = material.MaterialCode;
                existingMaterial.Name = material.Name;
                existingMaterial.CategoryID = material.CategoryID;
                existingMaterial.Unit = material.Unit;
                existingMaterial.MinimumStock = material.MinimumStock;
                existingMaterial.SellingPrice = material.SellingPrice;
                existingMaterial.Location = material.Location;
                existingMaterial.IsActive = material.IsActive;

                _context.Entry(existingMaterial).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated material with ID: {MaterialId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating material with ID: {MaterialId}", id);
                return StatusCode(500, "An error occurred while updating the material");
            }
        }

        /// <summary>
        /// Soft deletes a material (sets IsActive to false)
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")] // Only managers can delete materials
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            try
            {
                _logger.LogInformation("Deleting material with ID: {MaterialId}", id);

                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound($"Material with ID {id} not found");
                }

                // Check if material has stock or is used in transactions
                var hasStock = await _context.StockBatches
                    .AnyAsync(sb => sb.MaterialID == id && sb.RemainingQuantity > 0);

                var hasTransactions = await _context.SalesInvoiceItems
                    .AnyAsync(sii => sii.MaterialID == id) ||
                    await _context.PurchaseInvoiceItems
                    .AnyAsync(pii => pii.MaterialID == id);

                if (hasStock || hasTransactions)
                {
                    // Soft delete - set IsActive to false
                    material.IsActive = false;
                    _context.Entry(material).State = EntityState.Modified;
                    _logger.LogInformation("Soft deleted material with ID: {MaterialId} (has stock or transactions)", id);
                }
                else
                {
                    // Hard delete if no stock or transactions
                    _context.Materials.Remove(material);
                    _logger.LogInformation("Hard deleted material with ID: {MaterialId}", id);
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting material with ID: {MaterialId}", id);
                return StatusCode(500, "An error occurred while deleting the material");
            }
        }

        /// <summary>
        /// Gets the stock status for a specific material using FIFO calculations
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Stock status information</returns>
        [HttpGet("{id}/stock-status")]
        public async Task<ActionResult<StockStatusInfo>> GetMaterialStockStatus(int id)
        {
            try
            {
                _logger.LogInformation("Fetching stock status for material ID: {MaterialId}", id);

                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound($"Material with ID {id} not found");
                }

                var stockStatus = await _inventoryService.GetStockStatusAsync(id);
                return Ok(stockStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock status for material ID: {MaterialId}", id);
                return StatusCode(500, "An error occurred while fetching stock status");
            }
        }

        /// <summary>
        /// Gets all available stock batches for a material ordered by FIFO
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>List of available stock batches</returns>
        [HttpGet("{id}/batches")]
        public async Task<ActionResult<IEnumerable<StockBatch>>> GetMaterialBatches(int id)
        {
            try
            {
                _logger.LogInformation("Fetching stock batches for material ID: {MaterialId}", id);

                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound($"Material with ID {id} not found");
                }

                var batches = await _inventoryService.GetAvailableBatchesAsync(id);
                return Ok(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock batches for material ID: {MaterialId}", id);
                return StatusCode(500, "An error occurred while fetching stock batches");
            }
        }

        /// <summary>
        /// Gets materials with low stock (current stock <= minimum stock)
        /// </summary>
        /// <returns>List of materials with low stock</returns>
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Material>>> GetLowStockMaterials()
        {
            try
            {
                _logger.LogInformation("Fetching materials with low stock");

                var lowStockMaterials = await _context.Materials
                    .Include(m => m.Category)
                    .Where(m => m.IsActive && m.CurrentStock <= m.MinimumStock)
                    .OrderBy(m => m.CurrentStock)
                    .ToListAsync();

                return Ok(lowStockMaterials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching low stock materials");
                return StatusCode(500, "An error occurred while fetching low stock materials");
            }
        }

        /// <summary>
        /// Gets materials that are expiring within the specified number of days
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead (default: 30)</param>
        /// <returns>List of expiring stock batches</returns>
        [HttpGet("expiring")]
        public async Task<ActionResult<IEnumerable<StockBatch>>> GetExpiringMaterials([FromQuery] int daysAhead = 30)
        {
            try
            {
                _logger.LogInformation("Fetching materials expiring within {DaysAhead} days", daysAhead);

                var expiringBatches = await _inventoryService.GetExpiringMaterialsAsync(daysAhead);
                return Ok(expiringBatches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching expiring materials");
                return StatusCode(500, "An error occurred while fetching expiring materials");
            }
        }
    }
}

