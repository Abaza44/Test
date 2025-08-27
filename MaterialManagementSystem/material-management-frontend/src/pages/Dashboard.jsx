import React, { useState, useEffect } from 'react';
import { 
  Package, 
  Users, 
  ShoppingCart, 
  DollarSign, 
  TrendingUp, 
  TrendingDown,
  AlertTriangle,
  Eye,
  BarChart3
} from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell, LineChart, Line } from 'recharts';
import { formatCurrency, formatNumber, getStatusColor } from '../lib/utils';
import Layout from '../components/Layout/Layout';

/**
 * Dashboard page component
 * Displays overview statistics, charts, and key metrics for the material management system
 */
const Dashboard = () => {
  const [loading, setLoading] = useState(true);
  const [dashboardData, setDashboardData] = useState(null);

  /**
   * Mock dashboard data
   * In a real application, this would be fetched from the API
   */
  const mockDashboardData = {
    // Key Performance Indicators
    kpis: {
      totalMaterials: 156,
      totalClients: 89,
      monthlySales: 125000,
      monthlyPurchases: 85000,
      lowStockItems: 12,
      pendingInvoices: 8
    },
    
    // Sales trend data for the last 6 months
    salesTrend: [
      { month: 'يناير', sales: 95000, purchases: 65000 },
      { month: 'فبراير', sales: 110000, purchases: 75000 },
      { month: 'مارس', sales: 125000, purchases: 85000 },
      { month: 'أبريل', sales: 140000, purchases: 95000 },
      { month: 'مايو', sales: 135000, purchases: 90000 },
      { month: 'يونيو', sales: 125000, purchases: 85000 }
    ],
    
    // Top selling materials
    topMaterials: [
      { name: 'حديد التسليح 12 مم', quantity: 450, value: 45000 },
      { name: 'أسمنت بورتلاند', quantity: 320, value: 32000 },
      { name: 'رمل ناعم', quantity: 280, value: 14000 },
      { name: 'زلط', quantity: 250, value: 18750 },
      { name: 'طوب أحمر', quantity: 200, value: 16000 }
    ],
    
    // Stock status distribution
    stockStatus: [
      { name: 'متوفر', value: 120, color: '#10B981' },
      { name: 'كمية منخفضة', value: 24, color: '#F59E0B' },
      { name: 'نفدت الكمية', value: 12, color: '#EF4444' }
    ],
    
    // Recent activities
    recentActivities: [
      {
        id: 1,
        type: 'sale',
        description: 'فاتورة مبيعات جديدة #INV-2024-001',
        amount: 15000,
        time: '5 دقائق',
        client: 'شركة المقاولات المتحدة'
      },
      {
        id: 2,
        type: 'purchase',
        description: 'فاتورة شراء #PUR-2024-045',
        amount: 8500,
        time: '15 دقيقة',
        supplier: 'مصنع الحديد والصلب'
      },
      {
        id: 3,
        type: 'alert',
        description: 'تنبيه: مخزون منخفض - حديد التسليح 8 مم',
        time: '30 دقيقة'
      },
      {
        id: 4,
        type: 'payment',
        description: 'تحصيل من العميل: أحمد محمد',
        amount: 12000,
        time: '1 ساعة'
      }
    ],
    
    // Low stock alerts
    lowStockAlerts: [
      { material: 'حديد التسليح 8 مم', currentStock: 15, minStock: 50, unit: 'طن' },
      { material: 'أسمنت أبيض', currentStock: 8, minStock: 20, unit: 'شيكارة' },
      { material: 'دهان أبيض', currentStock: 5, minStock: 15, unit: 'جالون' }
    ]
  };

    /**
    * Simulates loading dashboard data
    */

  useEffect(() => {
    const loadDashboardData = async () => {
      setLoading(true);
      // Simulate API call delay
      await new Promise(resolve => setTimeout(resolve, 1000));
      setDashboardData(mockDashboardData);
      setLoading(false);
    };

    loadDashboardData();
  }, []);

  /**
   * Gets the appropriate icon for activity type
   */
  const getActivityIcon = (type) => {
    switch (type) {
      case 'sale': return <ShoppingCart className="w-4 h-4 text-green-600" />;
      case 'purchase': return <Package className="w-4 h-4 text-blue-600" />;
      case 'alert': return <AlertTriangle className="w-4 h-4 text-yellow-600" />;
      case 'payment': return <DollarSign className="w-4 h-4 text-purple-600" />;
      default: return <BarChart3 className="w-4 h-4 text-gray-600" />;
    }
  };

  if (loading) {
    return (
      <Layout>
        <div className="flex items-center justify-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="space-y-6">
        {/* Page Header */}
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold text-gray-900">لوحة التحكم</h1>
            <p className="text-gray-600">نظرة عامة على نشاط النظام والإحصائيات الرئيسية</p>
          </div>
          <div className="text-sm text-gray-500">
            آخر تحديث: {new Date().toLocaleString('ar-EG')}
          </div>
        </div>

        {/* KPI Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6">
          {/* Total Materials */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-blue-100 rounded-lg">
                <Package className="w-6 h-6 text-blue-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">إجمالي المواد</p>
                <p className="text-2xl font-bold text-gray-900">{dashboardData.kpis.totalMaterials}</p>
              </div>
            </div>
          </div>

          {/* Total Clients */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-green-100 rounded-lg">
                <Users className="w-6 h-6 text-green-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">إجمالي العملاء</p>
                <p className="text-2xl font-bold text-gray-900">{dashboardData.kpis.totalClients}</p>
              </div>
            </div>
          </div>

          {/* Monthly Sales */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-purple-100 rounded-lg">
                <TrendingUp className="w-6 h-6 text-purple-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">مبيعات الشهر</p>
                <p className="text-2xl font-bold text-gray-900">
                  {formatCurrency(dashboardData.kpis.monthlySales)}
                </p>
              </div>
            </div>
          </div>

          {/* Monthly Purchases */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-orange-100 rounded-lg">
                <TrendingDown className="w-6 h-6 text-orange-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">مشتريات الشهر</p>
                <p className="text-2xl font-bold text-gray-900">
                  {formatCurrency(dashboardData.kpis.monthlyPurchases)}
                </p>
              </div>
            </div>
          </div>

          {/* Low Stock Items */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-red-100 rounded-lg">
                <AlertTriangle className="w-6 h-6 text-red-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">مخزون منخفض</p>
                <p className="text-2xl font-bold text-gray-900">{dashboardData.kpis.lowStockItems}</p>
              </div>
            </div>
          </div>

          {/* Pending Invoices */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center">
              <div className="p-2 bg-yellow-100 rounded-lg">
                <Eye className="w-6 h-6 text-yellow-600" />
              </div>
              <div className="mr-4">
                <p className="text-sm font-medium text-gray-600">فواتير معلقة</p>
                <p className="text-2xl font-bold text-gray-900">{dashboardData.kpis.pendingInvoices}</p>
              </div>
            </div>
          </div>
        </div>

        {/* Charts Row */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Sales Trend Chart */}
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">اتجاه المبيعات والمشتريات</h3>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={dashboardData.salesTrend}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip formatter={(value) => formatCurrency(value)} />
                <Line type="monotone" dataKey="sales" stroke="#10B981" strokeWidth={2} name="المبيعات" />
                <Line type="monotone" dataKey="purchases" stroke="#3B82F6" strokeWidth={2} name="المشتريات" />
              </LineChart>
            </ResponsiveContainer>
          </div>

          {/* Stock Status Pie Chart */}
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">حالة المخزون</h3>
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={dashboardData.stockStatus}
                  cx="50%"
                  cy="50%"
                  labelLine={false}
                  label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                >
                  {dashboardData.stockStatus.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Bottom Row */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Top Materials */}
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">أكثر المواد مبيعاً</h3>
            <div className="space-y-4">
              {dashboardData.topMaterials.map((material, index) => (
                <div key={index} className="flex items-center justify-between">
                  <div>
                    <p className="text-sm font-medium text-gray-900">{material.name}</p>
                    <p className="text-xs text-gray-500">الكمية: {formatNumber(material.quantity)}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-semibold text-gray-900">
                      {formatCurrency(material.value)}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Recent Activities */}
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">الأنشطة الأخيرة</h3>
            <div className="space-y-4">
              {dashboardData.recentActivities.map((activity) => (
                <div key={activity.id} className="flex items-start space-x-3 space-x-reverse">
                  <div className="flex-shrink-0 mt-1">
                    {getActivityIcon(activity.type)}
                  </div>
                  <div className="flex-1">
                    <p className="text-sm text-gray-900">{activity.description}</p>
                    {activity.amount && (
                      <p className="text-sm font-semibold text-green-600">
                        {formatCurrency(activity.amount)}
                      </p>
                    )}
                    <p className="text-xs text-gray-500">منذ {activity.time}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Low Stock Alerts */}
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">تنبيهات المخزون المنخفض</h3>
            <div className="space-y-4">
              {dashboardData.lowStockAlerts.map((alert, index) => (
                <div key={index} className="p-3 bg-red-50 rounded-lg border border-red-200">
                  <p className="text-sm font-medium text-red-800">{alert.material}</p>
                  <div className="flex justify-between items-center mt-1">
                    <span className="text-xs text-red-600">
                      المتوفر: {alert.currentStock} {alert.unit}
                    </span>
                    <span className="text-xs text-red-600">
                      الحد الأدنى: {alert.minStock} {alert.unit}
                    </span>
                  </div>
                  <div className="w-full bg-red-200 rounded-full h-2 mt-2">
                    <div 
                      className="bg-red-600 h-2 rounded-full" 
                      style={{ width: `${(alert.currentStock / alert.minStock) * 100}%` }}
                    ></div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Dashboard;

