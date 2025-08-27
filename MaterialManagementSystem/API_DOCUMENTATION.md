# Material Management System - API Documentation

## 🔗 Base URL
```
Development: http://localhost:5000/api
Production: https://your-domain.com/api
```

## 🔐 Authentication

All API endpoints require authentication except for the login endpoint. Include the JWT token in the Authorization header:

```http
Authorization: Bearer <your-jwt-token>
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@materialmgmt.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "1",
    "email": "admin@materialmgmt.com",
    "fullName": "System Administrator",
    "role": "Manager"
  },
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

## 📦 Materials API

### Get All Materials
```http
GET /api/materials
Authorization: Bearer <token>
```

**Response:**
```json
[
  {
    "materialID": 1,
    "materialCode": "MAT001",
    "name": "حديد التسليح 12 مم",
    "categoryID": 1,
    "category": {
      "categoryID": 1,
      "name": "حديد التسليح"
    },
    "unit": "طن",
    "currentStock": 150.5,
    "minimumStock": 50.0,
    "sellingPrice": 25000.00,
    "location": "مخزن أ - رف 1",
    "isActive": true
  }
]
```

### Get Material by ID
```http
GET /api/materials/{id}
Authorization: Bearer <token>
```

### Create Material
```http
POST /api/materials
Authorization: Bearer <token>
Content-Type: application/json

{
  "materialCode": "MAT002",
  "name": "أسمنت بورتلاند",
  "categoryID": 2,
  "unit": "شيكارة",
  "minimumStock": 100.0,
  "sellingPrice": 85.00,
  "location": "مخزن ب - رف 2"
}
```

### Update Material
```http
PUT /api/materials/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "materialID": 1,
  "materialCode": "MAT001",
  "name": "حديد التسليح 12 مم محدث",
  "categoryID": 1,
  "unit": "طن",
  "minimumStock": 60.0,
  "sellingPrice": 26000.00,
  "location": "مخزن أ - رف 1",
  "isActive": true
}
```

### Delete Material
```http
DELETE /api/materials/{id}
Authorization: Bearer <token>
```

### Get Material Stock Status
```http
GET /api/materials/{id}/stock-status
Authorization: Bearer <token>
```

**Response:**
```json
{
  "currentStock": 150.5,
  "totalStockValue": 3762500.00,
  "weightedAverageCost": 25000.00,
  "lowestCost": 24500.00,
  "highestCost": 25500.00,
  "batchCount": 3
}
```

### Get Material Batches (FIFO)
```http
GET /api/materials/{id}/batches
Authorization: Bearer <token>
```

**Response:**
```json
[
  {
    "batchID": 1,
    "materialID": 1,
    "batchNumber": "BATCH001",
    "purchaseDate": "2024-01-01T00:00:00Z",
    "initialQuantity": 100.0,
    "remainingQuantity": 75.0,
    "unitCost": 24500.00,
    "expiryDate": null
  }
]
```

### Get Low Stock Materials
```http
GET /api/materials/low-stock
Authorization: Bearer <token>
```

### Get Expiring Materials
```http
GET /api/materials/expiring?daysAhead=30
Authorization: Bearer <token>
```

## 👥 Clients API

### Get All Clients
```http
GET /api/clients
Authorization: Bearer <token>
```

**Response:**
```json
[
  {
    "clientID": 1,
    "name": "شركة المقاولات المتحدة",
    "phone": "01234567890",
    "createdDate": "2024-01-01T00:00:00Z",
    "isActive": true,
    "addresses": [
      {
        "addressID": 1,
        "clientID": 1,
        "address": "شارع النيل، المعادي، القاهرة",
        "isDefault": true
      }
    ]
  }
]
```

### Get Client by ID
```http
GET /api/clients/{id}
Authorization: Bearer <token>
```

### Create Client
```http
POST /api/clients
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "شركة البناء الحديث",
  "phone": "01987654321",
  "addresses": [
    {
      "address": "شارع التحرير، وسط البلد، القاهرة",
      "isDefault": true
    }
  ]
}
```

### Update Client
```http
PUT /api/clients/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "clientID": 1,
  "name": "شركة المقاولات المتحدة المحدودة",
  "phone": "01234567890",
  "isActive": true,
  "addresses": [
    {
      "addressID": 1,
      "clientID": 1,
      "address": "شارع النيل، المعادي، القاهرة",
      "isDefault": true
    }
  ]
}
```

### Delete Client
```http
DELETE /api/clients/{id}
Authorization: Bearer <token>
```

### Search Clients
```http
GET /api/clients/search?searchTerm=شركة
Authorization: Bearer <token>
```

### Get Client Balance
```http
GET /api/clients/{id}/balance
Authorization: Bearer <token>
```

**Response:**
```json
{
  "clientID": 1,
  "clientName": "شركة المقاولات المتحدة",
  "totalSales": 150000.00,
  "totalCollections": 100000.00,
  "balance": 50000.00,
  "balanceStatus": "مدين"
}
```

### Get All Client Balances
```http
GET /api/clients/balances
Authorization: Bearer <token>
```

## 🚛 Suppliers API

### Get All Suppliers
```http
GET /api/suppliers
Authorization: Bearer <token>
```

### Get Supplier by ID
```http
GET /api/suppliers/{id}
Authorization: Bearer <token>
```

### Create Supplier
```http
POST /api/suppliers
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "مصنع الحديد والصلب",
  "phone": "01555666777",
  "address": "المنطقة الصناعية، العاشر من رمضان"
}
```

## 🛒 Sales API

### Get All Sales Invoices
```http
GET /api/sales
Authorization: Bearer <token>
```

### Get Sales Invoice by ID
```http
GET /api/sales/{id}
Authorization: Bearer <token>
```

### Create Sales Invoice
```http
POST /api/sales
Authorization: Bearer <token>
Content-Type: application/json

