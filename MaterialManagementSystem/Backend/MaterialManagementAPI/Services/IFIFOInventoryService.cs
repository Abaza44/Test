using MaterialManagementAPI.Models;

namespace MaterialManagementAPI.Services
{
    /// <summary>
    /// Interface for FIFO (First In, First Out) inventory management service
    /// Handles stock batch operations and cost calculations
    /// </summary>
    public interface IFIFOInventoryService
    {
        /// <summary>
        /// Adds new stock to inventory when a purchase is made
        /// Creates a new stock batch with the purchase details
        /// </summary>
        /// <param name="materialId">ID of the material being purchased</param>
        /// <param name="quantity">Quantity purchased</param>
        /// <param name="unitCost">Unit cost of the purchase</param>
        /// <param name="purchaseId">ID of the purchase invoice</param>
        /// <param name="purchaseDate">Date of the purchase</param>
        /// <param name="batchNumber">Optional batch number for tracking</param>
        /// <param name="expiryDate">Optional expiry date for the batch</param>
        /// <returns>The created stock batch</returns>
        Task<StockBatch> AddStockAsync(int materialId, decimal quantity, decimal unitCost, 
            int? purchaseId, DateTime purchaseDate, string? batchNumber = null, DateTime? expiryDate = null);

        /// <summary>
        /// Removes stock from inventory when a sale is made using FIFO methodology
        /// Updates stock batches and creates cost detail records
        /// </summary>
        /// <param name="materialId">ID of the material being sold</param>
        /// <param name="quantity">Quantity sold</param>
        /// <param name="salesItemId">ID of the sales invoice item</param>
        /// <returns>List of cost details showing which batches were used and their costs</returns>
        Task<List<SalesCostDetail>> RemoveStockAsync(int materialId, decimal quantity, int salesItemId);

        /// <summary>
        /// Gets the current stock status for a material including FIFO cost information
        /// </summary>
        /// <param name="materialId">ID of the material</param>
        /// <returns>Stock status information</returns>
        Task<StockStatusInfo> GetStockStatusAsync(int materialId);

        /// <summary>
        /// Gets all available stock batches for a material ordered by FIFO (oldest first)
        /// </summary>
        /// <param name="materialId">ID of the material</param>
        /// <returns>List of available stock batches</returns>
        Task<List<StockBatch>> GetAvailableBatchesAsync(int materialId);

        /// <summary>
        /// Updates the current stock quantity for a material based on all its batches
        /// </summary>
        /// <param name="materialId">ID of the material</param>
        /// <returns>Updated current stock quantity</returns>
        Task<decimal> UpdateCurrentStockAsync(int materialId);

        /// <summary>
        /// Gets materials that are expiring within the specified number of days
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead for expiring materials</param>
        /// <returns>List of expiring stock batches</returns>
        Task<List<StockBatch>> GetExpiringMaterialsAsync(int daysAhead = 30);
    }

    /// <summary>
    /// Data transfer object for stock status information
    /// </summary>
    public class StockStatusInfo
    {
        /// <summary>
        /// Current total stock quantity
        /// </summary>
        public decimal CurrentStock { get; set; }

        /// <summary>
        /// Total value of current stock
        /// </summary>
        public decimal TotalStockValue { get; set; }

        /// <summary>
        /// Weighted average cost of current stock
        /// </summary>
        public decimal WeightedAverageCost { get; set; }

        /// <summary>
        /// Lowest cost among current batches
        /// </summary>
        public decimal LowestCost { get; set; }

        /// <summary>
        /// Highest cost among current batches
        /// </summary>
        public decimal HighestCost { get; set; }

        /// <summary>
        /// Number of different batches in stock
        /// </summary>
        public int BatchCount { get; set; }
    }
}

