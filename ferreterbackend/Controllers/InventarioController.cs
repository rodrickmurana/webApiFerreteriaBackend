using ferreterbackend.Data;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public InventarioController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("movimientos")]
        public async Task<IActionResult> GetMovimientos()
        {
            var movimientos = await _db.Inventario
                .Include(i => i.Producto)
                .ToListAsync();

            return Ok(movimientos);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarMovimiento(Inventario movimiento)
        {
            _db.Inventario.Add(movimiento);

            // Ajustar stock del producto
            var producto = await _db.Productos.FindAsync(movimiento.ProductoId);
            if (producto == null) return BadRequest("Producto no encontrado");

            if (movimiento.TipoMovimiento == "Entrada")
                producto.Stock += movimiento.Cantidad;
            else if (movimiento.TipoMovimiento == "Salida" || movimiento.TipoMovimiento == "Ajuste")
                producto.Stock -= movimiento.Cantidad;

            await _db.SaveChangesAsync();
            return Ok(movimiento);
        }
    }
}