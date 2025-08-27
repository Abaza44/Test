import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import './App.css';

/**
 * Protected Route component
 * Redirects to login if user is not authenticated
 */
const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

/**
 * Main App component
 * Sets up routing and authentication context
 */
function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="App">
          <Routes>
            {/* Public Routes */}
            <Route path="/login" element={<Login />} />
            
            {/* Protected Routes */}
            <Route 
              path="/" 
              element={
                <ProtectedRoute>
                  <Dashboard />
                </ProtectedRoute>
              } 
            />
            
            {/* Placeholder routes for future pages */}
            <Route 
              path="/materials" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المواد</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/clients" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة العملاء</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/suppliers" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة الموردين</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/sales" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المبيعات</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/purchases" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المشتريات</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/expenses" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المصروفات</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/employees" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة الموظفين</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/equipment" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المعدات</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/reports" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة التقارير</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/alerts" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة التنبيهات</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/schedule" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة المواعيد</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            <Route 
              path="/documents" 
              element={
                <ProtectedRoute>
                  <div className="min-h-screen flex items-center justify-center bg-gray-50">
                    <div className="text-center">
                      <h1 className="text-2xl font-bold text-gray-900 mb-4">صفحة الوثائق</h1>
                      <p className="text-gray-600">قيد التطوير...</p>
                    </div>
                  </div>
                </ProtectedRoute>
              } 
            />
            
            {/* Catch all route - redirect to dashboard */}
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
