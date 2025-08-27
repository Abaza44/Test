import React, { createContext, useContext, useState, useEffect } from 'react';
import apiClient from '../lib/api';

/**
 * Authentication Context for managing user authentication state
 * Provides login, logout, and user information throughout the application
 */
const AuthContext = createContext();

/**
 * Custom hook to use the authentication context
 * @returns {Object} Authentication context value
 */
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

/**
 * Authentication Provider component
 * Wraps the application to provide authentication state and methods
 */
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  /**
   * Checks if user is authenticated on component mount
   * Validates the stored token and gets user information
   */
  useEffect(() => {
    const checkAuthStatus = async () => {
      try {
        const token = apiClient.getAuthToken();
        if (token) {
          // TODO: Add API endpoint to validate token and get user info
          // For now, we'll assume the token is valid if it exists
          setIsAuthenticated(true);
          // You can decode the JWT token here to get user info
          // or make an API call to get current user details
        }
      } catch (error) {
        console.error('Auth check failed:', error);
        apiClient.logout();
      } finally {
        setLoading(false);
      }
    };

    checkAuthStatus();
  }, []);

  /**
   * Logs in a user with email and password
   * @param {string} email - User email
   * @param {string} password - User password
   * @returns {Promise<Object>} Login result
   */
  const login = async (email, password) => {
    try {
      setLoading(true);
      const response = await apiClient.login(email, password);
      
      if (response.token) {
        setUser(response.user || { email });
        setIsAuthenticated(true);
        return { success: true, user: response.user };
      } else {
        throw new Error('No token received');
      }
    } catch (error) {
      console.error('Login failed:', error);
      return { 
        success: false, 
        error: error.message || 'Login failed. Please check your credentials.' 
      };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Logs out the current user
   * Clears authentication state and redirects to login
   */
  const logout = () => {
    apiClient.logout();
    setUser(null);
    setIsAuthenticated(false);
    // Redirect to login page
    window.location.href = '/login';
  };

  /**
   * Mock login function for development/testing
   * Simulates authentication without backend
   * @param {string} email - User email
   * @param {string} password - User password
   * @returns {Promise<Object>} Mock login result
   */
  const mockLogin = async (email, password) => {
    try {
      setLoading(true);
      
      // Simulate API delay
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Mock authentication logic
      if (email === 'admin@materialmgmt.com' && password === 'Admin123!') {
        const mockUser = {
          id: 1,
          email: 'admin@materialmgmt.com',
          fullName: 'System Administrator',
          role: 'Manager'
        };
        
        // Set a mock token
        const mockToken = 'mock-jwt-token-' + Date.now();
        apiClient.setAuthToken(mockToken);
        
        setUser(mockUser);
        setIsAuthenticated(true);
        
        return { success: true, user: mockUser };
      } else {
        throw new Error('Invalid credentials');
      }
    } catch (error) {
      return { 
        success: false, 
        error: error.message || 'Login failed. Please check your credentials.' 
      };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Checks if the current user has a specific role
   * @param {string} role - Role to check
   * @returns {boolean} True if user has the role
   */
  const hasRole = (role) => {
    return user?.role === role || user?.roles?.includes(role);
  };

  /**
   * Checks if the current user has any of the specified roles
   * @param {Array<string>} roles - Array of roles to check
   * @returns {boolean} True if user has any of the roles
   */
  const hasAnyRole = (roles) => {
    if (!user) return false;
    return roles.some(role => hasRole(role));
  };

  const value = {
    user,
    isAuthenticated,
    loading,
    login: mockLogin, // Use mockLogin for development, switch to login for production
    logout,
    hasRole,
    hasAnyRole
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

