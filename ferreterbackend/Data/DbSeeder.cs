using ferreterbackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await db.Database.MigrateAsync();

            // =====================================================
            // 1. ROLES
            // =====================================================
            string[] requiredRoles = { "Admin", "Cliente" };

            foreach (var role in requiredRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // =====================================================
            // 2. USUARIO ADMIN
            // =====================================================
            var adminEmail = "admin@ferreteria.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // =====================================================
            // 3. USUARIOS CLIENTES (3 usuarios)
            // =====================================================
            string[] clients =
            {
                "cliente1@test.com",
                "cliente2@test.com",
                "cliente3@test.com"
            };

            foreach (var email in clients)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email
                    };

                    await userManager.CreateAsync(newUser, "Cliente123!");
                    await userManager.AddToRoleAsync(newUser, "Cliente");
                }
            }

            await db.SaveChangesAsync();

            // =====================================================
            // 4. CATEGORÍAS (10)
            // =====================================================
            if (!db.Categorias.Any())
            {
                db.Categorias.AddRange(
                    new Categoria { Nombre = "Herramientas Manuales" },
                    new Categoria { Nombre = "Eléctricas" },
                    new Categoria { Nombre = "Construcción" },
                    new Categoria { Nombre = "Pinturas" },
                    new Categoria { Nombre = "Plomería" },
                    new Categoria { Nombre = "Carpintería" },
                    new Categoria { Nombre = "Seguridad Industrial" },
                    new Categoria { Nombre = "Iluminación" },
                    new Categoria { Nombre = "Jardinería" },
                    new Categoria { Nombre = "Automotriz" }
                );
                await db.SaveChangesAsync();
            }

            // =====================================================
            // 5. MARCAS (10)
            // =====================================================
            if (!db.Marcas.Any())
            {
                db.Marcas.AddRange(
                    new Marca { Nombre = "Tramontina" },
                    new Marca { Nombre = "Bosch" },
                    new Marca { Nombre = "Black & Decker" },
                    new Marca { Nombre = "Stanley" },
                    new Marca { Nombre = "Makita" },
                    new Marca { Nombre = "DeWalt" },
                    new Marca { Nombre = "3M" },
                    new Marca { Nombre = "Sika" },
                    new Marca { Nombre = "Philips" },
                    new Marca { Nombre = "Forte" }
                );
                await db.SaveChangesAsync();
            }

            // =====================================================
            // 6. PRODUCTOS (10)
            // =====================================================
            if (!db.Productos.Any())
            {
                var categorias = db.Categorias.ToList();
                var marcas = db.Marcas.ToList();

                db.Productos.AddRange(
                    new Producto { Nombre = "Martillo", Descripcion = "Martillo de acero", Precio = 30, Stock = 50, CategoriaId = categorias[0].Id, MarcaId = marcas[0].Id },
                    new Producto { Nombre = "Taladro", Descripcion = "Taladro eléctrico", Precio = 250, Stock = 15, CategoriaId = categorias[1].Id, MarcaId = marcas[1].Id },
                    new Producto { Nombre = "Pala", Descripcion = "Pala para construcción", Precio = 80, Stock = 20, CategoriaId = categorias[2].Id, MarcaId = marcas[2].Id },
                    new Producto { Nombre = "Brocha", Descripcion = "Brocha para pintar", Precio = 15, Stock = 100, CategoriaId = categorias[3].Id, MarcaId = marcas[3].Id },
                    new Producto { Nombre = "Llave inglesa", Descripcion = "Llave ajustable", Precio = 40, Stock = 30, CategoriaId = categorias[4].Id, MarcaId = marcas[4].Id },
                    new Producto { Nombre = "Sierra de mano", Descripcion = "Para carpintería", Precio = 60, Stock = 25, CategoriaId = categorias[5].Id, MarcaId = marcas[5].Id },
                    new Producto { Nombre = "Casco seguridad", Descripcion = "Protección industrial", Precio = 45, Stock = 40, CategoriaId = categorias[6].Id, MarcaId = marcas[6].Id },
                    new Producto { Nombre = "Lampara LED", Descripcion = "Iluminación", Precio = 35, Stock = 60, CategoriaId = categorias[7].Id, MarcaId = marcas[7].Id },
                    new Producto { Nombre = "Tijera podar", Descripcion = "Herramienta de jardín", Precio = 55, Stock = 18, CategoriaId = categorias[8].Id, MarcaId = marcas[8].Id },
                    new Producto { Nombre = "Gato hidráulico", Descripcion = "Levantar autos", Precio = 300, Stock = 10, CategoriaId = categorias[9].Id, MarcaId = marcas[9].Id }
                );
                await db.SaveChangesAsync();
            }

            // =====================================================
            // 7. INVENTARIO INICIAL POR PRODUCTO
            // =====================================================
            if (!db.Inventario.Any())
            {
                foreach (var p in db.Productos)
                {
                    db.Inventario.Add(new Inventario
                    {
                        ProductoId = p.Id,
                        Cantidad = p.Stock,
                        TipoMovimiento = "Inicial"
                    });
                }

                await db.SaveChangesAsync();
            }

            // =====================================================
            // 8. CARRITO INICIAL PARA CLIENTES
            // =====================================================
            var clientes = await userManager.GetUsersInRoleAsync("Cliente");

            if (!db.Carrito.Any())
            {
                foreach (var c in clientes)
                {
                    db.Carrito.Add(new Carrito
                    {
                        UsuarioId = c.Id,
                        ProductoId = 1,
                        Cantidad = 2
                    });
                }

                await db.SaveChangesAsync();
            }

            // =====================================================
            // 9. FAVORITOS INICIALES
            // =====================================================
            if (!db.Favoritos.Any())
            {
                foreach (var c in clientes)
                {
                    db.Favoritos.Add(new Favorito
                    {
                        UsuarioId = c.Id,
                        ProductoId = 2
                    });
                }

                await db.SaveChangesAsync();
            }

            // =====================================================
            // 10. ÓRDENES + DETALLES (HISTORIAL DE COMPRAS)
            // =====================================================
            if (!db.Ordenes.Any())
            {
                foreach (var c in clientes)
                {
                    var orden = new Orden
                    {
                        UsuarioId = c.Id,
                        MetodoPagoId = 1,
                        Total = 150,
                        FechaCreacion = DateTime.Now
                    };

                    db.Ordenes.Add(orden);
                    await db.SaveChangesAsync();

                    db.DetalleOrden.Add(new DetalleOrden
                    {
                        OrdenId = orden.Id,
                        ProductoId = 3,
                        Cantidad = 3,
                        PrecioUnitario = 50
                    });

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
