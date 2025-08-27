using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaterialManagementAPI.Data;
using MaterialManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace MaterialManagementAPI.Controllers
{
    /// <summary>
    /// API Controller for managing clients in the material management system
    /// Provides CRUD operations for clients and their addresses
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class ClientsController : ControllerBase
    {
        private readonly MaterialManagementContext _context;
        private readonly ILogger<ClientsController> _logger;

        /// <summary>
        /// Constructor for ClientsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public ClientsController(MaterialManagementContext context, ILogger<ClientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all active clients with their addresses
        /// </summary>
        /// <returns>List of clients</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            try
            {
                _logger.LogInformation("Fetching all active clients");

                var clients = await _context.Clients
                    .Include(c => c.Addresses)
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching clients");
                return StatusCode(500, "An error occurred while fetching clients");
            }
        }

        /// <summary>
        /// Gets a specific client by ID with addresses and recent invoices
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Client details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            try
            {
                _logger.LogInformation("Fetching client with ID: {ClientId}", id);

                var client = await _context.Clients
                    .Include(c => c.Addresses)
                    .Include(c => c.SalesInvoices.Take(10)) // Include last 10 invoices
                    .ThenInclude(si => si.Status)
                    .FirstOrDefaultAsync(c => c.ClientID == id);

                if (client == null)
                {
                    _logger.LogWarning("Client with ID: {ClientId} not found", id);
                    return NotFound($"Client with ID {id} not found");
                }

                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching client with ID: {ClientId}", id);
                return StatusCode(500, "An error occurred while fetching the client");
            }
        }

        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="client">Client data</param>
        /// <returns>Created client</returns>
        [HttpPost]
        [Authorize(Roles = "Manager,Sales")] // Only managers and sales employees can create clients
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            try
            {
                _logger.LogInformation("Creating new client with phone: {Phone}", client.Phone);

                // Validate that phone number is unique
                var existingClient = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Phone == client.Phone);

                if (existingClient != null)
                {
                    return BadRequest($"Client with phone number '{client.Phone}' already exists");
                }

                // Set default values
                client.CreatedDate = DateTime.UtcNow;
                client.IsActive = true;

                // If addresses are provided, ensure only one is marked as default
                if (client.Addresses?.Any() == true)
                {
                    var defaultAddresses = client.Addresses.Where(a => a.IsDefault).ToList();
                    if (defaultAddresses.Count > 1)
                    {
                        // Keep only the first one as default
                        for (int i = 1; i < defaultAddresses.Count; i++)
                        {
                            defaultAddresses[i].IsDefault = false;
                        }
                    }
                    else if (defaultAddresses.Count == 0)
                    {
                        // Set the first address as default
                        client.Addresses.First().IsDefault = true;
                    }
                }

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Fetch the created client with its addresses
                var createdClient = await _context.Clients
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.ClientID == client.ClientID);

                _logger.LogInformation("Successfully created client with ID: {ClientId}", client.ClientID);

                return CreatedAtAction(nameof(GetClient), new { id = client.ClientID }, createdClient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating client");
                return StatusCode(500, "An error occurred while creating the client");
            }
        }

        /// <summary>
        /// Updates an existing client
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="client">Updated client data</param>
        /// <returns>Success status</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Sales")] // Only managers and sales employees can update clients
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            try
            {
                if (id != client.ClientID)
                {
                    return BadRequest("Client ID mismatch");
                }

                _logger.LogInformation("Updating client with ID: {ClientId}", id);

                var existingClient = await _context.Clients
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.ClientID == id);

                if (existingClient == null)
                {
                    return NotFound($"Client with ID {id} not found");
                }

                // Check if phone number is unique (excluding current client)
                var duplicatePhone = await _context.Clients
                    .AnyAsync(c => c.Phone == client.Phone && c.ClientID != id);

                if (duplicatePhone)
                {
                    return BadRequest($"Client with phone number '{client.Phone}' already exists");
                }

                // Update basic properties
                existingClient.Name = client.Name;
                existingClient.Phone = client.Phone;
                existingClient.IsActive = client.IsActive;

                // Update addresses if provided
                if (client.Addresses?.Any() == true)
                {
                    // Remove existing addresses
                    _context.ClientAddresses.RemoveRange(existingClient.Addresses);

                    // Add new addresses
                    foreach (var address in client.Addresses)
                    {
                        address.ClientID = id;
                        existingClient.Addresses.Add(address);
                    }

                    // Ensure only one address is marked as default
                    var defaultAddresses = existingClient.Addresses.Where(a => a.IsDefault).ToList();
                    if (defaultAddresses.Count > 1)
                    {
                        for (int i = 1; i < defaultAddresses.Count; i++)
                        {
                            defaultAddresses[i].IsDefault = false;
                        }
                    }
                    else if (defaultAddresses.Count == 0 && existingClient.Addresses.Any())
                    {
                        existingClient.Addresses.First().IsDefault = true;
                    }
                }

                _context.Entry(existingClient).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated client with ID: {ClientId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client with ID: {ClientId}", id);
                return StatusCode(500, "An error occurred while updating the client");
            }
        }

        /// <summary>
        /// Soft deletes a client (sets IsActive to false)
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")] // Only managers can delete clients
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                _logger.LogInformation("Deleting client with ID: {ClientId}", id);

                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound($"Client with ID {id} not found");
                }

                // Check if client has invoices or collections
                var hasTransactions = await _context.SalesInvoices
                    .AnyAsync(si => si.ClientID == id) ||
                    await _context.Collections
                    .AnyAsync(c => c.ClientID == id);

                if (hasTransactions)
                {
                    // Soft delete - set IsActive to false
                    client.IsActive = false;
                    _context.Entry(client).State = EntityState.Modified;
                    _logger.LogInformation("Soft deleted client with ID: {ClientId} (has transactions)", id);
                }
                else
                {
                    // Hard delete if no transactions
                    _context.Clients.Remove(client);
                    _logger.LogInformation("Hard deleted client with ID: {ClientId}", id);
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting client with ID: {ClientId}", id);
                return StatusCode(500, "An error occurred while deleting the client");
            }
        }

        /// <summary>
        /// Searches clients by name or phone number
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching clients</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Client>>> SearchClients([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term is required");
                }

                _logger.LogInformation("Searching clients with term: {SearchTerm}", searchTerm);

                var clients = await _context.Clients
                    .Include(c => c.Addresses)
                    .Where(c => c.IsActive && 
                        (c.Name.Contains(searchTerm) || c.Phone.Contains(searchTerm)))
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching clients with term: {SearchTerm}", searchTerm);
                return StatusCode(500, "An error occurred while searching clients");
            }
        }

        /// <summary>
        /// Gets the balance information for a specific client
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Client balance information</returns>
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<object>> GetClientBalance(int id)
        {
            try
            {
                _logger.LogInformation("Fetching balance for client ID: {ClientId}", id);

                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound($"Client with ID {id} not found");
                }

                // Calculate total sales (excluding cancelled invoices)
                var totalSales = await _context.SalesInvoices
                    .Where(si => si.ClientID == id && si.StatusID != 5) // 5 = Cancelled
                    .SumAsync(si => si.TotalAmount);

                // Calculate total collections
                var totalCollections = await _context.Collections
                    .Where(c => c.ClientID == id)
                    .SumAsync(c => c.Amount);

                // Calculate balance
                var balance = totalSales - totalCollections;

                var balanceInfo = new
                {
                    ClientID = id,
                    ClientName = client.Name,
                    TotalSales = totalSales,
                    TotalCollections = totalCollections,
                    Balance = balance,
                    BalanceStatus = balance > 0 ? "مدين" : balance < 0 ? "دائن" : "متوازن"
                };

                return Ok(balanceInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching balance for client ID: {ClientId}", id);
                return StatusCode(500, "An error occurred while fetching client balance");
            }
        }

        /// <summary>
        /// Gets all client balances
        /// </summary>
        /// <returns>List of client balances</returns>
        [HttpGet("balances")]
        [Authorize(Roles = "Manager,Accountant")] // Only managers and accountants can view all balances
        public async Task<ActionResult<IEnumerable<object>>> GetAllClientBalances()
        {
            try
            {
                _logger.LogInformation("Fetching all client balances");

                var clientBalances = await _context.Clients
                    .Where(c => c.IsActive)
                    .Select(c => new
                    {
                        ClientID = c.ClientID,
                        ClientName = c.Name,
                        Phone = c.Phone,
                        TotalSales = c.SalesInvoices
                            .Where(si => si.StatusID != 5) // Exclude cancelled invoices
                            .Sum(si => si.TotalAmount),
                        TotalCollections = c.Collections.Sum(col => col.Amount),
                        Balance = c.SalesInvoices
                            .Where(si => si.StatusID != 5)
                            .Sum(si => si.TotalAmount) - c.Collections.Sum(col => col.Amount)
                    })
                    .OrderByDescending(cb => cb.Balance)
                    .ToListAsync();

                // Add balance status
                var result = clientBalances.Select(cb => new
                {
                    cb.ClientID,
                    cb.ClientName,
                    cb.Phone,
                    cb.TotalSales,
                    cb.TotalCollections,
                    cb.Balance,
                    BalanceStatus = cb.Balance > 0 ? "مدين" : cb.Balance < 0 ? "دائن" : "متوازن"
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all client balances");
                return StatusCode(500, "An error occurred while fetching client balances");
            }
        }
    }
}

