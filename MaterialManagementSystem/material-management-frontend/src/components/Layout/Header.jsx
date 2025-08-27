import React, { useState } from 'react';
import { 
  Menu, 
  Bell, 
  Search, 
  User, 
  LogOut, 
  Settings,
  Moon,
  Sun
} from 'lucide-react';
import { useAuth } from '../../contexts/AuthContext';
import { cn } from '../../lib/utils';

/**
 * Header component with navigation, search, notifications, and user menu
 * Provides top-level navigation and user actions
 */
const Header = ({ onMenuClick }) => {
  const { user, logout } = useAuth();
  const [showUserMenu, setShowUserMenu] = useState(false);
  const [showNotifications, setShowNotifications] = useState(false);
  const [isDarkMode, setIsDarkMode] = useState(false);

  /**
   * Mock notifications data
   * In a real application, this would come from an API
   */
  const notifications = [
    {
      id: 1,
      title: 'مخزون منخفض',
      message: 'حديد التسليح 12 مم - الكمية أقل من الحد الأدنى',
      time: '5 دقائق',
      type: 'warning',
      unread: true
    },
    {
      id: 2,
      title: 'فاتورة جديدة',
      message: 'تم إنشاء فاتورة مبيعات جديدة #INV-2024-001',
      time: '15 دقيقة',
      type: 'info',
      unread: true
    },
    {
      id: 3,
      title: 'انتهاء صلاحية',
      message: 'أسمنت بورتلاند - ينتهي خلال 7 أيام',
      time: '1 ساعة',
      type: 'error',
      unread: false
    }
  ];

  const unreadCount = notifications.filter(n => n.unread).length;

  /**
   * Handles user logout
   */
  const handleLogout = () => {
    logout();
    setShowUserMenu(false);
  };

  /**
   * Toggles dark mode
   */
  const toggleDarkMode = () => {
    setIsDarkMode(!isDarkMode);
    // In a real application, you would persist this preference
    document.documentElement.classList.toggle('dark');
  };

  /**
   * Gets notification icon color based on type
   */
  const getNotificationColor = (type) => {
    switch (type) {
      case 'warning': return 'text-yellow-600';
      case 'error': return 'text-red-600';
      case 'info': return 'text-blue-600';
      default: return 'text-gray-600';
    }
  };

  return (
    <header className="bg-white border-b border-gray-200 px-4 py-3">
      <div className="flex items-center justify-between">
        {/* Left side - Menu and Search */}
        <div className="flex items-center space-x-4 space-x-reverse">
          {/* Mobile menu button */}
          <button
            onClick={onMenuClick}
            className="lg:hidden p-2 rounded-md hover:bg-gray-100"
          >
            <Menu className="w-5 h-5" />
          </button>

          {/* Search bar */}
          <div className="hidden md:flex items-center">
            <div className="relative">
              <Search className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
              <input
                type="text"
                placeholder="البحث في النظام..."
                className="w-64 pl-4 pr-10 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-right"
              />
            </div>
          </div>
        </div>

        {/* Right side - Notifications and User Menu */}
        <div className="flex items-center space-x-4 space-x-reverse">
          {/* Dark mode toggle */}
          <button
            onClick={toggleDarkMode}
            className="p-2 rounded-md hover:bg-gray-100 transition-colors"
            title={isDarkMode ? 'الوضع النهاري' : 'الوضع الليلي'}
          >
            {isDarkMode ? (
              <Sun className="w-5 h-5 text-gray-600" />
            ) : (
              <Moon className="w-5 h-5 text-gray-600" />
            )}
          </button>

          {/* Notifications */}
          <div className="relative">
            <button
              onClick={() => setShowNotifications(!showNotifications)}
              className="p-2 rounded-md hover:bg-gray-100 transition-colors relative"
            >
              <Bell className="w-5 h-5 text-gray-600" />
              {unreadCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                  {unreadCount}
                </span>
              )}
            </button>

            {/* Notifications dropdown */}
            {showNotifications && (
              <div className="absolute left-0 mt-2 w-80 bg-white rounded-lg shadow-lg border border-gray-200 z-50">
                <div className="p-4 border-b border-gray-200">
                  <h3 className="text-lg font-semibold text-gray-900">التنبيهات</h3>
                </div>
                <div className="max-h-96 overflow-y-auto">
                  {notifications.map((notification) => (
                    <div
                      key={notification.id}
                      className={cn(
                        "p-4 border-b border-gray-100 hover:bg-gray-50 cursor-pointer",
                        notification.unread && "bg-blue-50"
                      )}
                    >
                      <div className="flex items-start space-x-3 space-x-reverse">
                        <div className={cn("w-2 h-2 rounded-full mt-2", {
                          'bg-yellow-500': notification.type === 'warning',
                          'bg-red-500': notification.type === 'error',
                          'bg-blue-500': notification.type === 'info'
                        })} />
                        <div className="flex-1">
                          <p className="text-sm font-medium text-gray-900">
                            {notification.title}
                          </p>
                          <p className="text-sm text-gray-600 mt-1">
                            {notification.message}
                          </p>
                          <p className="text-xs text-gray-500 mt-2">
                            منذ {notification.time}
                          </p>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
                <div className="p-4 border-t border-gray-200">
                  <button className="text-sm text-blue-600 hover:text-blue-800">
                    عرض جميع التنبيهات
                  </button>
                </div>
              </div>
            )}
          </div>

          {/* User menu */}
          <div className="relative">
            <button
              onClick={() => setShowUserMenu(!showUserMenu)}
              className="flex items-center space-x-2 space-x-reverse p-2 rounded-md hover:bg-gray-100 transition-colors"
            >
              <div className="w-8 h-8 bg-blue-500 rounded-full flex items-center justify-center">
                <span className="text-white text-sm font-medium">
                  {user?.fullName?.charAt(0) || 'U'}
                </span>
              </div>
              <div className="hidden md:block text-right">
                <p className="text-sm font-medium text-gray-900">
                  {user?.fullName || 'مستخدم'}
                </p>
                <p className="text-xs text-gray-500">
                  {user?.role === 'Manager' ? 'مدير' : 
                   user?.role === 'Sales' ? 'مبيعات' : 
                   user?.role === 'Accountant' ? 'محاسب' : 'مستخدم'}
                </p>
              </div>
            </button>

            {/* User dropdown menu */}
            {showUserMenu && (
              <div className="absolute left-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-gray-200 z-50">
                <div className="py-2">
                  <button className="flex items-center space-x-3 space-x-reverse w-full px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                    <User className="w-4 h-4" />
                    <span>الملف الشخصي</span>
                  </button>
                  <button className="flex items-center space-x-3 space-x-reverse w-full px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                    <Settings className="w-4 h-4" />
                    <span>الإعدادات</span>
                  </button>
                  <hr className="my-2" />
                  <button
                    onClick={handleLogout}
                    className="flex items-center space-x-3 space-x-reverse w-full px-4 py-2 text-sm text-red-600 hover:bg-red-50"
                  >
                    <LogOut className="w-4 h-4" />
                    <span>تسجيل الخروج</span>
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Mobile search bar */}
      <div className="md:hidden mt-3">
        <div className="relative">
          <Search className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
          <input
            type="text"
            placeholder="البحث في النظام..."
            className="w-full pl-4 pr-10 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-right"
          />
        </div>
      </div>

      {/* Click outside handlers */}
      {(showUserMenu || showNotifications) && (
        <div
          className="fixed inset-0 z-40"
          onClick={() => {
            setShowUserMenu(false);
            setShowNotifications(false);
          }}
        />
      )}
    </header>
  );
};

export default Header;

