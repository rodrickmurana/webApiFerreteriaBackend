using ferreterbackend.Data;
using ferreterbackend.DTOs;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Cliente,Admin")]
    public class OrdenesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdenesController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost("crear")]
        public async Task<IActionResult> CrearOrden(CreateOrdenDto dto)
        {
            var userId = GetUserId();

            var carrito = await _db.Carrito
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            if (!carrito.Any())
                return BadRequest("El carrito está vacío");

            var orden = new Orden
            {
                UsuarioId = userId,
                MetodoPagoId = dto.MetodoPagoId,
                FechaCreacion = DateTime.Now,
                Total = 0,
                Detalles = new List<DetalleOrden>()
            };

            foreach (var item in carrito)
            {
                var detalle = new DetalleOrden
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Producto.Precio
                };

                orden.Detalles.Add(detalle);
                orden.Total += item.Cantidad * item.Producto.Precio;

                // Actualizar stock
                item.Producto.Stock -= item.Cantidad;
            }

            _db.Ordenes.Add(orden);
            _db.Carrito.RemoveRange(carrito);

            await _db.SaveChangesAsync();

            return Ok(orden);
        }

        [HttpGet("mis-ordenes")]
        public async Task<IActionResult> MisOrdenes()
        {
            var userId = GetUserId();

            var ordenes = await _db.Ordenes
                .Include(o => o.MetodoPago)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(o => o.UsuarioId == userId)
                .ToListAsync();

            return Ok(ordenes);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("todas")]
        public async Task<IActionResult> Todas()
        {
            var ordenes = await _db.Ordenes
                .Include(o => o.MetodoPago)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            return Ok(ordenes);
        }
    }
}
