import React, { useState } from 'react';
import Header from './Header';
import Sidebar from './Sidebar';

/**
 * Main layout component that provides the overall structure for the application
 * Combines header, sidebar, and main content area
 * Handles responsive behavior for mobile and desktop
 */
const Layout = ({ children }) => {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  /**
   * Toggles the sidebar open/closed state
   */
  const toggleSidebar = () => {
    setSidebarOpen(!sidebarOpen);
  };

  /**
   * Closes the sidebar
   */
  const closeSidebar = () => {
    setSidebarOpen(false);
  };

  return (
    <div className="min-h-screen bg-gray-50" dir="rtl">
      {/* Header */}
      <Header onMenuClick={toggleSidebar} />
      
      <div className="flex">
        {/* Sidebar */}
        <Sidebar isOpen={sidebarOpen} onClose={closeSidebar} />
        
        {/* Main content area */}
        <main className="flex-1 lg:mr-64 transition-all duration-300">
          <div className="p-6">
            {children}
          </div>
        </main>
      </div>
    </div>
  );
};

export default Layout;

