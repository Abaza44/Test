using MaterialManagementAPI.Data;
using MaterialManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MaterialManagementAPI.Services
{
    /// <summary>
    /// Implementation of FIFO (First In, First Out) inventory management service
    /// Handles all stock batch operations and cost calculations
    /// </summary>
    public class FIFOInventoryService : IFIFOInventoryService
    {
        private readonly MaterialManagementContext _context;
        private readonly ILogger<FIFOInventoryService> _logger;

        /// <summary>
        /// Constructor for FIFOInventoryService
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public FIFOInventoryService(MaterialManagementContext context, ILogger<FIFOInventoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds new stock to inventory when a purchase is made
        /// Creates a new stock batch with the purchase details
        /// </summary>
        public async Task<StockBatch> AddStockAsync(int materialId, decimal quantity, decimal unitCost, 
            int? purchaseId, DateTime purchaseDate, string? batchNumber = null, DateTime? expiryDate = null)
        {
            try
            {
                _logger.LogInformation("Adding stock for Material ID: {MaterialId}, Quantity: {Quantity}, Unit Cost: {UnitCost}", 
                    materialId, quantity, unitCost);

                // Validate that the material exists
                var material = await _context.Materials.FindAsync(materialId);
                if (material == null)
                {
                    throw new ArgumentException($"Material with ID {materialId} not found");
                }

                // Create new stock batch
                var stockBatch = new StockBatch
                {
                    MaterialID = materialId,
                    PurchaseID = purchaseId,
                    BatchNumber = batchNumber,
                    PurchaseDate = purchaseDate,
                    InitialQuantity = quantity,
                    RemainingQuantity = quantity,
                    UnitCost = unitCost,
                    ExpiryDate = expiryDate
                };

                // Add the stock batch to the database
                _context.StockBatches.Add(stockBatch);

                // Create stock movement record for the incoming stock
                var stockMovement = new StockMovement
                {
                    MaterialID = materialId,
                    MovementType = "IN",
                    Quantity = quantity,
                    UnitPrice = unitCost,
                    ReferenceType = "PURCHASE",
                    ReferenceID = purchaseId,
                    MovementDate = purchaseDate,
                    Notes = $"Stock added from purchase - Batch: {batchNumber}"
                };

                _context.StockMovements.Add(stockMovement);

                // Update the material's current stock
                await UpdateCurrentStockAsync(materialId);

                // Save all changes
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully added stock batch ID: {BatchId} for Material ID: {MaterialId}", 
                    stockBatch.BatchID, materialId);

                return stockBatch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock for Material ID: {MaterialId}", materialId);
                throw;
            }
        }

        /// <summary>
        /// Removes stock from inventory when a sale is made using FIFO methodology
        /// Updates stock batches and creates cost detail records
        /// </summary>
        public async Task<List<SalesCostDetail>> RemoveStockAsync(int materialId, decimal quantity, int salesItemId)
        {
            try
            {
                _logger.LogInformation("Removing stock for Material ID: {MaterialId}, Quantity: {Quantity}, Sales Item ID: {SalesItemId}", 
                    materialId, quantity, salesItemId);

                var costDetails = new List<SalesCostDetail>();
                decimal remainingQuantityToRemove = quantity;

                // Get available stock batches ordered by FIFO (oldest first)
                var availableBatches = await GetAvailableBatchesAsync(materialId);

                // Check if we have enough stock
                var totalAvailableStock = availableBatches.Sum(b => b.RemainingQuantity);
                if (totalAvailableStock < quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock. Available: {totalAvailableStock}, Required: {quantity}");
                }

                // Process each batch in FIFO order
                foreach (var batch in availableBatches)
                {
                    if (remainingQuantityToRemove <= 0)
                        break;

                    // Calculate how much to take from this batch
                    decimal quantityFromBatch = Math.Min(remainingQuantityToRemove, batch.RemainingQuantity);

                    // Create cost detail record
                    var costDetail = new SalesCostDetail
                    {
                        SalesItemID = salesItemId,
                        BatchID = batch.BatchID,
                        QuantityFromBatch = quantityFromBatch,
                        UnitCostFromBatch = batch.UnitCost
                        // TotalCostFromBatch is computed in the database
                    };

                    costDetails.Add(costDetail);
                    _context.SalesCostDetails.Add(costDetail);

                    // Update the batch's remaining quantity
                    batch.RemainingQuantity -= quantityFromBatch;
                    _context.StockBatches.Update(batch);

                    // Update remaining quantity to remove
                    remainingQuantityToRemove -= quantityFromBatch;

                    _logger.LogDebug("Used {Quantity} from Batch ID: {BatchId}, Remaining in batch: {RemainingQuantity}", 
                        quantityFromBatch, batch.BatchID, batch.RemainingQuantity);
                }

                // Create stock movement record for the outgoing stock
                var stockMovement = new StockMovement
                {
                    MaterialID = materialId,
                    MovementType = "OUT",
                    Quantity = quantity,
                    ReferenceType = "SALE",
                    ReferenceID = salesItemId,
                    MovementDate = DateTime.UtcNow,
                    Notes = $"Stock removed for sale - Sales Item ID: {salesItemId}"
                };

                _context.StockMovements.Add(stockMovement);

                // Update the material's current stock
                await UpdateCurrentStockAsync(materialId);

                // Save all changes
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully removed stock for Material ID: {MaterialId}, Created {CostDetailCount} cost details", 
                    materialId, costDetails.Count);

                return costDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing stock for Material ID: {MaterialId}", materialId);
                throw;
            }
        }

        /// <summary>
        /// Gets the current stock status for a material including FIFO cost information
        /// </summary>
        public async Task<StockStatusInfo> GetStockStatusAsync(int materialId)
        {
            try
            {
                var availableBatches = await GetAvailableBatchesAsync(materialId);

                if (!availableBatches.Any())
                {
                    return new StockStatusInfo
                    {
                        CurrentStock = 0,
                        TotalStockValue = 0,
                        WeightedAverageCost = 0,
                        LowestCost = 0,
                        HighestCost = 0,
                        BatchCount = 0
                    };
                }

                var totalQuantity = availableBatches.Sum(b => b.RemainingQuantity);
                var totalValue = availableBatches.Sum(b => b.RemainingQuantity * b.UnitCost);
                var weightedAverageCost = totalQuantity > 0 ? totalValue / totalQuantity : 0;

                return new StockStatusInfo
                {
                    CurrentStock = totalQuantity,
                    TotalStockValue = totalValue,
                    WeightedAverageCost = weightedAverageCost,
                    LowestCost = availableBatches.Min(b => b.UnitCost),
                    HighestCost = availableBatches.Max(b => b.UnitCost),
                    BatchCount = availableBatches.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock status for Material ID: {MaterialId}", materialId);
                throw;
            }
        }

        /// <summary>
        /// Gets all available stock batches for a material ordered by FIFO (oldest first)
        /// </summary>
        public async Task<List<StockBatch>> GetAvailableBatchesAsync(int materialId)
        {
            return await _context.StockBatches
                .Where(sb => sb.MaterialID == materialId && sb.RemainingQuantity > 0)
                .OrderBy(sb => sb.PurchaseDate)
                .ThenBy(sb => sb.BatchID) // Secondary sort for consistency
                .ToListAsync();
        }

        /// <summary>
        /// Updates the current stock quantity for a material based on all its batches
        /// </summary>
        public async Task<decimal> UpdateCurrentStockAsync(int materialId)
        {
            try
            {
                var material = await _context.Materials.FindAsync(materialId);
                if (material == null)
                {
                    throw new ArgumentException($"Material with ID {materialId} not found");
                }

                // Calculate current stock from all available batches
                var currentStock = await _context.StockBatches
                    .Where(sb => sb.MaterialID == materialId && sb.RemainingQuantity > 0)
                    .SumAsync(sb => sb.RemainingQuantity);

                // Update the material's current stock
                material.CurrentStock = currentStock;
                _context.Materials.Update(material);

                _logger.LogDebug("Updated current stock for Material ID: {MaterialId} to {CurrentStock}", 
                    materialId, currentStock);

                return currentStock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating current stock for Material ID: {MaterialId}", materialId);
                throw;
            }
        }

        /// <summary>
        /// Gets materials that are expiring within the specified number of days
        /// </summary>
        public async Task<List<StockBatch>> GetExpiringMaterialsAsync(int daysAhead = 30)
        {
            try
            {
                var cutoffDate = DateTime.Today.AddDays(daysAhead);

                return await _context.StockBatches
                    .Include(sb => sb.Material)
                    .Where(sb => sb.ExpiryDate.HasValue 
                        && sb.ExpiryDate.Value <= cutoffDate 
                        && sb.RemainingQuantity > 0)
                    .OrderBy(sb => sb.ExpiryDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring materials");
                throw;
            }
        }
    }
}

