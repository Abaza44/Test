# دليل النشر - Material Management System Deployment Guide

هذا الدليل يوضح كيفية نشر نظام إدارة مواد البناء في بيئات مختلفة.

## 🚀 خيارات النشر

### 1. النشر باستخدام Docker (الموصى به)

#### المتطلبات
- Docker Engine 20.10+
- Docker Compose 2.0+
- 4GB RAM كحد أدنى
- 20GB مساحة تخزين

#### خطوات النشر
```bash
# 1. استنساخ المشروع
git clone <repository-url>
cd MaterialManagementSystem

# 2. إعداد متغيرات البيئة
cp .env.example .env
# تحرير ملف .env حسب البيئة

# 3. تشغيل النظام
docker-compose up -d

# 4. التحقق من حالة الخدمات
docker-compose ps

# 5. عرض السجلات
docker-compose logs -f
```

#### الوصول للنظام
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **Database**: localhost:1433

### 2. النشر على Azure

#### Azure Container Instances
```bash
# إنشاء مجموعة موارد
az group create --name material-mgmt-rg --location "East US"

# نشر قاعدة البيانات
az sql server create --name material-mgmt-sql --resource-group material-mgmt-rg --location "East US" --admin-user sqladmin --admin-password "MaterialMgmt123!"

# إنشاء قاعدة البيانات
az sql db create --resource-group material-mgmt-rg --server material-mgmt-sql --name MaterialManagementDB --service-objective Basic

# نشر Container Instances
az container create --resource-group material-mgmt-rg --name material-mgmt-app --image <your-registry>/material-mgmt:latest --dns-name-label material-mgmt --ports 80
```

#### Azure App Service
```bash
# إنشاء App Service Plan
az appservice plan create --name material-mgmt-plan --resource-group material-mgmt-rg --sku B1 --is-linux

# إنشاء Web App للـ Backend
az webapp create --resource-group material-mgmt-rg --plan material-mgmt-plan --name material-mgmt-api --deployment-container-image-name <your-registry>/material-mgmt-api:latest

# إنشاء Web App للـ Frontend
az webapp create --resource-group material-mgmt-rg --plan material-mgmt-plan --name material-mgmt-frontend --deployment-container-image-name <your-registry>/material-mgmt-frontend:latest
```

### 3. النشر على AWS

#### AWS ECS (Elastic Container Service)
```bash
# إنشاء ECS Cluster
aws ecs create-cluster --cluster-name material-mgmt-cluster

# إنشاء Task Definition
aws ecs register-task-definition --cli-input-json file://task-definition.json

# إنشاء Service
aws ecs create-service --cluster material-mgmt-cluster --service-name material-mgmt-service --task-definition material-mgmt-task --desired-count 1
```

#### AWS RDS للقاعدة
```bash
# إنشاء RDS Instance
aws rds create-db-instance --db-instance-identifier material-mgmt-db --db-instance-class db.t3.micro --engine sqlserver-ex --master-username admin --master-user-password MaterialMgmt123! --allocated-storage 20
```

### 4. النشر التقليدي (IIS + SQL Server)

#### متطلبات الخادم
- Windows Server 2019+
- IIS 10+
- .NET 8.0 Runtime
- SQL Server 2019+
- Node.js 20+ (للبناء)

#### خطوات النشر

##### Backend (API)
```bash
# 1. بناء التطبيق
cd Backend/MaterialManagementAPI
dotnet publish -c Release -o ./publish

# 2. نسخ الملفات إلى IIS
# نسخ محتويات مجلد publish إلى C:\inetpub\wwwroot\material-mgmt-api

# 3. إعداد IIS Application Pool
# - إنشاء Application Pool جديد
# - تعيين .NET CLR Version إلى "No Managed Code"
# - تعيين Process Model Identity إلى ApplicationPoolIdentity

# 4. إعداد IIS Site
# - إنشاء موقع جديد يشير إلى مجلد التطبيق
# - ربط Application Pool بالموقع
```

##### Frontend (React)
```bash
# 1. بناء التطبيق
cd material-management-frontend
npm install
npm run build

# 2. نسخ الملفات إلى IIS
# نسخ محتويات مجلد dist إلى C:\inetpub\wwwroot\material-mgmt-frontend

# 3. إعداد URL Rewrite لـ React Router
# تثبيت URL Rewrite Module وإضافة web.config
```

