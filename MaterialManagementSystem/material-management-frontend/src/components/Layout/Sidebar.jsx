import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { 
  Package, 
  Users, 
  Truck, 
  ShoppingCart, 
  ShoppingBag, 
  DollarSign, 
  UserCheck, 
  Settings, 
  BarChart3,
  AlertTriangle,
  Calendar,
  FileText,
  Home
} from 'lucide-react';
import { cn } from '../../lib/utils';
import { useAuth } from '../../contexts/AuthContext';

/**
 * Sidebar navigation component
 * Provides navigation links for different modules of the application
 * Shows/hides links based on user roles and permissions
 */
const Sidebar = ({ isOpen, onClose }) => {
  const location = useLocation();
  const { user, hasAnyRole } = useAuth();

  /**
   * Navigation items configuration
   * Each item includes icon, label, path, and required roles
   */
  const navigationItems = [
    {
      icon: Home,
      label: 'الرئيسية',
      path: '/',
      roles: ['Manager', 'Sales', 'Accountant']
    },
    {
      icon: Package,
      label: 'المواد والمخزون',
      path: '/materials',
      roles: ['Manager', 'Sales']
    },
    {
      icon: Users,
      label: 'العملاء',
      path: '/clients',
      roles: ['Manager', 'Sales']
    },
    {
      icon: Truck,
      label: 'الموردين',
      path: '/suppliers',
      roles: ['Manager', 'Sales']
    },
    {
      icon: ShoppingCart,
      label: 'المبيعات',
      path: '/sales',
      roles: ['Manager', 'Sales']
    },
    {
      icon: ShoppingBag,
      label: 'المشتريات',
      path: '/purchases',
      roles: ['Manager', 'Sales']
    },
    {
      icon: DollarSign,
      label: 'المصروفات',
      path: '/expenses',
      roles: ['Manager', 'Accountant']
    },
    {
      icon: UserCheck,
      label: 'الموظفين والرواتب',
      path: '/employees',
      roles: ['Manager', 'Accountant']
    },
    {
      icon: Settings,
      label: 'المعدات والصيانة',
      path: '/equipment',
      roles: ['Manager']
    },
    {
      icon: BarChart3,
      label: 'التقارير',
      path: '/reports',
      roles: ['Manager', 'Accountant']
    },
    {
      icon: AlertTriangle,
      label: 'التنبيهات',
      path: '/alerts',
      roles: ['Manager', 'Sales']
    },
    {
      icon: Calendar,
      label: 'المواعيد',
      path: '/schedule',
      roles: ['Manager', 'Sales']
    },
    {
      icon: FileText,
      label: 'الوثائق',
      path: '/documents',
      roles: ['Manager', 'Accountant']
    }
  ];

  /**
   * Filters navigation items based on user roles
   */
  const filteredItems = navigationItems.filter(item => 
    hasAnyRole(item.roles)
  );

  /**
   * Checks if a navigation item is currently active
   * @param {string} path - Navigation path
   * @returns {boolean} True if the path is active
   */
  const isActive = (path) => {
    if (path === '/') {
      return location.pathname === '/';
    }
    return location.pathname.startsWith(path);
  };

  return (
    <>
      {/* Mobile overlay */}
      {isOpen && (
        <div 
          className="fixed inset-0 bg-black bg-opacity-50 z-40 lg:hidden"
          onClick={onClose}
        />
      )}

      {/* Sidebar */}
      <div className={cn(
        "fixed top-0 right-0 h-full w-64 bg-white border-l border-gray-200 transform transition-transform duration-300 ease-in-out z-50",
        "lg:translate-x-0 lg:static lg:z-auto",
        isOpen ? "translate-x-0" : "translate-x-full"
      )}>
        {/* Header */}
        <div className="flex items-center justify-between p-4 border-b border-gray-200">
          <h2 className="text-lg font-semibold text-gray-800">
            نظام إدارة المواد
          </h2>
          <button
            onClick={onClose}
            className="lg:hidden p-2 rounded-md hover:bg-gray-100"
          >
            ×
          </button>
        </div>

        {/* User info */}
        <div className="p-4 border-b border-gray-200">
          <div className="flex items-center space-x-3 space-x-reverse">
            <div className="w-8 h-8 bg-blue-500 rounded-full flex items-center justify-center">
              <span className="text-white text-sm font-medium">
                {user?.fullName?.charAt(0) || 'U'}
              </span>
            </div>
            <div>
              <p className="text-sm font-medium text-gray-900">
                {user?.fullName || 'مستخدم'}
              </p>
              <p className="text-xs text-gray-500">
                {user?.role === 'Manager' ? 'مدير' : 
                 user?.role === 'Sales' ? 'مبيعات' : 
                 user?.role === 'Accountant' ? 'محاسب' : 'مستخدم'}
              </p>
            </div>
          </div>
        </div>

        {/* Navigation */}
        <nav className="flex-1 p-4 space-y-2 overflow-y-auto">
          {filteredItems.map((item) => {
            const Icon = item.icon;
            const active = isActive(item.path);
            
            return (
              <Link
                key={item.path}
                to={item.path}
                onClick={onClose}
                className={cn(
                  "flex items-center space-x-3 space-x-reverse px-3 py-2 rounded-lg text-sm font-medium transition-colors",
                  active
                    ? "bg-blue-50 text-blue-700 border-r-2 border-blue-700"
                    : "text-gray-600 hover:bg-gray-50 hover:text-gray-900"
                )}
              >
                <Icon className="w-5 h-5" />
                <span>{item.label}</span>
              </Link>
            );
          })}
        </nav>

        {/* Footer */}
        <div className="p-4 border-t border-gray-200">
          <div className="text-xs text-gray-500 text-center">
            نظام إدارة مواد البناء
            <br />
            الإصدار 1.0
          </div>
        </div>
      </div>
    </>
  );
};

export default Sidebar;

