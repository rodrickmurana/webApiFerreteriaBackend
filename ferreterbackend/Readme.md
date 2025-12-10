# 🛠️ Sistema de Ferretería – Backend (ASP.NET Core 8 Web API)

Backend completo para un sistema de ferretería desarrollado con **ASP.NET Core Web API**, **Entity Framework Core**, **SQL Server** y **JWT Authentication**.  
Incluye **roles (Admin / Cliente)**, **carrito de compras**, **favoritos**, **órdenes**, **productos**, **categorías**, **marcas**, **inventario**, **historial de compras** y más.

---

## 🚀 Tecnologías Utilizadas

- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server
- ASP.NET Identity (Usuarios y Roles)
- JWT Bearer Authentication
- Swagger
- C# 12
- JetBrains Rider / Visual Studio

---

## 📁 Estructura del Proyecto

```
ferreterbackend/
│
├── Controllers/
├── Models/
├── DTOs/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── DbSeeder.cs
│
├── Migrations/
├── Program.cs
├── appsettings.json
└── README.md
```

---

# 🔐 Autenticación y Autorización (RBAC)

Sistema basado en **JWT** + **Roles**.

### Roles:
- **Admin**
- **Cliente**

### Permisos

| Funcionalidad | Cliente | Admin |
|---------------|---------|-------|
| Ver productos | ✔ | ✔ |
| Agregar al carrito | ✔ | ❌ |
| Comprar | ✔ | ❌ |
| CRUD productos | ❌ | ✔ |
| CRUD categorías | ❌ | ✔ |
| CRUD marcas | ❌ | ✔ |
| Ver historial | ✔ | ✔ |

---

# 🧩 Base de Datos

La API genera automáticamente todas las tablas:

### Tablas principales
- Categorias
- Marcas
- Productos
- MetodosPago
- Carrito
- Favoritos
- Ordenes
- DetalleOrden
- Inventario

### Tablas Identity
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetRoleClaims
- AspNetUserLogins
- AspNetUserTokens

---

# 🌱 Seed Automático (Datos iniciales)

Al iniciar el proyecto se generan:

### Usuarios
- **Admin**
    - email: admin@ferreteria.com
    - pass: Admin123!
- **Clientes**
    - cliente1@test.com
    - cliente2@test.com
    - cliente3@test.com

### Datos Iniciales
- 10 Categorías
- 10 Marcas
- 10 Productos
- 10 Métodos de Pago
- Inventario automático
- Carritos iniciales
- Favoritos iniciales
- Órdenes + Detalles (historial real)

---

# 📌 Instalación y Configuración

## 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/tuusuario/ferreterbackend.git
cd ferreterbackend
```

## 2️⃣ Configurar la base de datos en `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=FerreteriaDB;Trusted_Connection=True;Encrypt=False;"
}
```

## 3️⃣ Ejecutar migraciones

```bash
dotnet ef migrations add Initial
dotnet ef database update
```

## 4️⃣ Ejecutar el proyecto

```bash
dotnet run
```

## 5️⃣ Abrir Swagger

```
https://localhost:7167/swagger
```

---

# 📚 Endpoints Principales

## 🔐 Auth
| Método | Endpoint | Rol |
|--------|----------|-----|
| POST | /api/Auth/register | Public |
| POST | /api/Auth/login | Public |

## 📦 Productos
| Método | Endpoint | Rol |
|--------|----------|------|
| GET | /api/Productos | Todos |
| GET | /api/Productos/{id} | Todos |
| POST | /api/Productos | Admin |
| PUT | /api/Productos/{id} | Admin |
| DELETE | /api/Productos/{id} | Admin |

## 🛒 Carrito
| Método | Endpoint | Rol |
|--------|----------|------|
| GET | /api/Carrito | Cliente |
| POST | /api/Carrito/add | Cliente |
| DELETE | /api/Carrito/remove/{id} | Cliente |

## ⭐ Favoritos
| Método | Endpoint | Rol |
|--------|----------|------|
| GET | /api/Favoritos | Cliente |
| POST | /api/Favoritos/add | Cliente |

## 🧾 Órdenes
| Método | Endpoint | Rol |
|--------|----------|------|
| GET | /api/Ordenes | Cliente / Admin |
| POST | /api/Ordenes/create | Cliente |

---

# ⚙️ JWT Authentication

Enviar el token en los headers:

```
Authorization: Bearer {token}
```

---

# 🧪 Pruebas recomendadas

1. Iniciar sesión como Admin y crear productos.
2. Iniciar sesión como Cliente y comprar productos.
3. Verificar que Cliente recibe `403 Forbidden` al intentar usar rutas de Admin.

---

# 📝 Requisitos Mínimos

- .NET SDK 8
- SQL Server
- JetBrains Rider o Visual Studio 2022

---

# 🧑‍💻 Contribuciones

Pull requests, mejoras y reportes de errores son bienvenidos.

---

# 📄 Licencia

Proyecto académico / educativo — libre para uso y modificación.