##### قاعدة البيانات
```sql
-- 1. إنشاء قاعدة البيانات
CREATE DATABASE MaterialManagementDB;

-- 2. تشغيل سكريبت الإنشاء
-- تشغيل ملف DEV_1_ABAZA_BULDING_MATRIAL.sql

-- 3. إنشاء مستخدم للتطبيق
CREATE LOGIN MaterialMgmtUser WITH PASSWORD = 'MaterialMgmt123!';
USE MaterialManagementDB;
CREATE USER MaterialMgmtUser FOR LOGIN MaterialMgmtUser;
ALTER ROLE db_owner ADD MEMBER MaterialMgmtUser;
```

## 🔧 إعدادات الإنتاج

### متغيرات البيئة

#### Backend
```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=your-server;Database=MaterialManagementDB;User Id=your-user;Password=your-password;TrustServerCertificate=true
JwtSettings__SecretKey=YourProductionSecretKeyThatIsAtLeast32CharactersLong!
JwtSettings__Issuer=MaterialManagementAPI
JwtSettings__Audience=MaterialManagementClient
JwtSettings__ExpirationInMinutes=60
```

#### Frontend
```env
VITE_API_BASE_URL=https://your-api-domain.com/api
```

### إعدادات الأمان

#### HTTPS Configuration
```bash
# إعداد SSL Certificate
# للـ Docker
docker run -d -p 443:443 -v /path/to/certs:/etc/nginx/ssl nginx

# للـ IIS
# ربط SSL Certificate بالموقع من IIS Manager
```

#### Firewall Rules
```bash
# فتح المنافذ المطلوبة
# Port 80 (HTTP)
# Port 443 (HTTPS)
# Port 1433 (SQL Server) - للخوادم الداخلية فقط
```

## 📊 مراقبة النظام

### Health Checks
```bash
# فحص حالة الخدمات
curl http://localhost:5000/health
curl http://localhost:3000/health
```

### Logging
```bash
# عرض سجلات Docker
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f database

# مسار السجلات في IIS
# C:\inetpub\logs\LogFiles\
```

### Performance Monitoring
```bash
# مراقبة استخدام الموارد
docker stats

# مراقبة قاعدة البيانات
# استخدام SQL Server Management Studio
# أو Azure Monitor للـ Azure SQL
```

## 🔄 النسخ الاحتياطي

### قاعدة البيانات
```sql
-- إنشاء نسخة احتياطية
BACKUP DATABASE MaterialManagementDB 
TO DISK = 'C:\Backup\MaterialManagementDB.bak'
WITH FORMAT, INIT;

-- استعادة النسخة الاحتياطية
RESTORE DATABASE MaterialManagementDB 
FROM DISK = 'C:\Backup\MaterialManagementDB.bak'
WITH REPLACE;
```

### ملفات التطبيق
```bash
# نسخ احتياطي للملفات المرفوعة
tar -czf uploads-backup-$(date +%Y%m%d).tar.gz /app/wwwroot/uploads/

# نسخ احتياطي لإعدادات Docker
docker-compose config > docker-compose-backup.yml
```

## 🚨 استكشاف الأخطاء

### مشاكل شائعة

#### خطأ الاتصال بقاعدة البيانات
```bash
# التحقق من حالة SQL Server
docker-compose exec database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P MaterialMgmt123! -Q "SELECT 1"

# فحص connection string
echo $ConnectionStrings__DefaultConnection
```

#### مشاكل CORS
```javascript
// إضافة CORS headers في Backend
app.UseCors(options => 
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
```

#### مشاكل Authentication
```bash
# التحقق من JWT Token
# فحص expiration date
# التأكد من صحة Secret Key
```

### أدوات التشخيص
```bash
# فحص حالة الشبكة
docker network ls
docker network inspect material-mgmt-network

# فحص استخدام الموارد
docker system df
docker system prune

# فحص السجلات التفصيلية
docker-compose logs --tail=100 -f
```

## 📈 تحسين الأداء

### Database Optimization
```sql
-- إضافة فهارس للاستعلامات الشائعة
CREATE INDEX IX_Materials_CategoryID ON Materials(CategoryID);
CREATE INDEX IX_SalesInvoices_ClientID ON SalesInvoices(ClientID);
CREATE INDEX IX_StockBatches_MaterialID ON StockBatches(MaterialID);
```

### Caching
```csharp
// إضافة Memory Caching في Backend
services.AddMemoryCache();
services.AddResponseCaching();
```

### CDN Configuration
```nginx
# إعداد Nginx للملفات الثابتة
location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
}
```

## 🔐 أمان الإنتاج

### Security Headers
```csharp
// إضافة Security Headers
app.UseSecurityHeaders(policies =>
    policies.AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin());
```

### Rate Limiting
```csharp
// إضافة Rate Limiting
services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});
```

---

**ملاحظة**: تأكد من تحديث جميع كلمات المرور والمفاتيح السرية قبل النشر في الإنتاج.

