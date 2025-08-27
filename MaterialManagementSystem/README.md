# نظام إدارة مواد البناء - Material Management System

نظام شامل لإدارة مواد البناء والمخزون مع واجهة عربية حديثة ونظام FIFO للمخزون.

## 📋 نظرة عامة

هذا النظام مصمم خصيصاً لشركات مواد البناء والمقاولات لإدارة:
- المواد والمخزون بنظام FIFO
- العملاء والموردين
- المبيعات والمشتريات
- المصروفات والرواتب
- المعدات والصيانة
- التقارير المالية والإحصائية

## 🏗️ البنية التقنية

### Backend (الخادم)
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Token
- **Architecture**: Clean Architecture with CQRS patterns

### Frontend (الواجهة)
- **Framework**: React 18 with Vite
- **Styling**: Tailwind CSS
- **UI Components**: shadcn/ui
- **Charts**: Recharts
- **Icons**: Lucide React
- **Language**: Arabic RTL Support

### Database (قاعدة البيانات)
- **Engine**: Microsoft SQL Server
- **Design**: Normalized relational database
- **Features**: FIFO inventory tracking, audit trails

## 🚀 التشغيل السريع

### المتطلبات الأساسية
- Docker & Docker Compose
- Git

### التشغيل باستخدام Docker
```bash
# استنساخ المشروع
git clone <repository-url>
cd MaterialManagementSystem

# تشغيل النظام كاملاً
docker-compose up -d

# الوصول للنظام
# Frontend: http://localhost:3000
# Backend API: http://localhost:5000
# Database: localhost:1433
```

### بيانات الدخول التجريبية
- **البريد الإلكتروني**: admin@materialmgmt.com
- **كلمة المرور**: Admin123!

## 🛠️ التطوير المحلي

### Backend Development
```bash
cd Backend/MaterialManagementAPI

# تثبيت المتطلبات
dotnet restore

# تشغيل قاعدة البيانات
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MaterialMgmt123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# تحديث إعدادات قاعدة البيانات في appsettings.json
# تشغيل التطبيق
dotnet run
```

### Frontend Development
```bash
cd material-management-frontend

# تثبيت المتطلبات
pnpm install

# تشغيل خادم التطوير
pnpm run dev
```

## 📊 الميزات الرئيسية

### إدارة المخزون
- ✅ نظام FIFO (First In, First Out) للمخزون
- ✅ تتبع دفعات المواد وتواريخ الانتهاء
- ✅ تنبيهات المخزون المنخفض
- ✅ تقارير حركة المخزون

### إدارة المبيعات
- ✅ فواتير المبيعات مع حساب التكلفة التلقائي
- ✅ إدارة العملاء والعناوين
- ✅ تتبع المدفوعات والمتبقي
- ✅ تقارير المبيعات

### إدارة المشتريات
- ✅ فواتير المشتريات
- ✅ إدارة الموردين
- ✅ تتبع المدفوعات للموردين
- ✅ تقارير المشتريات

### النظام المالي
- ✅ إدارة المصروفات
- ✅ رواتب الموظفين
- ✅ التقارير المالية
- ✅ حسابات العملاء والموردين

### إدارة المعدات
- ✅ سجل المعدات
- ✅ جدولة الصيانة
- ✅ تتبع تكاليف الصيانة

## 🔐 الأمان والصلاحيات

### الأدوار المتاحة
- **مدير (Manager)**: صلاحيات كاملة
- **مبيعات (Sales)**: إدارة المبيعات والعملاء والمواد
- **محاسب (Accountant)**: إدارة المالية والتقارير

### ميزات الأمان
- ✅ مصادقة JWT
- ✅ تشفير كلمات المرور
- ✅ صلاحيات مبنية على الأدوار
- ✅ تسجيل العمليات (Audit Trail)

## 📱 الواجهة والتجربة

### الميزات
- ✅ تصميم متجاوب (Responsive)
- ✅ دعم اللغة العربية مع RTL
- ✅ واجهة حديثة وسهلة الاستخدام
- ✅ رسوم بيانية تفاعلية
- ✅ تنبيهات فورية
- ✅ بحث متقدم

## 🗄️ قاعدة البيانات

### الجداول الرئيسية
- **Materials**: المواد
- **Clients**: العملاء
- **Suppliers**: الموردين
- **SalesInvoices**: فواتير المبيعات
- **PurchaseInvoices**: فواتير المشتريات
- **StockBatches**: دفعات المخزون
- **StockMovements**: حركات المخزون
- **Employees**: الموظفين
- **Equipment**: المعدات

## 🔧 إعدادات التكوين

### Backend Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MaterialManagementDB;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "MaterialManagementAPI",
    "Audience": "MaterialManagementClient",
    "ExpirationInMinutes": 60
  }
}
```

### Frontend Configuration (.env)
```env
VITE_API_BASE_URL=http://localhost:5000/api
```

## 📈 التقارير المتاحة

### تقارير المبيعات
- تقرير المبيعات اليومي/الشهري/السنوي
- تقرير أفضل العملاء
- تقرير أفضل المواد مبيعاً

### تقارير المخزون
- تقرير حالة المخزون
- تقرير المواد منتهية الصلاحية
- تقرير حركة المخزون

### التقارير المالية
- تقرير الأرباح والخسائر
- تقرير التدفق النقدي
- تقرير أعمار الديون

## 🧪 الاختبار

### اختبار Backend
```bash
cd Backend/MaterialManagementAPI
dotnet test
```

### اختبار Frontend
```bash
cd material-management-frontend
pnpm test
```

## 📦 النشر (Deployment)

### Docker Production
```bash
# إنتاج مع Nginx
docker-compose --profile production up -d
```

### Manual Deployment
1. بناء Backend: `dotnet publish -c Release`
2. بناء Frontend: `pnpm build`
3. نشر على الخادم المطلوب

## 🤝 المساهمة

### متطلبات التطوير
- .NET 8.0 SDK
- Node.js 20+
- SQL Server
- Visual Studio Code أو Visual Studio

### إرشادات الكود
- جميع التعليقات باللغة الإنجليزية
- أسماء المتغيرات والدوال بالإنجليزية
- واجهة المستخدم بالعربية
- اتباع معايير Clean Code

## 📞 الدعم

للحصول على الدعم أو الإبلاغ عن مشاكل:
- إنشاء Issue في GitHub
- مراجعة الوثائق
- فحص ملفات السجلات (Logs)

## 📄 الترخيص

هذا المشروع مرخص تحت رخصة MIT - راجع ملف [LICENSE](LICENSE) للتفاصيل.

## 🔄 تحديثات النسخة

### الإصدار 1.0.0
- ✅ النظام الأساسي مع جميع الميزات الرئيسية
- ✅ واجهة عربية كاملة
- ✅ نظام FIFO للمخزون
- ✅ تقارير شاملة
- ✅ نشر بـ Docker

---

**تم تطوير هذا النظام بعناية فائقة لتلبية احتياجات شركات مواد البناء في المنطقة العربية**