{
  "clientID": 1,
  "invoiceDate": "2024-01-15T00:00:00Z",
  "statusID": 2,
  "items": [
    {
      "materialID": 1,
      "quantity": 10.0,
      "unitPrice": 25000.00,
      "discount": 0.0
    }
  ]
}
```

## 📦 Purchases API

### Get All Purchase Invoices
```http
GET /api/purchases
Authorization: Bearer <token>
```

### Create Purchase Invoice
```http
POST /api/purchases
Authorization: Bearer <token>
Content-Type: application/json

{
  "supplierID": 1,
  "invoiceDate": "2024-01-15T00:00:00Z",
  "statusID": 2,
  "items": [
    {
      "materialID": 1,
      "quantity": 50.0,
      "unitCost": 24000.00,
      "batchNumber": "BATCH002",
      "expiryDate": null
    }
  ]
}
```

## 💰 Expenses API

### Get All Expenses
```http
GET /api/expenses
Authorization: Bearer <token>
```

### Create Expense
```http
POST /api/expenses
Authorization: Bearer <token>
Content-Type: application/json

{
  "expenseTypeID": 1,
  "amount": 5000.00,
  "description": "صيانة المعدات",
  "expenseDate": "2024-01-15T00:00:00Z"
}
```

## 👨‍💼 Employees API

### Get All Employees
```http
GET /api/employees
Authorization: Bearer <token>
```

### Create Employee
```http
POST /api/employees
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "أحمد محمد",
  "phone": "01122334455",
  "position": "مشرف مخزن",
  "salary": 8000.00,
  "hireDate": "2024-01-01T00:00:00Z"
}
```

## 🔧 Equipment API

### Get All Equipment
```http
GET /api/equipment
Authorization: Bearer <token>
```

### Create Equipment
```http
POST /api/equipment
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "رافعة شوكية",
  "model": "Toyota 8FG25",
  "serialNumber": "TY123456",
  "purchaseDate": "2023-06-01T00:00:00Z",
  "purchasePrice": 150000.00,
  "location": "المخزن الرئيسي"
}
```

## 📊 Reports API

### Sales Report
```http
GET /api/reports/sales?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer <token>
```

### Inventory Report
```http
GET /api/reports/inventory
Authorization: Bearer <token>
```

### Financial Report
```http
GET /api/reports/financial?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer <token>
```

## ❌ Error Responses

### 400 Bad Request
```json
{
  "error": "Validation failed",
  "details": [
    "Material code is required",
    "Selling price must be greater than 0"
  ]
}
```

### 401 Unauthorized
```json
{
  "error": "Authentication required",
  "message": "Please provide a valid JWT token"
}
```

### 403 Forbidden
```json
{
  "error": "Access denied",
  "message": "You don't have permission to access this resource"
}
```

### 404 Not Found
```json
{
  "error": "Resource not found",
  "message": "Material with ID 999 not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "Internal server error",
  "message": "An unexpected error occurred"
}
```

## 📝 Notes

1. All dates are in ISO 8601 format (UTC)
2. Decimal values use dot (.) as decimal separator
3. Currency amounts are in Egyptian Pounds (EGP)
4. All text fields support Arabic characters
5. File uploads are supported for invoice and equipment images
6. Rate limiting: 100 requests per minute per IP
7. Maximum request size: 10MB

## 🔄 Pagination

For endpoints that return lists, pagination is supported:

```http
GET /api/materials?page=1&pageSize=20&sortBy=name&sortOrder=asc
```

**Response:**
```json
{
  "data": [...],
  "pagination": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalItems": 100,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

## 🔍 Filtering

Most list endpoints support filtering:

```http
GET /api/materials?categoryId=1&isActive=true&minStock=10
GET /api/sales?clientId=1&startDate=2024-01-01&endDate=2024-01-31
```

