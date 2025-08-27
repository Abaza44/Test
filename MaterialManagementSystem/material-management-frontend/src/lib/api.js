/**
 * API client for Material Management System
 * Handles all HTTP requests to the backend API with authentication
 */

const API_BASE_URL = 'http://localhost:5000/api'; // Update this to match your backend URL

/**
 * API client class that handles all HTTP requests
 */
class ApiClient {
  constructor() {
    this.baseURL = API_BASE_URL;
    this.token = localStorage.getItem('authToken');
  }

  /**
   * Sets the authentication token for API requests
   * @param {string} token - JWT token
   */
  setAuthToken(token) {
    this.token = token;
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  }

  /**
   * Gets the current authentication token
   * @returns {string|null} Current token or null
   */
  getAuthToken() {
    return this.token || localStorage.getItem('authToken');
  }

  /**
   * Creates headers for API requests including authentication
   * @param {Object} additionalHeaders - Additional headers to include
   * @returns {Object} Headers object
   */
  createHeaders(additionalHeaders = {}) {
    const headers = {
      'Content-Type': 'application/json',
      ...additionalHeaders
    };

    const token = this.getAuthToken();
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    return headers;
  }

  /**
   * Makes an HTTP request to the API
   * @param {string} endpoint - API endpoint (without base URL)
   * @param {Object} options - Fetch options
   * @returns {Promise<Object>} Response data
   */
  async request(endpoint, options = {}) {
    const url = `${this.baseURL}${endpoint}`;
    const config = {
      headers: this.createHeaders(options.headers),
      ...options
    };

    try {
      const response = await fetch(url, config);

      // Handle authentication errors
      if (response.status === 401) {
        this.setAuthToken(null);
        window.location.href = '/login';
        throw new Error('Authentication required');
      }

      // Handle other HTTP errors
      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData || `HTTP error! status: ${response.status}`);
      }

      // Handle empty responses
      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return await response.json();
      } else {
        return await response.text();
      }
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  /**
   * Makes a GET request
   * @param {string} endpoint - API endpoint
   * @returns {Promise<Object>} Response data
   */
  async get(endpoint) {
    return this.request(endpoint, { method: 'GET' });
  }

  /**
   * Makes a POST request
   * @param {string} endpoint - API endpoint
   * @param {Object} data - Request body data
   * @returns {Promise<Object>} Response data
   */
  async post(endpoint, data) {
    return this.request(endpoint, {
      method: 'POST',
      body: JSON.stringify(data)
    });
  }

  /**
   * Makes a PUT request
   * @param {string} endpoint - API endpoint
   * @param {Object} data - Request body data
   * @returns {Promise<Object>} Response data
   */
  async put(endpoint, data) {
    return this.request(endpoint, {
      method: 'PUT',
      body: JSON.stringify(data)
    });
  }

  /**
   * Makes a DELETE request
   * @param {string} endpoint - API endpoint
   * @returns {Promise<Object>} Response data
   */
  async delete(endpoint) {
    return this.request(endpoint, { method: 'DELETE' });
  }

  // ========== Authentication API Methods ==========

  /**
   * Authenticates a user with email and password
   * @param {string} email - User email
   * @param {string} password - User password
   * @returns {Promise<Object>} Authentication response with token
   */
  async login(email, password) {
    const response = await this.post('/auth/login', { email, password });
    if (response.token) {
      this.setAuthToken(response.token);
    }
    return response;
  }

  /**
   * Logs out the current user
   */
  logout() {
    this.setAuthToken(null);
  }

  // ========== Materials API Methods ==========

  /**
   * Gets all materials
   * @returns {Promise<Array>} List of materials
   */
  async getMaterials() {
    return this.get('/materials');
  }

  /**
   * Gets a specific material by ID
   * @param {number} id - Material ID
   * @returns {Promise<Object>} Material data
   */
  async getMaterial(id) {
    return this.get(`/materials/${id}`);
  }

  /**
   * Creates a new material
   * @param {Object} materialData - Material data
   * @returns {Promise<Object>} Created material
   */
  async createMaterial(materialData) {
    return this.post('/materials', materialData);
  }

  /**
   * Updates an existing material
   * @param {number} id - Material ID
   * @param {Object} materialData - Updated material data
   * @returns {Promise<Object>} Update response
   */
  async updateMaterial(id, materialData) {
    return this.put(`/materials/${id}`, materialData);
  }

  /**
   * Deletes a material
   * @param {number} id - Material ID
   * @returns {Promise<Object>} Delete response
   */
  async deleteMaterial(id) {
    return this.delete(`/materials/${id}`);
  }

  /**
   * Gets stock status for a material
   * @param {number} id - Material ID
   * @returns {Promise<Object>} Stock status information
   */
  async getMaterialStockStatus(id) {
    return this.get(`/materials/${id}/stock-status`);
  }

  /**
   * Gets stock batches for a material
   * @param {number} id - Material ID
   * @returns {Promise<Array>} List of stock batches
   */
  async getMaterialBatches(id) {
    return this.get(`/materials/${id}/batches`);
  }

  /**
   * Gets materials with low stock
   * @returns {Promise<Array>} List of low stock materials
   */
  async getLowStockMaterials() {
    return this.get('/materials/low-stock');
  }

  /**
   * Gets expiring materials
   * @param {number} daysAhead - Days to look ahead (default: 30)
   * @returns {Promise<Array>} List of expiring materials
   */
  async getExpiringMaterials(daysAhead = 30) {
    return this.get(`/materials/expiring?daysAhead=${daysAhead}`);
  }

  // ========== Clients API Methods ==========

  /**
   * Gets all clients
   * @returns {Promise<Array>} List of clients
   */
  async getClients() {
    return this.get('/clients');
  }

  /**
   * Gets a specific client by ID
   * @param {number} id - Client ID
   * @returns {Promise<Object>} Client data
   */
  async getClient(id) {
    return this.get(`/clients/${id}`);
  }

  /**
   * Creates a new client
   * @param {Object} clientData - Client data
   * @returns {Promise<Object>} Created client
   */
  async createClient(clientData) {
    return this.post('/clients', clientData);
  }

  /**
   * Updates an existing client
   * @param {number} id - Client ID
   * @param {Object} clientData - Updated client data
   * @returns {Promise<Object>} Update response
   */
  async updateClient(id, clientData) {
    return this.put(`/clients/${id}`, clientData);
  }

  /**
   * Deletes a client
   * @param {number} id - Client ID
   * @returns {Promise<Object>} Delete response
   */
  async deleteClient(id) {
    return this.delete(`/clients/${id}`);
  }

  /**
   * Searches clients by name or phone
   * @param {string} searchTerm - Search term
   * @returns {Promise<Array>} List of matching clients
   */
  async searchClients(searchTerm) {
    return this.get(`/clients/search?searchTerm=${encodeURIComponent(searchTerm)}`);
  }

  /**
   * Gets client balance information
   * @param {number} id - Client ID
   * @returns {Promise<Object>} Client balance data
   */
  async getClientBalance(id) {
    return this.get(`/clients/${id}/balance`);
  }

  /**
   * Gets all client balances
   * @returns {Promise<Array>} List of client balances
   */
  async getAllClientBalances() {
    return this.get('/clients/balances');
  }

  // ========== Material Categories API Methods ==========

  /**
   * Gets all material categories
   * @returns {Promise<Array>} List of material categories
   */
  async getMaterialCategories() {
    return this.get('/materialcategories');
  }
}

// Create and export a singleton instance
const apiClient = new ApiClient();
export default apiClient;

